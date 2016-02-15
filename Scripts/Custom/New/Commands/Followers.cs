using System;
using Server;
using System.Text;
using System.Collections;
using System.Net;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Targets;
using Server.Gumps;
using Server.Scripts.Commands;
using System.Text.RegularExpressions;

namespace Server.Scripts.Commands
{
    public class Followers
    {
        public static void Initialize()
        {
            Server.Commands.Register("Followers", AccessLevel.GameMaster, new CommandEventHandler(Followers_OnCommand));
        }

        [Usage("Followers")]
        [Description("Brings up a menu with a players pets or brings you to the owner of a pet.")]
        public static void Followers_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(Followers_OnTarget));
            e.Mobile.SendMessage("Target a player to get their pets information.");
        }

        public static void Followers_OnTarget(Mobile from, object obj)
        {
            if (from == null || from.Deleted || !(obj is Mobile) )
                return;

            if (((Mobile)obj).Player)
            {

                Mobile m_from = from;
                Mobile master = null;
                ArrayList pets = new ArrayList();

                master = (Mobile)obj;

                foreach (Mobile m in World.Mobiles.Values)
                {
                    if (m is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)m;

                        if ((bc.Controlled && bc.ControlMaster == master) || (bc.Summoned && bc.SummonMaster == master))
                            pets.Add(bc);
                    }
                }

                m_from.CloseGump(typeof(FollowersGump));
                m_from.SendGump(new FollowersGump(pets, master));
            }
            else if ((Mobile)obj is BaseCreature)
            {
                BaseCreature pet = (BaseCreature)obj;
                if (pet.Controlled && pet.ControlMaster != null)
                {
                    if (pet.ControlMaster == from)
                    {
                        from.SendMessage("Genius!");
                        return;
                    }
                    if (pet.ControlMaster.Map == Map.Internal)
                    {
                        from.SendMessage("The owner of this pet is currently not online.");
                        return;
                    }
                    from.MoveToWorld(pet.ControlMaster.Location , pet.ControlMaster.Map);
                    from.SendMessage("You have been moved to the creatures master: {0}.", pet.ControlMaster.Name);
                }
                else
                {
                    from.SendMessage("That creature is unowned.");
                }
            }
        }

        public static void StablePet(Mobile from, BaseCreature pet)
        {
            if (pet == null || pet.Deleted || !(pet is BaseCreature) || from == null || from.Deleted)
                return;

            if (pet.ControlMaster == null)
                from.SendMessage("This creature has no owner.");

            else if (pet.IsStabled)
                from.SendMessage("The pet is already stabled.");
			else
			{
				Mobile owner = pet.ControlMaster;

				if (owner.Stabled.Count >= Server.Mobiles.AnimalTrainer.GetMaxStabled(owner))
					from.SendMessage("Warning: The owner has not enough free stable slots. Forcing GM stable...");

				if (pet is IMount)
				{
					IMount bm = (IMount)pet;
					bm.Rider = null;
				}

				pet.ControlTarget = null;
				pet.ControlOrder = OrderType.Stay;
				pet.Internalize();

				pet.SetControlMaster(null);
				pet.SummonMaster = null;

				pet.IsStabled = true;
				owner.Stabled.Add(pet);

				from.SendMessage("The pet is now stabled.");
			}
        }

        public static void UnStablePet(Mobile from, BaseCreature pet, Mobile gm)
        {
            if (from == null || from.Deleted || pet == null || pet.Deleted || gm == null || gm.Deleted)
                return;

			if (from.Stabled.Contains(pet))
			{
				gm.SendMessage("Warning: This is a force claiming. Followers count will not be checked!");

				pet.SetControlMaster(from);
				if (pet.Summoned)
					pet.SummonMaster = from;

				pet.ControlTarget = from;
				pet.ControlOrder = OrderType.Follow;

				if (from.Map == Map.Internal)
				{
					gm.MoveToWorld(from.Location, from.Map);
				}
				else
				{
					pet.MoveToWorld(from.Location, from.Map);
				}

				pet.IsStabled = false;
				from.Stabled.Remove(pet);
				//break;
			}
        }
    }

    public class Stable
    {
        public static void Initialize()
        {
            Server.Commands.Register("Stable", AccessLevel.GameMaster, new CommandEventHandler(Stable_OnCommand));
        }

        [Usage("Stable")]
        [Description("Stables a pet for its owner.")]
        public static void Stable_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(Stable_OnTarget));
            e.Mobile.SendMessage("Target the pet you want to stable.");
        }

        public static void Stable_OnTarget(Mobile from, object obj)
        {
            if (from == null || from.Deleted || !(obj is BaseCreature) )
                return;

            BaseCreature pet = (BaseCreature)obj;
            Followers.StablePet(from, pet);
        }
    }
}

namespace Server.Gumps
{
    public class FollowersGump : Gump
    {
        private ArrayList m_Pets;
        private ArrayList m_StabledPets;
        private Mobile m_Master;

