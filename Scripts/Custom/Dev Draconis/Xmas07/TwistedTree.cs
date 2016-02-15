using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class TwistedTree : BaseBoss
	{
		[Constructable]
		public TwistedTree() : base( AIType.AI_Melee )
		{
			Name = "a twisted christmas tree";
			Hue = 1367;
			Body = 47;
			BaseSoundID = 442;

			SetStr( 400 );
			SetDex( 125 );
			SetInt( 100 );

			SetHits( 5000 );

			SetDamage( 30, 40 );

			SetSkill( SkillName.MagicResist, 2000.0 );
			SetSkill( SkillName.Wrestling, 200.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 50;
		}

		public override void OnDeath( Container c )
		{
			c.DropItem( new SnowDrop( Utility.RandomMinMax( 20, 50 ) ) );

			base.OnDeath( c );
		}

		public override int DoWeaponsDoMoreDamage{ get{ return 2; } }
		public override bool DisallowAllMoves{ get{ return true; } }

		private DateTime m_NextPull;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextPull )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && !combatant.InRange( this, 4 ) )
				{
					m_NextPull = DateTime.Now + TimeSpan.FromSeconds( 20.0 );

					combatant.MoveToWorld( this.Location, this.Map );
					combatant.Poison = Poison.Lethal;
					combatant.Freeze( TimeSpan.FromSeconds( 5.0 ) );
					combatant.SendMessage( "A hidden vine pulls you to the tree!" );
				}
			}
		}

		public TwistedTree( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}