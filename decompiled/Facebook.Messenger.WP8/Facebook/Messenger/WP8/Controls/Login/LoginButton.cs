using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls.Login
{
	public class LoginButton : Button
	{
		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoginButton), new PropertyMetadata((object)false, (PropertyChangedCallback)IsEnabledPropertyChanged));

		public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LoginButton), new PropertyMetadata((object)string.Empty, (PropertyChangedCallback)IsEnabledPropertyChanged));

		public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LoginButton), new PropertyMetadata((object)string.Empty, (PropertyChangedCallback)IsEnabledPropertyChanged));

		private bool _contentLoaded;

		public bool IsLoading
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsLoadingProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsLoadingProperty, (object)value);
			}
		}

		public string Username
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(UsernameProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(UsernameProperty, (object)value);
			}
		}

		public string Password
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(PasswordProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(PasswordProperty, (object)value);
			}
		}

		public LoginButton()
		{
			InitializeComponent();
		}

		private static void IsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			LoginButton loginButton = d as LoginButton;
			if (loginButton != null)
			{
				((Control)loginButton).IsEnabled = !loginButton.IsLoading && !string.IsNullOrWhiteSpace(loginButton.Username) && !string.IsNullOrWhiteSpace(loginButton.Password);
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Login/LoginButton.xaml", UriKind.Relative));
			}
		}

		static LoginButton()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected Obj, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected Obj, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected Obj, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected Obj, but got Unknown
		}
	}
}
