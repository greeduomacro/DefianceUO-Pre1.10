using System;
using System.IO;
using Server.Network;
using Server.Gumps;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Misc
{
    public class ToSRulesChecker
    {
        private static string m_Directory = Path.Combine(Core.BaseDirectory, "Saves");
        private static string m_FilePath = Path.Combine(Core.BaseDirectory, "Saves\\TosRules.bin");

        public static int Version = 0; // Increment on new rules

        private static string m_Rules = @"1.) Harrassment [J]
No racial slurs or harassing, including but not limited to, following a player repeatedly to spam insults.

2.) Amount of clients [J]
No more than 1 client online in arena events such as tournaments, capture the flags, last man standing etc.

3.) Trapping [J]
No boxing/trapping of players. There has to be 1 route of escape that is free of blocking items, including when you gate people with the intent to kill them.

4.) Bug Exploiting [b]
No exploiting/abuse of ingame bugs. Should you discover a bug we ask you kindly to report it at our forums.

5.) Advertisement [J]/[b]
No advertising of other servers or products.
This does not include UO items to be sold in or out of DefianceNET. The Spam rule is your previous advert must of expired before you can repeat it.

6.) Third party programs [J]/[b]
No 3rd party programs, that includes but is not limited to PlayUO & Speedhacks of any type, however we do permit the use of Razor and EasyUO, abusing RaZoR Resync is Illegal however.

7.) Hacking [b]
No hacking or guessing passwords tolerated.

8.) Unattended Macroing [J]/[E1]
Collecting resources while being afk (unattended macroing) is allowed within realistic reason i.e. no account abuse macroing. This rule applies to all resources (ore, wood, cotton, gold etc.).
You may not create more than two accounts to farm resources off them.

9.) Disrespect towards staffmembers [J]
We do not tolerate any disrespecting actions towards any of our staffmembers, this includes badmouthing.

10.) Bod collecting [b]
You are allowed to have no more than 5 characters that can collect tailoring and/or blacksmith bods.

11.) Character Creation Abuse [J]/[b]
You are not allowed to create newbie characters only with the intent to kill AFK players in town.
You are not allowed to create more than two accounts with the intent of farming the newbie resources from them.

12.) Scripts and Plugins [b]
No use of automatted healing/loading/dropping/potion chug scripts in PVP combat. (This rule is about intent, please read the examples)

Example1: You setup a healing loop macro in your house with the intent to gain skill - this is no violation since your intent was to macro and you did it at a quiet location.

[b] Example2: You setup a gating macro at a town with a loop to heal/cure yourself if you lose HP - this is a violation since your intent is to survive PVP while gating afk.

[b] Example3: You have a script that auto-heals party members, or auto-drops enemies with your party - this is a violation since your intent is to PVP.

13.) Read the ToS for our forums before registering.

Having an account within Defiance Network is a privilege. This privilege may be withdrawn at any time by any of the Defiance staff, should they find you breaking these simple rules, or in general spending your time being a problem to the staff/shard. The punishment as stated at each rule is a guideline and may vary when your situation is being reviewed. Consecutively breaking one of the jailable rules will automatically result in a ban.

By playing on one of our game servers or participating on one of our forums you automaticly agree on our terms of service and rules.

