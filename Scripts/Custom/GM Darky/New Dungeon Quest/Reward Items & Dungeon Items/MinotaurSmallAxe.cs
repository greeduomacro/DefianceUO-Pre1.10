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
	[FlipableAttribute( 0x2D28, 0x2D34 )]
	public class MinotaurSmallAxe : BaseAxe
	{
		public override int OldStrengthReq{ get{ return 90; } }
		public override int OldMinDamage{ get{ return 50; } }
		public override int OldMaxDamage{ get{ return 65; } }
		public override int OldSpeed{ get{ return 25; } }

		public override int InitMinHits{ get{ return 150; } }
		public override int InitMaxHits{ get{ return 150; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

		[Constructable]
		public MinotaurSmallAxe() : base( 0x2D34 )
		{
			Weight = 24.0;
			Name = "[Minotaur Small Axe]   10% chance to strengthen wielder";
			LootType = LootType.Newbied;
			Layer = Layer.OneHanded;
		}

		public MinotaurSmallAxe( Serial serial ) : base( serial )
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

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			if ( 0.1 > Utility.RandomDouble() )
			{
				SpellHelper.AddStatBonus( attacker, attacker, StatType.Str, 25, TimeSpan.FromMinutes( 2.0 ) );

				attacker.FixedParticles( 0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist );
				attacker.FixedParticles( 0x3728, 1, 13, 9502, 33, 7, (EffectLayer)255 );
				attacker.PlaySound( 1435 );
				attacker.SendMessage("The strength of the minotaur runs through you!");
			}

			base.OnHit( attacker, defender );

			if ( attacker.Alive && defender is PlayerMobile && defender.Alive )
			{
				AOS.Damage( attacker, 50, 0, 100, 0, 0, 0 );
				attacker.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				attacker.PlaySound( 0x307 );
				Delete();
			}
		}
	}
}