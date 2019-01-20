using System;
using Records.Common;
using Xwt;

namespace Records.Windows
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App.Run(ToolkitType.Wpf);
        }

        /// <summary>
		/// Gets or sets the System.Windows.Controls.WebView Emulation mode
		/// </summary>
		/// <remarks>This is a simple example on how to change the WebView emulation mode</remarks>
		public static IEEmulationMode WebViewEmulationMode
		{
			get
			{
				var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
				if (regKey == null)
					return IEEmulationMode.Default;

				string myProgramName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
				var currentValue = regKey.GetValue(myProgramName);
				return currentValue != null ? (IEEmulationMode)currentValue : IEEmulationMode.Default;
			}
			set
			{
				var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
				if (regKey == null)
					regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BEHAVIORS", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

				string executableName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
				var currentValue = regKey.GetValue(executableName);
				if (currentValue == null || (int)currentValue != (int)value)
					regKey.SetValue(executableName, (int)value, Microsoft.Win32.RegistryValueKind.DWord);
			}
		}

		/// <summary>Internet Explorer Emulation Modes</summary>
		/// <remarks>https://msdn.microsoft.com/de-de/library/ee330730.aspx#browser_emulation</remarks>
		public enum IEEmulationMode
		{
			IE7 = 0x00001b58,
			IE8 = 0x00001f40,
			IE8Force = 0x000022b8,
			IE9 = 0x00002328,
			IE9Force = 0x0000270f,
			IE10 = 0x00002710,
			IE10Force = 0x00002711,
			IE11 = 0x00002af8,
			IE11Force = 0x00002af9,
			Default = IE7,
		}
    }
}
