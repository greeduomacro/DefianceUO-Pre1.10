using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	public class Tallon : BaseBellBoss
	{
		static bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public Tallon() : base( AIType.AI_Mage )
		{
			Name = "Tallon Wispcatcher";
			Body = 776;
			BaseSoundID = 357;
			Kills = 5;
			m_Active = true;

			SetStr( 850 );
			SetDex( 250 );
			SetInt( 500 );

			SetHits( 6000 );

			SetDamage( 50, 70 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 200 );
			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Tactics, 120 );
			SetSkill( SkillName.Wrestling, 120 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 140;
		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 2 ) < 1 )
			c.DropItem( new MysticKeySinglePart(1) );
			base.OnDeath( c );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new MetalChips() );

			base.OnDeath( c );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill ) // Taken from Questboss.cs and m_ability removed
		{
			if ( from != null && 0.05 >= Utility.RandomDouble() )
			{
				BaseCreature spawn = new DecayingSpawn ( this );

				spawn.BodyValue = this.BodyValue;
				spawn.Hue = this.Hue;
				spawn.BaseSoundID = this.BaseSoundID;
				spawn.Team = this.Team;
				spawn.MoveToWorld( from.Location, from.Map );
				spawn.Combatant = from;
			}
			base.OnDamage(amount, from, willKill);
		}

		public Tallon( Serial serial ) : base( serial )
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
			//Explanation see GhostPast
			m_Active = true;
		}
	}
}