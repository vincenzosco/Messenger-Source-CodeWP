using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Extensions;
using ImageTools.Helpers;
using ImageTools.IO;
using Windows.Storage;

namespace ImageTools;

[DebuggerDisplay("Image: {PixelWidth}x{PixelHeight}")]
public sealed class ExtendedImage : ImageBase
{
	public const double DefaultDensityX = 75.0;

	public const double DefaultDensityY = 75.0;

	private readonly object _lockObject = new object();

	private ImageFrameCollection _frames = new ImageFrameCollection();

	private ImagePropertyCollection _properties = new ImagePropertyCollection();

	private Uri _uriSource;

	public bool IsLoading { get; private set; }

	public double DensityX { get; set; }

	public double DensityY { get; set; }

	public double InchWidth
	{
		get
		{
			double num = DensityX;
			if (num <= 0.0)
			{
				num = 75.0;
			}
			return (double)PixelWidth / num;
		}
	}

	public double InchHeight
	{
		get
		{
			double num = DensityY;
			if (num <= 0.0)
			{
				num = 75.0;
			}
			return (double)PixelHeight / num;
		}
	}

	public bool IsAnimated => _frames.Count > 0;

	public ImageFrameCollection Frames => _frames;

	public ImagePropertyCollection Properties => _properties;

	public Uri UriSource
	{
		get
		{
			return _uriSource;
		}
		set
		{
			lock (_lockObject)
			{
				_uriSource = value;
				if (UriSource != null)
				{
					LoadAsync(UriSource);
				}
			}
		}
	}

	public event OpenReadCompletedEventHandler DownloadCompleted;

	public event DownloadProgressChangedEventHandler DownloadProgress;

	public event EventHandler LoadingCompleted;

	public event EventHandler<UnhandledExceptionEventArgs> LoadingFailed;

	private void OnDownloadCompleted(OpenReadCompletedEventArgs e)
	{
		DownloadCompleted?.Invoke(this, e);
	}

	private void OnDownloadProgress(DownloadProgressChangedEventArgs e)
	{
		DownloadProgress?.Invoke(this, e);
	}

	private void OnLoadingCompleted(EventArgs e)
	{
		LoadingCompleted?.Invoke(this, e);
	}

	private void OnLoadingFailed(UnhandledExceptionEventArgs e)
	{
		LoadingFailed?.Invoke(this, e);
	}

	public ExtendedImage(int width, int height)
		: base(width, height)
	{
		Contract.Requires<ArgumentException>(width >= 0, "Width must be greater or equals than zero.");
		Contract.Requires<ArgumentException>(height >= 0, "Height must be greater or equals than zero.");
		DensityX = 75.0;
		DensityY = 75.0;
	}

	public ExtendedImage(ExtendedImage other)
		: base(other)
	{
		Contract.Requires<ArgumentNullException>(other != null, "Other image cannot be null.");
		Contract.Requires<ArgumentException>(other.IsFilled, "Other image has not been loaded.");
		foreach (ImageFrame frame in other.Frames)
		{
			if (frame != null)
			{
				if (!frame.IsFilled)
				{
					throw new ArgumentException("The image contains a frame that has not been loaded yet.");
				}
				Frames.Add(new ImageFrame(frame));
			}
		}
		DensityX = 75.0;
		DensityY = 75.0;
	}

	public ExtendedImage()
	{
		DensityX = 75.0;
		DensityY = 75.0;
	}

	public ExtendedImage(Uri uri)
		: this()
	{
		UriSource = uri;
	}

	public void SetSource(Stream stream)
	{
		Contract.Requires<ArgumentNullException>(stream != null, "Stream cannot be null.");
		if (_uriSource == null)
		{
			LoadAsync(stream);
		}
	}

