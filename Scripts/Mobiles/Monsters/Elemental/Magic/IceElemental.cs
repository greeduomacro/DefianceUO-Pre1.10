using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ice elemental corpse" )]
	public class IceElemental : BaseCreature
	{
		[Constructable]
		public IceElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ice elemental";
			Body = 161;
			BaseSoundID = 268;

			SetStr( 156, 185 );
			SetDex( 96, 115 );
			SetInt( 171, 192 );

			SetHits( 94, 111 );

			SetDamage( 10, 21 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Cold, 75 );

			SetSkill( SkillName.EvalInt, 10.5, 60.0 );
			SetSkill( SkillName.Magery, 10.5, 60.0 );
			SetSkill( SkillName.MagicResist, 30.1, 80.0 );
			SetSkill( SkillName.Tactics, 70.1, 100.0 );
			SetSkill( SkillName.Wrestling, 60.1, 100.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 40;

			PackGem();
			PackGem();
			PackGold( 50, 150 );
			PackItem( new BlackPearl() );
			PackReg( 2 );
			PackItem( new Arrow( Utility.Random( 1, 10 ) ) );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.WestEdge ) );

			base.OnDeath( c );
	  	}

		public IceElemental( Serial serial ) : base( serial )
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