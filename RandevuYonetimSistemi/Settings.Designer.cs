
namespace RandevuYonetimSistemi
{
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(
        "Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator",
        "17.14.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        private static Settings defaultInstance =
            ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get { return defaultInstance; }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("false")]
        public bool IsLoggedIn
        {
            get { return ((bool)(this["IsLoggedIn"])); }
            set { this["IsLoggedIn"] = value; }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int UserId
        {
            get { return ((int)(this["UserId"])); }
            set { this["UserId"] = value; }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Email
        {
            get { return ((string)(this["Email"])); }
            set { this["Email"] = value; }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string JwtToken
        {
            get { return ((string)(this["JwtToken"])); }
            set { this["JwtToken"] = value; }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0001-01-01T00:00:00")]
        public global::System.DateTime TokenExpiry
        {
            get { return ((global::System.DateTime)(this["TokenExpiry"])); }
            set { this["TokenExpiry"] = value; }
        }
    }
}
