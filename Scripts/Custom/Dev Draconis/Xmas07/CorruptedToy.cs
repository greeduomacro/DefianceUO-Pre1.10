using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class CorruptedToy : BaseBoss
	{
		[Constructable]
		public CorruptedToy() : base( AIType.AI_Melee )
		{
			Name = "a corrupted toy";
			Body = 752;

			SetStr( 300 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( 1000 );

			SetDamage( 20, 30 );

			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 100;
		}

		public override int GetAngerSound()
		{
			return 541;
		}

		public override int GetIdleSound()
		{
			return 542;
		}

		public override int GetDeathSound()
		{
			return 545;;
		}

		public override int GetAttackSound()
		{
			return 562;
		}

		public override int GetHurtSound()
		{
			return 320;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 3 ) < 1 )
				c.DropItem( new SnowDrop( Utility.RandomMinMax( 10, 15 ) ) );

			base.OnDeath( c );
		}

		public override bool DoImmuneToPets{ get{ return true; } }
		public override int CanCheckReflect{ get{ return 1; } }
		public override int DoStunPunch{ get{ return 4; } }

		public CorruptedToy( Serial serial ) : base( serial )
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