using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a black lich corpse" )]
	public class BlackLich : BaseCreature
	{
		Timer m_Timer;
		[Constructable]
		public BlackLich() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a black lich";
			Body = 24;
			BaseSoundID = 0x3E9;

			Hue = 1175;

			SetStr( 171, 200 );
			SetDex( 126, 145 );
			SetInt( 276, 305 );

			SetHits( 8000 );

			SetDamage( 24, 26 );

			SetDamageType( ResistanceType.Physical, 10 );
			/*SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );*/

			SetSkill( SkillName.EvalInt, 130.0 );
			SetSkill( SkillName.Magery, 130.0 );
			SetSkill( SkillName.Meditation, 130.0 );
			SetSkill( SkillName.MagicResist, 130.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;

			m_Timer = new FlameTimer( this );

			PackItem( new GnarledStaff() );

			switch ( Utility.Random( 50 ) )
			{
				case 0: PackItem( new BlackLichItem() ); break;
			}
			//PackNecroReg( 15, 25 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public BlackLich( Serial serial ) : base( serial )
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
			m_Timer = new FlameTimer( this );
		}

		private class FlameTimer : Timer
		{
			private BlackLich m_Lich;

			public FlameTimer( BlackLich lich ) : base ( TimeSpan.FromSeconds( 0.0 ), TimeSpan.FromSeconds( 10.0 ) )
			{
				m_Lich = lich;
				Start();
			}

			protected override void OnTick()
			{
				if ( m_Lich == null || m_Lich.Deleted || !m_Lich.Alive )
					Stop();
				m_Lich.FixedParticles( 14089, 20, 30, 14089, 91, 3, (EffectLayer)255 );
				m_Lich.PlaySound( 0x208 );
			}
		}

		private class BlackLichItem : Item
		{
			[Constructable]
            public BlackLichItem() : base( 0xF8D )
			{
				Hue = 1175;
				Name = "decayed hair";
				Stackable = false;
				Weight = 15;
			}

			public BlackLichItem( Serial serial ) : base( serial )
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
}