	private async Task Load(Stream stream)
	{
		int num = default;
		_ = num;
		_ = 0;
		try
		{
			if (!stream.CanRead)
			{
				throw new NotSupportedException("Cannot read from the stream.");
			}
			if (!stream.CanSeek)
			{
				throw new NotSupportedException("The stream does not support seeking.");
			}
			ReadOnlyCollection<IImageDecoder> decoders = Decoders.GetAvailableDecoders();
			if (decoders.Count > 0)
			{
				int maxHeaderSize = decoders.Max((IImageDecoder x) => x.HeaderSize);
				if (maxHeaderSize > 0)
				{
					byte[] header = new byte[maxHeaderSize];
					stream.Read(header, 0, maxHeaderSize);
					stream.Position = 0L;
					IImageDecoder decoder = decoders.FirstOrDefault((IImageDecoder x) => x.IsSupportedFileFormat(header));
					if (decoder != null)
					{
						await decoder.Decode(this, stream);
						IsLoading = false;
					}
				}
			}
			if (!IsLoading)
			{
				return;
			}
			IsLoading = false;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Image cannot be loaded. Available decoders:");
			foreach (IImageDecoder item in decoders)
			{
				stringBuilder.AppendLine("-" + item);
			}
			throw new UnsupportedImageFormatException(stringBuilder.ToString());
		}
		finally
		{
			stream.Dispose();
		}
	}

	private void LoadAsync(Stream stream)
	{
		IsLoading = true;
		ThreadPool.QueueUserWorkItem(async (object theStream) =>
		{
			Stream s = theStream as Stream;
			try
			{
				await Load(s);
				OnLoadingCompleted(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				OnLoadingFailed(new UnhandledExceptionEventArgs(exception, isTerminating: false));
			}
		}, stream);
	}

	private void LoadAsync(Uri uri)
	{
		try
		{
			bool flag = false;
			if (!uri.IsAbsoluteUri)
			{
				string text = uri.ToString();
				text = text.Replace("\\", "/");
				if (text.StartsWith("/", StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(1);
				}
				Stream localResourceStream = Extensions.GetLocalResourceStream(new Uri(text, UriKind.Relative));
				if (localResourceStream != null)
				{
					LoadAsync(localResourceStream);
					flag = true;
				}
			}
			if (!flag)
			{
				IsLoading = true;
				if (uri.IsFile)
				{
					OpenLocalStream(uri).DoNotWait();
					return;
				}
				WebClient webClient = new WebClient();
				webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
				webClient.OpenReadCompleted += webClient_OpenReadCompleted;
				webClient.OpenReadAsync(uri);
			}
		}
		catch (ArgumentException exception)
		{
			OnLoadingFailed(new UnhandledExceptionEventArgs(exception, isTerminating: false));
		}
		catch (InvalidOperationException exception2)
		{
			OnLoadingFailed(new UnhandledExceptionEventArgs(exception2, isTerminating: false));
		}
	}

	private async Task OpenLocalStream(Uri uri)
	{
		StorageFile file = await StorageFile.GetFileFromPathAsync(uri.LocalPath);
		if (file != null)
		{
			await Load(await ((IStorageFile)(object)file).OpenStreamForReadAsync());
			OnLoadingCompleted(EventArgs.Empty);
		}
	}

	private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
	{
		try
		{
			if (e.Error == null)
			{
				Stream result = e.Result;
				if (result != null)
				{
					LoadAsync(result);
				}
			}
			else
			{
				OnLoadingFailed(new UnhandledExceptionEventArgs(e.Error, isTerminating: false));
			}
			OnDownloadCompleted(e);
		}
		catch (WebException exception)
		{
			OnLoadingFailed(new UnhandledExceptionEventArgs(exception, isTerminating: false));
		}
	}

	private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		OnDownloadProgress(e);
	}

	public ExtendedImage Clone()
	{
		return new ExtendedImage(this);
	}

	public static ExtendedImage ApplyFilters(ExtendedImage source, params IImageFilter[] filters)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		Contract.Requires<ArgumentNullException>(filters != null, "Filters cannot be null.");
		Rectangle bounds = source.Bounds;
		foreach (IImageFilter filter in filters)
		{
			ExtendedImage source2 = source;
			Action<ImageBase, ImageBase> action = (ImageBase sourceImage, ImageBase targetImage) =>
			{
				filter.Apply(targetImage, sourceImage, bounds);
			};
			source = PerformAction(source2, clone: true, action);
		}
		return source;
	}