        public FollowersGump(ArrayList pets, Mobile master)
            : base(20, 30)
        {

            m_Master = master;
            m_Pets = pets;

            int x = 30;
            int y = 70;
            int buttonID = 0;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(0, 0, 365, 335, 2600);
            this.AddLabel(130, 20, 248, @"Pets of " + master.Name);

            this.AddLabel(30, 50, 0, @"Kind");
            this.AddLabel(105, 50, 0, @"Name");
            this.AddLabel(194, 50, 0, @"Get");
            this.AddLabel(225, 50, 0, @"Go to");
            this.AddLabel(265, 50, 0, @"Props");
            this.AddLabel(306, 50, 0, @"Stable");

            if (m_Pets.Count != 0)
            {
                foreach (Mobile pet in m_Pets)
                {
                    AddLabel(x, y, 902, "" + GetPetKind(pet));
                    AddLabel(x + 75, y, 902, "" + pet.Name);
                    AddButton(x + 165, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    AddButton(x + 205, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    AddButton(x + 246, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    AddButton(x + 285, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    y += 20;
                }
            }
            else
            {
                AddLabel(x, y, 902, "The person has no followers.");
            }

            y += 25;

            this.AddLabel(x, y, 248, @"In Stable");
            this.AddLabel(x, y + 20, 0, @"Kind");
            this.AddLabel(x + 75, y + 20, 0, @"Name");
            this.AddLabel(x + 164, y + 20, 0, @"Props");
            this.AddLabel(x + 205, y + 20, 0, @"Unstable");

            y += 40;

            if (m_Master.Stabled.Count > 0)
            {
                m_StabledPets = m_Master.Stabled;
                buttonID = 50;

                foreach (Mobile stabled_pet in m_StabledPets)
                {
                    AddLabel(x, y, 902, "" + GetPetKind(stabled_pet) );
                    AddLabel(x + 75, y, 902, "" + stabled_pet.Name);
                    AddButton(x + 165, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    AddButton(x + 205, y + 4, 5230, 5231, ++buttonID, GumpButtonType.Reply, 0);
                    y += 20;
                }
            }
            else
            {
                AddLabel(x, y, 902, "No animals in stable");
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (sender == null ||
                sender.Mobile == null || sender.Mobile.Deleted ||
                info == null || info.ButtonID == 0)
                return;

            if (info.ButtonID < 0 &&
                sender.Mobile.AccessLevel < AccessLevel.GameMaster)
                return;

            Mobile pet;
            int todo;
            int petID;

            if (info.ButtonID < 51)
            {
                todo = (info.ButtonID - 1) % 4;
                petID = (info.ButtonID - 1) / 4;

                if (petID < 0 || petID > m_Pets.Count - 1)
                {
                    sender.Mobile.SendMessage("The pets have changed after gump creation. Please try again.");
                    return;
                }
                pet = (Mobile)m_Pets[petID];

                if (pet == null || pet.Deleted)
                    return;

                switch (todo)
                {
                    case 0:
                        //get
                        if (pet is BaseMount && ((BaseMount)pet).Rider != null)
                        {
                            BaseMount bm = (BaseMount)pet;
                            bm.Rider = null;
                            sender.Mobile.SendMessage("That creature was ridden. The rider has been dismouned.");
                        }

                        pet.MoveToWorld(sender.Mobile.Location, sender.Mobile.Map);
                        break;

                    case 1:
                        //goto
                        if (pet is BaseMount && ((BaseMount)pet).Rider != null)
                        {
                            BaseMount bm = (BaseMount)pet;
                            if (bm.Rider != bm.ControlMaster)
                            {
                                sender.Mobile.SendMessage("The rider of the pet is not the owner of it. GM fun? Or something strange is going on here. Request canceled.");
                                return;
                            }
                            sender.Mobile.MoveToWorld(bm.Rider.Location, bm.Rider.Map);
                            sender.Mobile.SendMessage("The owner of this pet is riding it atm. You have been moved to the owner.");
                        }
                        else
                        {
                            sender.Mobile.MoveToWorld(pet.Location, pet.Map);
                        }
                        break;

                    case 2:
                        //props
                        sender.Mobile.SendGump(new PropertiesGump(sender.Mobile, pet));
                        break;

                    case 3:
                        //stable
                        BaseCreature bc = (BaseCreature)pet;
                        Followers.StablePet(sender.Mobile, bc);
                        break;
                }
            }
            else
            {
                int tmp = info.ButtonID - 50;
                petID = (tmp - 1) / 2;
                todo = (tmp - 1) % 2;

                if (petID < 0 || petID > m_StabledPets.Count - 1)
                {
                    sender.Mobile.SendMessage("The pets have changed after gump creation. Please try again.");
                    return;
                }
                pet = (Mobile)m_StabledPets[petID];

                if (pet == null || pet.Deleted)
                    return;

                switch (todo)
                {
                    case 0:
                        // props
                        sender.Mobile.SendGump(new PropertiesGump(sender.Mobile, pet));
                        break;

                    case 1:
                        // unstable
                        BaseCreature bc = (BaseCreature)pet;
                        Followers.UnStablePet(m_Master, bc, sender.Mobile);
                        break;
                }

            }
        }

        public String GetPetKind(Mobile pet)
        {
            if (pet == null || pet.Deleted)
                return "";

            String petclass = pet.GetType().ToString();
            Regex rx = new Regex(@"^.*\.(?<pc>.*)$");
            Match m = rx.Match(petclass);

            return m.Groups["pc"].Value;
        }
    }
}