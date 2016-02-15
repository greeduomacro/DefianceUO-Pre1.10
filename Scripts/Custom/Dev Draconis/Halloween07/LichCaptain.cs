using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	[CorpseName( "a liche's corpse" )]
	public class LichCaptain : Lich
	{
		[Constructable]
		public LichCaptain() : base()
		{
			Name = "a lich captain";
			Hue = 1132;

			SetHits( 170, 190 );

			SetSkill( SkillName.EvalInt, 90.0 );
			SetSkill( SkillName.Magery, 90.0 );
			SetSkill( SkillName.Meditation, 90.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 90.0 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 200 ) < 1 )
			c.DropItem( new LichCaptainStatue() );

			if ( Utility.Random( 125 ) < 1 )
			c.DropItem( new SilverPrizeToken() );


			base.OnDeath( c );
		}

		public override bool BardImmune{ get{ return true;} }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if ( from != null && 0.33 >= Utility.RandomDouble() )
			{
				DrainMana();
			}
			base.OnDamage( amount, from, willKill );
		}

		public void DrainMana()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 4 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel weaker!" );

				int toDrain = Utility.RandomMinMax( 10, 20 );
				int toLeech = Utility.RandomMinMax( 5, 10 );
				m.Mana -= toDrain;
				Mana += toDrain;
				m.Hits -= toLeech;
				Hits += toLeech;
			}
		}

		public LichCaptain( Serial serial ) : base( serial )
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