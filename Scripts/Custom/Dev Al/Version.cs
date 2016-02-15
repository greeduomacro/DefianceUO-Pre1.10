//Al@2006-05-09
using System;
using System.Text.RegularExpressions;

namespace Server.Scripts.Commands
{
    public class Version
    {
        private static String ms_version;
        private static Regex ms_regexHeadURL = new Regex(@"\$HeadURL: svn\+ssh:\/\/[^\/]+(?<version>\/.*)\/Custom\/.+\/Version.cs");
        private static String ms_headURL = "$HeadURL: https://svn.defianceuo.com/uor/trunk/Custom/Dev%20Al/Version.cs $";

        public static void Initialize()
        {
            Server.Commands.Register("Version", AccessLevel.Seer, new CommandEventHandler(Version_OnCommand));
            Match m = ms_regexHeadURL.Match(ms_headURL);
            ms_version=m.Groups["version"].Value;
            if (ms_version == "") ms_version = ms_headURL;
        }

        [Usage("Version")]
        [Description("Displays script versioning information.")]
        public static void Version_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage(ms_version);
        }
    }
}