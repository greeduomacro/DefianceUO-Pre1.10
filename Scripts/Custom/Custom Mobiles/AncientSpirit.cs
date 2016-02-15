using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ancient spirit" )]
	public class AncientSpirit : BaseCreature
	{
		[Constructable]
		public AncientSpirit () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ancient spirit";
			Body = 58;
			Hue = 1259;
			BaseSoundID = 466;

			SetStr(305, 425);
			SetDex(72, 150);
			SetInt(4500, 5500);
			SetHits(950, 1050);
			SetStam(102, 300);

			SetDamage(15, 15);


			SetDamageType( ResistanceType.Physical, 30 );
			SetDamageType( ResistanceType.Energy, 70 );

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 48, 54 );
			SetResistance( ResistanceType.Cold, 48, 54 );
			SetResistance( ResistanceType.Poison, 48, 54 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.EvalInt, 240.0, 260.0 );
			SetSkill( SkillName.Magery, 190.0, 210.0 );
			SetSkill( SkillName.MagicResist, 240.0, 260.0 );
			SetSkill( SkillName.Wrestling, 140.0, 160.0 );


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
				PackItem(new OrcishRemains());
			}
		}


		public AncientSpirit(Serial serial) : base( serial )
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
	public class OrcishRemains : BaseContainer
	{
		public override int DefaultGumpID{ get{ return 0x9; } }
		public override int DefaultDropSound{ get{ return 0x42; } }


		[Constructable]
		public OrcishRemains() : base( 3791 )
		{
			Hue = 2212;
			Name = "orcish remains";
			Weight = 11.0;
		}

		public OrcishRemains(Serial serial) : base( serial )
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
		}
	}
}