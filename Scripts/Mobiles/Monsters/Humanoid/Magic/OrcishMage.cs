using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a glowing orc corpse" )]
	public class OrcishMage : BaseCreature
	{
		[Constructable]
		public OrcishMage () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an orcish mage";
			Body = 140;
			BaseSoundID = 0x45A;

			SetStr( 116, 150 );
			SetDex( 91, 115 );
			SetInt( 161, 185 );

			SetHits( 70, 90 );

			SetDamage( 4, 14 );

			SetSkill( SkillName.EvalInt, 60.1, 92.5 );
			SetSkill( SkillName.Magery, 60.1, 95.5 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.Wrestling, 40.1, 50.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 30;

			PackReg( 5 );
			PackGold( 50, 150 );
			PackScroll( 1, 4 );
			PackPotion();
			PackItem( new Arrow( Utility.Random( 1, 10 ) ) );
			PackGem();

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new OrcishKinMask() );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy( Mobile m )
		{
			Item helm = m.FindItemOnLayer( Layer.Helm );
			if ( m.Player && helm is OrcishKinMask || helm is OrcishKinRPMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
		}

		public OrcishMage( Serial serial ) : base( serial )
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