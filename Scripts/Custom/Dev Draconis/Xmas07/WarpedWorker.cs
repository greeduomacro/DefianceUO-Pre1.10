using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class WarpedWorker : BaseBoss
	{
		[Constructable]
		public WarpedWorker() : base( AIType.AI_Melee )
		{
			Name = "a warped elvish worker";
			Body = 255;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 500 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Wrestling, 80.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 10;
		}

		public override int GetDeathSound()
		{
			return 0x423;
		}

		public override int GetAttackSound()
		{
			return 0x23B;
		}

		public override int GetHurtSound()
		{
			return 0x140;
		}

		private BaseTool m_Tool;

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 6 ) < 1 )
				c.DropItem( new SnowDrop( Utility.RandomMinMax( 5, 8 ) ) );

			if ( Utility.Random( 5 ) < 1 )
			{
				int randomise = Utility.Random( 5 );

				switch ( randomise )
				{
					case 0: m_Tool = new SmithHammer(); break;
					case 1: m_Tool = new TinkerTools(); break;
					case 2: m_Tool = new Saw(); break;
					case 3: m_Tool = new Hammer(); break;
					case 4: m_Tool = new SewingKit(); break;
				}

				m_Tool.UsesRemaining = 100;
				c.DropItem( m_Tool );
			}
			base.OnDeath( c );
		}

		public override int CanBandageSelf{ get{ return 50; } }
		public override bool DoSkillLoss{ get{ return true; } }
		public override bool DoDetectHidden{ get{ return true; } }

		public WarpedWorker( Serial serial ) : base( serial )
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