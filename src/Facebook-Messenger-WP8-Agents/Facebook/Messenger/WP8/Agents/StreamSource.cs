using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Media;
using Facebook.Base.Logging;
using Microsoft.Phone.Reactive;

namespace Facebook.Messenger.WP8.Agents
{
	internal class StreamSource : MediaStreamSource
	{
		private MediaStreamDescription descriptor;

		private MemoryStream samples;

		private int delay = 1000;

		private long dt;

		private Stream Samples
		{
			get
			{
				if (samples == null)
				{
					samples = new MemoryStream(8192);
					samples.SetLength(samples.Capacity);
				}
				samples.Seek(0L, SeekOrigin.Begin);
				return samples;
			}
		}

		public event EventHandler StreamComplete;

		protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
		{
		}

		protected override void SeekAsync(long seekToTime)
		{
			ReportSeekCompleted(seekToTime);
		}

		protected override void OpenMediaAsync()
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected Obj, but got Unknown
			//IL_00b1: Expected Obj, but got Unknown
			Dictionary<MediaSourceAttributesKeys, string> dictionary = new Dictionary<MediaSourceAttributesKeys, string>();
			List<MediaStreamDescription> list = new List<MediaStreamDescription>();
			Dictionary<MediaStreamAttributeKeys, string> dictionary2 = new Dictionary<MediaStreamAttributeKeys, string>();
			dictionary.Add((MediaSourceAttributesKeys)0, "0");
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((ushort)1);
					binaryWriter.Write((ushort)1);
					binaryWriter.Write(8192u);
					binaryWriter.Write(8192u);
					binaryWriter.Write((ushort)1);
					binaryWriter.Write((ushort)8);
					binaryWriter.Write(0u);
					binaryWriter.Flush();
					dictionary2.Add((MediaStreamAttributeKeys)0, ToHexString(memoryStream.ToArray()));
				}
			}
			MediaStreamDescription val = new MediaStreamDescription((MediaStreamType)0, (IDictionary<MediaStreamAttributeKeys, string>)dictionary2);
			MediaStreamDescription item = val;
			descriptor = val;
			list.Add(item);
			ReportOpenMediaCompleted((IDictionary<MediaSourceAttributesKeys, string>)dictionary, (IEnumerable<MediaStreamDescription>)list);
		}

		protected override void GetSampleAsync(MediaStreamType mediaStreamType)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			ObservableExtensions.Subscribe<Unit>(Observable.SubscribeOn<Unit>(Observable.Return<Unit>(default(Unit)), (IScheduler)(object)Scheduler.NewThread), (Action<Unit>)((Unit _) =>
			{
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Expected Obj, but got Unknown
				Thread.Sleep(delay);
				delay = Math.Min(delay * 2, 520000);
				Dictionary<MediaSampleAttributeKeys, string> dictionary = new Dictionary<MediaSampleAttributeKeys, string>();
				MediaStreamSample val = new MediaStreamSample(descriptor, Samples, 0L, Samples.Length, dt, (IDictionary<MediaSampleAttributeKeys, string>)dictionary);
				dt++;
				ReportGetSampleCompleted(val);
			}));
		}

		protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
		{
		}

		private string ToHexString(byte[] bytes)
		{
			byte[] array = new byte[bytes.Length * 2];
			int num = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				int num2 = bytes[i];
				if (num2 < 0)
				{
					num2 += 256;
				}
				array[num] = ForDigit(num2 >> 4);
				num++;
				array[num] = ForDigit(num2 % 16);
				num++;
			}
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		private byte ForDigit(int b)
		{
			if (b < 10)
			{
				return (byte)(48 + b);
			}
			return (byte)(97 + b - 10);
		}

		public void Shutdown()
		{
			Logger.Info("[Audio Streamer Agent] Shutdown", "Shutdown", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\StreamSource.cs", 139);
			CloseStream();
		}

		protected override void CloseMedia()
		{
			Logger.Info("[Audio Streamer Agent] Close Media", "CloseMedia", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\StreamSource.cs", 145);
			CloseStream();
		}

		private void CloseStream()
		{
			if (StreamComplete != null)
			{
				StreamComplete(this, null);
			}
		}
	}
}