	public static ExtendedImage ApplyFilters(ExtendedImage source, Rectangle rectangle, params IImageFilter[] filters)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		Contract.Requires<ArgumentNullException>(filters != null, "Filters cannot be null.");
		foreach (IImageFilter filter in filters)
		{
			ExtendedImage source2 = source;
			Action<ImageBase, ImageBase> action = (ImageBase sourceImage, ImageBase targetImage) =>
			{
				filter.Apply(targetImage, sourceImage, rectangle);
			};
			source = PerformAction(source2, clone: true, action);
		}
		return source;
	}

	public static ExtendedImage Crop(ExtendedImage source, Rectangle bounds)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		return PerformAction(source, clone: false, (ImageBase sourceImage, ImageBase targetImage) =>
		{
			ImageBase.Crop(sourceImage, targetImage, bounds);
		});
	}

	public static ExtendedImage Transform(ExtendedImage source, RotationType rotationType, FlippingType flippingType)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		return PerformAction(source, clone: false, (ImageBase sourceImage, ImageBase targetImage) =>
		{
			ImageBase.Transform(sourceImage, targetImage, rotationType, flippingType);
		});
	}

	public static ExtendedImage Resize(ExtendedImage source, int width, int height, IImageResizer resizer)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		Contract.Requires<ArgumentNullException>(resizer != null, "Image Resizer cannot be null.");
		return PerformAction(source, clone: false, (ImageBase sourceImage, ImageBase targetImage) =>
		{
			resizer.Resize(sourceImage, targetImage, width, height);
		});
	}

	public static ExtendedImage Resize(ExtendedImage source, int size, IImageResizer resizer)
	{
		Contract.Requires<ArgumentNullException>(source != null, "Source image cannot be null.");
		Contract.Requires<ArgumentException>(source.IsFilled, "Source image has not been loaded.");
		Contract.Requires<ArgumentNullException>(resizer != null, "Image Resizer cannot be null.");
		int width = 0;
		int height = 0;
		float num = (float)source.PixelWidth / (float)source.PixelHeight;
		if (source.PixelWidth > source.PixelHeight && num > 0f)
		{
			width = size;
			height = (int)((float)width / num);
		}
		else
		{
			height = size;
			width = (int)((float)height * num);
		}
		return PerformAction(source, clone: false, (ImageBase sourceImage, ImageBase targetImage) =>
		{
			resizer.Resize(sourceImage, targetImage, width, height);
		});
	}

	private static ExtendedImage PerformAction(ExtendedImage source, bool clone, Action<ImageBase, ImageBase> action)
	{
		VerifyHasLoaded(source);
		ExtendedImage extendedImage = (clone ? new ExtendedImage(source) : new ExtendedImage());
		action(source, extendedImage);
		foreach (ImageFrame frame in source.Frames)
		{
			ImageFrame imageFrame = new ImageFrame();
			action(frame, imageFrame);
			if (!clone)
			{
				extendedImage.Frames.Add(imageFrame);
			}
		}
		return extendedImage;
	}

	private static void VerifyHasLoaded(ExtendedImage image)
	{
		if (!image.IsFilled)
		{
			throw new InvalidOperationException("Image has not been loaded");
		}
		foreach (ImageFrame frame in image.Frames)
		{
			if (frame != null && frame.IsFilled)
			{
				throw new InvalidOperationException("Not all frames has been loaded yet.");
			}
		}
	}
}