You can review the shard rules at any time by using the [ShardRules command.
";

        public static string Rules { get { return m_Rules; } }

        public static void Configure()
        {
            ColorizeMessage();
            EventSink.CharacterCreated += new CharacterCreatedEventHandler(OnCharacterCreated);
            EventSink.Login += new LoginEventHandler(OnLogin);
            Server.Commands.Register( "ShardRules", AccessLevel.Player, new CommandEventHandler( OnCommand ) );
            EventSink.WorldLoad += new WorldLoadEventHandler(Deserialize);
            EventSink.WorldSave += new WorldSaveEventHandler(Serialize);
        }

        private static void ColorizeMessage()
        {
            string endbasefont = "<basefont color=#ffffff>";

            m_Rules = m_Rules.Replace("[J]", GetBaseFont(3) + "[Jail]" + endbasefont);
            m_Rules = m_Rules.Replace("[b]", GetBaseFont(2) + "[Ban]" + endbasefont);
            m_Rules = m_Rules.Replace("[E1]", GetBaseFont(2) + "[Resources Removal]" + endbasefont);

        }

        private static string GetBaseFont(int color)
        {
            string htmlcolor = "";

            switch (color)
            {
                case 0: htmlcolor = "ffffff"; break;//white
                case 1: htmlcolor = "00ffff"; break;//blue
                case 2: htmlcolor = "ff0000"; break;//red
                case 3: htmlcolor = "ffff00"; break;//yellow
            }

            return String.Format("<basefont color=#{0}>", htmlcolor);
        }

        private static void OnCharacterCreated(CharacterCreatedEventArgs args)
        {
            Timer.DelayCall(TimeSpan.FromMinutes(1.5), new TimerStateCallback(SendGumpTo), args.Mobile);
        }

        [Usage("ShardRules")]
        [Description("Display the shardrules")]
        private static void OnCommand(CommandEventArgs args)
        {
            Mobile m = args.Mobile;

            if (m != null)
                m.SendGump(new ToSGump());
        }

        private static void OnLogin(LoginEventArgs args)
        {
            Mobile m = args.Mobile;
            Account acct = (Account)m.Account;

            if (m != null && !Convert.ToBoolean(acct.GetTag("ToS_accepted")))
                m.SendGump(new ToSGump());
        }

        private static void SendGumpTo(object o)
        {
            Mobile m = (Mobile)o;

            if (m != null)
                m.SendGump(new ToSGump());
        }

        public static void SetAccepted(Mobile m)
        {
            Account acct = m.Account as Account;
            if (acct != null)
                acct.SetTag("ToS_accepted", "true");
        }

        public static void NewVersion()
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m == null || !(m is PlayerMobile))
                    continue;

                PlayerMobile pm = (PlayerMobile)m;
                Account acct = (Account)m.Account;
                if (acct == null)
                    continue;

                acct.SetTag("ToS_accepted", "false");
            }
        }

        #region Serialising
        private static void Serialize(WorldSaveEventArgs e)
        {
            if (!Directory.Exists(m_Directory))
                Directory.CreateDirectory(m_Directory);

            GenericWriter writer = new BinaryFileWriter(m_FilePath, true);

            writer.Write(Version);//version
            writer.Close();
        }

        private static void Deserialize()
        {
            if (File.Exists(m_FilePath))
            {
                using (FileStream fs = new FileStream(m_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        BinaryReader br = new BinaryReader(fs);
                        BinaryFileReader reader = new BinaryFileReader(br);

                        int version = reader.ReadInt();

                        if (version != Version)
                            NewVersion();
                    }

                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
        }
        #endregion
    }

    public class ToSGump : Gump
    {
        public ToSGump()
            : base(60, 30)
        {
            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = false;
            AddPage(0);

Closable=true;
Disposable=true;
Dragable=true;
Resizable=false;
AddPage(0);
AddBackground(0, 0, 550, 469, 83);
AddBackground(103, 13, 352, 74, 5150);
AddHtml( 0, 40, 550, 18,"<basefont color=#ffffff><center>Defiance Terms of Service and Rules</center>", false, false);
AddBackground(40, 95, 470, 315, 9380);
AddHtml( 72, 136, 407, 233, ToSRulesChecker.Rules, false, true);
AddLabel(70, 425, 898, "I have read and accepted the ToS/Rules");
AddCheck(40, 425, 210, 211, false, 0);
AddButton(423, 422, 247, 248, 1, GumpButtonType.Reply, 0);

        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            bool id = info.ButtonID == 1;
            bool hasaccepted = info.IsSwitched(0);

            if (id && hasaccepted)
                ToSRulesChecker.SetAccepted(sender.Mobile);

            else
                sender.Mobile.SendGump(this);
        }
    }
}