using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Controls.Photos
{
	public class PhotoDrawingPopup : FacebookPopup, IPreferences
	{
		private enum EditMode
		{
			Initial,
			TextMode,
			DrawMode
		}

		private const string LastUsedEditModeKey = "PhotoDrawingLastUsedEditingMode";

		private const EditMode DefaultEditMode = EditMode.DrawMode;

		private const int DesiredTextPosition = 200;

		private const int BottomImageTextPadding = 100;

		private static readonly DependencyProperty CurrentEditModeProperty = DependencyProperty.Register("CurrentEditMode", typeof(EditMode), typeof(PhotoDrawingPopup), new PropertyMetadata((object)EditMode.Initial, (PropertyChangedCallback)((DependencyObject d, DependencyPropertyChangedEventArgs e) =>
		{
			((PhotoDrawingPopup)(object)d).UpdateControls();
		})));

		private EditMode _lastUsedEditMode;

		internal FacebookPopup PhotoDrawingPopupRoot;

		internal Storyboard SendAnimation;

		internal Storyboard ColorPickerFadeIn;

		internal DoubleAnimation UndoButtonAnimation;

		internal Storyboard ColorPickerFadeOut;

		internal Grid RootGrid;

		internal Grid ContentPanel;

		internal Grid ImageCanvas;

		internal Image Photo;

		internal DrawingOverlay DrawCanvas;

		internal DropShadowTextBox HintTextBlock;

		internal Grid PhotoTextGrid;

		internal DropShadowTextBox PhotoEditText;

		internal ImageButton RemoveTextButton;

		internal TranslateTransform TextBoxTranslateTransform;

		internal ColorPickerOverlay ColorPicker;

		internal FBMessengerProgressBar MessengerProgressBar;

		internal ImageButton UndoButton;

		internal Grid CustomApplicationBar;

		internal CircledIcon SendButton;

		internal CircledIcon TextButton;

		internal CircledIcon EditButton;

		private bool _contentLoaded;

		private EditMode CurrentEditMode
		{
			get
			{
				return (EditMode)((DependencyObject)this).GetValue(CurrentEditModeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(CurrentEditModeProperty, (object)value);
			}
		}

		public event EventHandler<BitmapSource> PhotoEditCompleted;

		public PhotoDrawingPopup(IApplicationBar applicationBar)
			: base(applicationBar)
		{
			InitializeComponent();
			DrawCanvas.SetDrawingSurface((UIElement)(object)ImageCanvas);
		}

		public void SetPhoto(PhotoData photoData)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected Obj, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected Obj, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			WriteableBitmap val = new WriteableBitmap(photoData.Source);
			RotationAngle rotationAngle = photoData.GetRotationAngle();
			bool flag = rotationAngle == RotationAngle.LandscapeLeft || rotationAngle == RotationAngle.LandscapeRight;
			int num = (flag ? ((BitmapSource)val).PixelHeight : ((BitmapSource)val).PixelWidth);
			int num2 = (flag ? ((BitmapSource)val).PixelWidth : ((BitmapSource)val).PixelHeight);
			double num3 = Math.Min(MaxContentPanelWidth / (double)num, MaxContentPanelHeight / (double)num2);
			int width = (int)((double)((BitmapSource)val).PixelWidth * num3);
			int height = (int)((double)((BitmapSource)val).PixelHeight * num3);
			val = val.Resize(width, height, WriteableBitmapExtensions.Interpolation.Bilinear);
			if (rotationAngle != RotationAngle.Portrait)
			{
				val = val.Rotate(rotationAngle.ToDegrees());
			}
			((FrameworkElement)ImageCanvas).Height = ((BitmapSource)val).PixelHeight;
			Grid imageCanvas = ImageCanvas;
			double width2 = (((FrameworkElement)HintTextBlock).Width = ((BitmapSource)val).PixelWidth);
			((FrameworkElement)imageCanvas).Width = width2;
			Grid imageCanvas2 = ImageCanvas;
			RectangleGeometry val2 = new RectangleGeometry();
			val2.Rect = new Rect(0.0, 0.0, (double)((BitmapSource)val).PixelWidth, (double)((BitmapSource)val).PixelHeight);
			((UIElement)imageCanvas2).Clip = (Geometry)(object)val2;
			Photo.Source = (ImageSource)(object)val;
			CurrentEditMode = _lastUsedEditMode;
		}

		private void DrawCanvasOnDrawActionStarted(object sender, EventArgs eventArgs)
		{
			ColorPickerFadeOut.Begin();
		}

		private void DrawCanvasOnDrawActionComplete(object sender, EventArgs eventArgs)
		{
			UpdateUndoButton(DrawCanvas.HasDrawings());
			FadeInColorPicker();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)ContentPanel).Width = MaxContentPanelWidth;
			((FrameworkElement)ContentPanel).Height = MaxContentPanelHeight;
		}

		private void SendAnimation_OnCompleted(object sender, EventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			if (PhotoEditCompleted != null)
			{
				WriteableBitmap e2 = new WriteableBitmap((UIElement)(object)ImageCanvas, (Transform)null);
				PhotoEditCompleted(this, (BitmapSource)(object)e2);
			}
		}

		protected override void Reset()
		{
			ResetContentPanelLocation();
			((UIElement)ColorPicker).Opacity = 1.0;
			((UIElement)HintTextBlock).Opacity = 1.0;
			Photo.Source = null;
			ColorPicker.Reset();
			DrawCanvas.Reset();
			PhotoEditText.ClearText();
			CurrentEditMode = EditMode.Initial;
			((UIElement)UndoButton).Visibility = (Visibility)1;
			((UIElement)HintTextBlock).Visibility = (Visibility)1;
			UpdateUndoButton(false);
			UpdateRemoveButton();
		}

		private void ResetContentPanelLocation()
		{
			Transform renderTransform = ((UIElement)ContentPanel).RenderTransform;
			CompositeTransform val = (CompositeTransform)(object)((renderTransform is CompositeTransform) ? renderTransform : null);
			if (val != null)
			{
				val.TranslateY = 0.0;
			}
		}

		private void UpdateControls()
		{
			switch (CurrentEditMode)
			{
			case EditMode.TextMode:
				ColorPickerFadeOut.Begin();
				((UIElement)PhotoTextGrid).IsHitTestVisible = true;
				((UIElement)PhotoEditText).IsHitTestVisible = true;
				TextButton.IsSelected = true;
				EditButton.IsSelected = false;
				((Control)DrawCanvas).IsEnabled = false;
				((UIElement)UndoButton).Visibility = (Visibility)1;
				((Control)PhotoEditText).IsEnabled = true;
				break;
			case EditMode.DrawMode:
				FadeInColorPicker();
				((UIElement)PhotoTextGrid).IsHitTestVisible = false;
				((UIElement)PhotoEditText).IsHitTestVisible = false;
				TextButton.IsSelected = false;
				EditButton.IsSelected = true;
				((Control)DrawCanvas).IsEnabled = true;
				((UIElement)UndoButton).Visibility = (Visibility)0;
				((Control)PhotoEditText).IsEnabled = false;
				break;
			}
			UpdateHintTextBlock();
			UpdateRemoveButton();
		}

		private void FadeInColorPicker()
		{
			UndoButtonAnimation.To = (((Control)UndoButton).IsEnabled ? 1.0 : 0.5);
			ColorPickerFadeIn.Begin();
		}

		private void UpdateUndoButton(bool isEnabled)
		{
			((Control)UndoButton).IsEnabled = isEnabled;
			((UIElement)UndoButton).Opacity = (isEnabled ? 1.0 : 0.5);
		}

		private void Canvas_OnTap(object sender, GestureEventArgs e)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (CurrentEditMode == EditMode.TextMode && string.IsNullOrEmpty(PhotoEditText.Text))
			{
				Point val = TransformToCoordinateSpace((UIElement)(object)RootGrid, (UIElement)(object)ImageCanvas, e.GetPosition((UIElement)null));
				double textBoxOffsetY = val.Y - ((FrameworkElement)PhotoEditText).ActualHeight / 2.0;
				ConstrainTextBoxToCanvas(textBoxOffsetY);
				((Control)PhotoEditText).Focus();
			}
		}

		private void PhotoTextGrid_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			if (CurrentEditMode == EditMode.TextMode)
			{
				MoveTextUsingManipulation(e);
			}
		}

		private void MoveTextUsingManipulation(ManipulationDeltaEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Point manipulationOrigin = e.ManipulationOrigin;
			double y = manipulationOrigin.Y;
			Point translation = e.DeltaManipulation.Translation;
			Point p = new Point(0.0, y + translation.Y);
			Point val = TransformToCoordinateSpace(e.ManipulationContainer, (UIElement)(object)ImageCanvas, p);
			double textBoxOffsetY = val.Y - ((FrameworkElement)PhotoEditText).ActualHeight / 2.0;
			ConstrainTextBoxToCanvas(textBoxOffsetY);
		}

		private void PhotoEditText_OnGotFocus(object sender, RoutedEventArgs e)
		{
			CenterText();
			((UIElement)HintTextBlock).Visibility = (Visibility)1;
		}

		private void CenterText()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			Transform renderTransform = ((UIElement)ContentPanel).RenderTransform;
			CompositeTransform val = (CompositeTransform)(object)((renderTransform is CompositeTransform) ? renderTransform : null);
			if (val != null)
			{
				Point val2 = TransformToCoordinateSpace((UIElement)(object)PhotoEditText, (UIElement)(object)ContentPanel, new Point(0.0, 0.0));
				if (val2.Y < 0.0)
				{
					val2 = TransformToCoordinateSpace((UIElement)(object)ImageCanvas, (UIElement)(object)ContentPanel, new Point(0.0, ((FrameworkElement)ImageCanvas).ActualHeight - 100.0));
				}
				val.TranslateY = 200.0 - val2.Y;
			}
		}

		private void PhotoEditText_OnLostFocus(object sender, RoutedEventArgs e)
		{
			ResetContentPanelLocation();
			UpdateHintTextBlock();
		}

		private void PhotoEditText_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateRemoveButton();
		}

		private void UpdateHintTextBlock()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)HintTextBlock).Visibility = VisibilityUtil.FromBool(string.IsNullOrEmpty(PhotoEditText.Text) && CurrentEditMode == EditMode.TextMode);
		}

		private void UpdateRemoveButton()
		{
			bool flag = !string.IsNullOrEmpty(PhotoEditText.Text) && CurrentEditMode == EditMode.TextMode;
			((Control)RemoveTextButton).IsEnabled = flag;
			((UIElement)RemoveTextButton).Opacity = (flag ? 1.0 : 0.0);
		}

		private void RemoveTextButton_OnClick(object sender, RoutedEventArgs e)
		{
			PhotoEditText.ClearText();
		}

		private void PhotoTextGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			double y = TextBoxTranslateTransform.Y;
			ConstrainTextBoxToCanvas(y);
		}

		private void ConstrainTextBoxToCanvas(double textBoxOffsetY)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			textBoxOffsetY = Math.Max(0.0, textBoxOffsetY);
			double num = ((FrameworkElement)ImageCanvas).ActualHeight - ((FrameworkElement)PhotoEditText).ActualHeight;
			Thickness margin = ((FrameworkElement)RemoveTextButton).Margin;
			textBoxOffsetY = Math.Min(num - margin.Top, textBoxOffsetY);
			if (textBoxOffsetY + ((FrameworkElement)PhotoEditText).ActualHeight / 2.0 > ((FrameworkElement)ImageCanvas).ActualHeight / 2.0)
			{
				textBoxOffsetY -= ((FrameworkElement)RemoveTextButton).ActualHeight;
				Grid.SetRow((FrameworkElement)(object)RemoveTextButton, 0);
				Grid.SetRow((FrameworkElement)(object)PhotoEditText, 1);
			}
			else
			{
				Grid.SetRow((FrameworkElement)(object)PhotoEditText, 0);
				Grid.SetRow((FrameworkElement)(object)RemoveTextButton, 1);
			}
			TextBoxTranslateTransform.Y = textBoxOffsetY;
		}

		private Point TransformToCoordinateSpace(UIElement source, UIElement destination, Point p)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (!IsOpen)
			{
				return p;
			}
			GeneralTransform val = source.TransformToVisual(destination);
			return val.Transform(p);
		}

		private void UndoButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (DrawCanvas.HasDrawings())
			{
				DrawCanvas.RemoveLastDrawing();
				((Control)MessengerProgressBar).Focus();
				UpdateUndoButton(DrawCanvas.HasDrawings());
			}
		}

		private void SendButton_OnClick(object sender, RoutedEventArgs e)
		{
			((UIElement)MessengerProgressBar).Visibility = (Visibility)0;
			MessengerProgressBar.IsIndeterminate = true;
			((Control)MessengerProgressBar).Focus();
			((Control)RemoveTextButton).IsEnabled = false;
			((UIElement)RemoveTextButton).Opacity = 0.0;
			((UIElement)HintTextBlock).Visibility = (Visibility)1;
			MessengerProgressBar.IsIndeterminate = false;
			SendAnimation.Begin();
		}

		private void TextButton_OnClick(object sender, RoutedEventArgs e)
		{
			CurrentEditMode = EditMode.TextMode;
		}

		private void EditButton_OnClick(object sender, RoutedEventArgs e)
		{
			CurrentEditMode = EditMode.DrawMode;
		}

		public void RestorePreferences(IDictionary<string, string> preferences)
		{
			if (preferences == null || !preferences.ContainsKey("PhotoDrawingLastUsedEditingMode") || preferences["PhotoDrawingLastUsedEditingMode"] == null)
			{
				_lastUsedEditMode = EditMode.DrawMode;
			}
			else
			{
				Enum.TryParse<EditMode>(preferences["PhotoDrawingLastUsedEditingMode"], out _lastUsedEditMode);
			}
		}

		public IDictionary<string, string> GetPreferences()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("PhotoDrawingLastUsedEditingMode", CurrentEditMode.ToString());
			return dictionary;
		}

		public IList<string> GetPreferenceKeys()
		{
			List<string> list = new List<string>();
			list.Add("PhotoDrawingLastUsedEditingMode");
			return list;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected Obj, but got Unknown
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Expected Obj, but got Unknown
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Photos/PhotoDrawingPopup.xaml", UriKind.Relative));
				PhotoDrawingPopupRoot = (FacebookPopup)((FrameworkElement)this).FindName("PhotoDrawingPopupRoot");
				SendAnimation = (Storyboard)((FrameworkElement)this).FindName("SendAnimation");
				ColorPickerFadeIn = (Storyboard)((FrameworkElement)this).FindName("ColorPickerFadeIn");
				UndoButtonAnimation = (DoubleAnimation)((FrameworkElement)this).FindName("UndoButtonAnimation");
				ColorPickerFadeOut = (Storyboard)((FrameworkElement)this).FindName("ColorPickerFadeOut");
				RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
				ContentPanel = (Grid)((FrameworkElement)this).FindName("ContentPanel");
				ImageCanvas = (Grid)((FrameworkElement)this).FindName("ImageCanvas");
				Photo = (Image)((FrameworkElement)this).FindName("Photo");
				DrawCanvas = (DrawingOverlay)((FrameworkElement)this).FindName("DrawCanvas");
				HintTextBlock = (DropShadowTextBox)((FrameworkElement)this).FindName("HintTextBlock");
				PhotoTextGrid = (Grid)((FrameworkElement)this).FindName("PhotoTextGrid");
				PhotoEditText = (DropShadowTextBox)((FrameworkElement)this).FindName("PhotoEditText");
				RemoveTextButton = (ImageButton)((FrameworkElement)this).FindName("RemoveTextButton");
				TextBoxTranslateTransform = (TranslateTransform)((FrameworkElement)this).FindName("TextBoxTranslateTransform");
				ColorPicker = (ColorPickerOverlay)((FrameworkElement)this).FindName("ColorPicker");
				MessengerProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("MessengerProgressBar");
				UndoButton = (ImageButton)((FrameworkElement)this).FindName("UndoButton");
				CustomApplicationBar = (Grid)((FrameworkElement)this).FindName("CustomApplicationBar");
				SendButton = (CircledIcon)((FrameworkElement)this).FindName("SendButton");
				TextButton = (CircledIcon)((FrameworkElement)this).FindName("TextButton");
				EditButton = (CircledIcon)((FrameworkElement)this).FindName("EditButton");
			}
		}

		static PhotoDrawingPopup()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected Obj, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
		}
	}
}
