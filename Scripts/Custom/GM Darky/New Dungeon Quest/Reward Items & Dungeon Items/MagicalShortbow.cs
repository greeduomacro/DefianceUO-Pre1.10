////////////////////
//Created by Darky//
////////////////////

using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	[FlipableAttribute( 0x2D1F, 0x2D2B )]
	public class MagicalShortbow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		public override int OldStrengthReq{ get{ return 50; } }
		public override int OldDexterityReq{ get{ return 75; } }
		public override int OldMinDamage{ get{ return 30; } }
		public override int OldMaxDamage{ get{ return 45; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefMaxRange{ get{ return 12; } }

		public override int InitMinHits{ get{ return 125; } }
		public override int InitMaxHits{ get{ return 150; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public MagicalShortbow() : base( 0x2D1F )
		{
			Weight = 10.0;
			Layer = Layer.TwoHanded;
                        LootType = LootType.Newbied;
			Name = " [Protector of the Forest]  10% summon creature chance";
		}

		public MagicalShortbow( Serial serial ) : base( serial )
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

			if ( Weight == 7.0 )
				Weight = 10.0;
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
				((Mobile)parent).Dex += 5;
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				((Mobile)parent).Dex -= 5;
		}

		private static Type[] m_Types = new Type[]
			{
				typeof( GrizzlyBear ),
				typeof( BlackBear ),
				typeof( BrownBear ),
				typeof( Cougar ),
				typeof( DireWolf ),
				typeof( Panther ),
				typeof( TimberWolf ),
			};

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			if ( defender is PlayerMobile )
			{
				AOS.Damage( attacker, 50, 0, 100, 0, 0, 0 );
				Delete();
				attacker.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				attacker.PlaySound( 0x307 );
			}

			if ( attacker.Alive )
			{
				if ( 0.1 > Utility.RandomDouble() )
				{
						BaseCreature creature = (BaseCreature)Activator.CreateInstance( m_Types[Utility.Random( m_Types.Length )] );
						creature.ControlSlots = 1;
						TimeSpan duration = TimeSpan.FromMinutes( 2.0 );
						SpellHelper.Summon( creature, attacker, 0x215, duration, false, false );
						creature.Combatant = defender;
						attacker.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
						attacker.BoltEffect( 0 );
						attacker.PlaySound( 1481 );
						attacker.SendMessage("A creature has been summoned to your aid!");
				}
				base.OnHit( attacker, defender );
			}
		}
	}
}