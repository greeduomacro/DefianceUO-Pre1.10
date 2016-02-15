using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Spells;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	[CorpseName( "a windcaller corpse" )]
	public class FakeWindcaller : BaseFakeMob
	{
		[Constructable]
		public FakeWindcaller() : base( AIType.AI_Mage )
		{
			Name = NameList.RandomName( "male" );
			Title = "the WindCaller";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 300 );
			SetDex( 300 );
			SetInt( 600 );

			SetHits( 800 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 30 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 100;
			Karma = 100;

			VirtualArmor = 30;

			Sandals foot = new Sandals();
			foot.Hue = 1154;
			foot.LootType = LootType.Blessed;
			AddItem( foot );

			WizardsHat top = new WizardsHat();
			top.Hue = 1154;
			top.LootType = LootType.Blessed;
			AddItem( top );

			Skirt bottom = new Skirt();
			bottom.Hue = 1154;
			bottom.LootType = LootType.Blessed;
			AddItem ( bottom );

		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			{
				DoHarmful( defender );
				defender.SendMessage( "Some of your life force has been stolen!" );
				int toDrain = Utility.RandomMinMax( 20, 25 );
				Hits += toDrain;
				defender.Damage( toDrain, this );
			}
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public FakeWindcaller( Serial serial ) : base( serial )
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
}