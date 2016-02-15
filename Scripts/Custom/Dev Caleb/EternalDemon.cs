using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an eternal demon corpse" )]
	public class EternalDemon : BaseCreature
	{
		[Constructable]
		public EternalDemon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Name = "an eternal demon";
			Body = 10;
			Hue = 1150;

			SetStr(900, 1100);
			SetDex(200, 250);
			SetInt(900, 1100);
			SetHits(950, 1050);
			SetStam(102, 300);

			SetDamage(40, 40);


			SetDamageType( ResistanceType.Physical, 30 );
			SetDamageType( ResistanceType.Energy, 70 );

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 48, 54 );
			SetResistance( ResistanceType.Cold, 48, 54 );
			SetResistance( ResistanceType.Poison, 48, 54 );
			SetResistance( ResistanceType.Energy, 70 );


			SetSkill( SkillName.EvalInt, 80.0, 80.0 );
			SetSkill( SkillName.Magery, 80.0, 80.0 );
			SetSkill( SkillName.MagicResist, 0.0, 0.0 );
			SetSkill( SkillName.Wrestling, 280.0, 320.0 );


			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;

			int i = Utility.Random(0, 8);
			switch (i)
			{
				case 0:
					PackItem(new BlackPearl(20));
					break;
				case 1:
					PackItem(new Bloodmoss(20));
					break;
				case 2:
					PackItem(new Garlic(20));
					break;
				case 3:
					PackItem(new Ginseng(20));
					break;
				case 4:
					PackItem(new MandrakeRoot(20));
					break;
				case 5:
					PackItem(new Nightshade(20));
					break;
				case 6:
					PackItem(new SulfurousAsh(20));
					break;
				case 7:
					PackItem(new SpidersSilk(20));
					break;
			}

			if (Utility.RandomDouble() > 0.99)
			{
                PackItem(new CTFTicket());
			}
		}


        public EternalDemon(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class CTFTicket : Item
	{

		[Constructable]
        public CTFTicket() : base(0x14F0)
		{
			Hue = 98;
            Name = "CTF Ticket";
			Weight = 11.0;
            //LootType = LootType.Blessed;

            Timer.DelayCall(TimeSpan.FromHours(1.0), new TimerStateCallback(Decay_Callback), this);
		}

        private void Decay_Callback(object m)
        {
            if (m == null ||!(m is CTFTicket))
                return;

            CTFTicket t = m as CTFTicket;
            if (DateTime.Now > DateTime.Parse("6/1/2005 00:00:00"))
            {
                //t.Delete();
            }
            else
            {
                Timer.DelayCall(TimeSpan.FromHours(1.0), new TimerStateCallback(Decay_Callback), this);
            }

        }

        public CTFTicket(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
            Timer.DelayCall(TimeSpan.FromHours(1.0), new TimerStateCallback(Decay_Callback), this);
		}
	}
}