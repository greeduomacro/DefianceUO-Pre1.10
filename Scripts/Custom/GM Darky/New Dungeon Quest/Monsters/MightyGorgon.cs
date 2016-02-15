using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests.Collector;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a gorgon corpse" )]
	public class MightyGorgon : BaseCreature
	{
		[Constructable]
		public MightyGorgon() : base( AIType.AI_Berserk, FightMode.Weakest, 10, 1, 0.2, 0.2 )
		{
			Name = "a mighty gorgon";
			Body = Utility.RandomList( 0xE8, 0xE9 );
			BaseSoundID = 0x64;
			Hue = Utility.RandomList( 1149, 1175 );
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 477, 511 );
			SetDex( 156, 175 );
			SetInt( 47, 75 );

			SetHits( 280, 364 );

			SetDamage( 10, 15 );

			SetSkill( SkillName.MagicResist, 170.6, 250.0 );
			SetSkill( SkillName.Tactics, 97.6, 99.8 );
			SetSkill( SkillName.Anatomy, 87.6, 95.0 );
			SetSkill( SkillName.Wrestling, 98.1, 99.5 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 125;

			PackGold( 400, 750 );
			PackGem();
			PackGem();

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new Obsidian() );

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new EnchantedWood() );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new GorgonRug() );

            		base.OnDeath( c );
		}

		public override int Meat{ get{ return 15; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Black; } }
                public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public override void OnGaveMeleeAttack( Mobile defender )
			{
			base.OnGaveMeleeAttack( defender );

			if ( 0.2 >= Utility.RandomDouble() )
				Earthquake();
			}

		public void Earthquake()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			ArrayList targets = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 25 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					targets.Add( m );
				else if ( m.Player )
					targets.Add( m );
			}

			PlaySound( 0x2F3 );

			for ( int i = 0; i < targets.Count; ++i )
			{
				Mobile m = (Mobile)targets[i];

				double damage = m.Hits * 0.6;

				if ( damage < 10.0 )
					damage = 10.0;
				else if ( damage > 75.0 )
					damage = 75.0;

				DoHarmful( m );

				AOS.Damage( m, this, (int)damage, 100, 0, 0, 0, 0 );

				if ( m.Alive && m.Body.IsHuman && !m.Mounted )
					m.Animate( 20, 7, 1, true, false, 0 ); // take hit
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
			{
			scalar = 0.0; // Immune to magic
			}

		public MightyGorgon(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}