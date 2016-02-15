////////////////////
//Created by Darky//
////////////////////

using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x2D1E, 0x2D2A )]
	public class MagicalCompositeBow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		public override int OldStrengthReq{ get{ return 75; } }
		public override int OldDexterityReq{ get{ return 90; } }
		public override int OldMinDamage{ get{ return 40; } }
		public override int OldMaxDamage{ get{ return 55; } }
		public override int OldSpeed{ get{ return 27; } }

		public override int DefMaxRange{ get{ return 16; } }

		public override int InitMinHits{ get{ return 125; } }
		public override int InitMaxHits{ get{ return 150; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public MagicalCompositeBow() : base( 0x2D1E )
		{
			Weight = 15.0;
			Layer = Layer.TwoHanded;
                        LootType = LootType.Newbied;
			Name = "  [Magical Composite Longbow]  10% chance to grace the wielder";
		}

		public MagicalCompositeBow( Serial serial ) : base( serial )
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
				Weight = 15.0;
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
				((Mobile)parent).VirtualArmorMod += 10;
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				((Mobile)parent).VirtualArmorMod -= 10;
		}

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
				if ( 0.15 > Utility.RandomDouble() )
				{
					SpellHelper.AddStatBonus( attacker, attacker, StatType.Dex, 25, TimeSpan.FromMinutes( 2.0 ) );

					attacker.PlaySound( 1324 );
					attacker.SendMessage("The agility of the wolf graces you!");
					attacker.FixedParticles( 0x3779, 1, 30, 9964, 3, 3, EffectLayer.Waist );

					IEntity from = new Entity( Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z ), attacker.Map );
					IEntity to = new Entity( Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 50 ), attacker.Map );
					Effects.SendMovingParticles( from, to, 0x13B1, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

				}
				base.OnHit( attacker, defender );
			}
		}
	}
}