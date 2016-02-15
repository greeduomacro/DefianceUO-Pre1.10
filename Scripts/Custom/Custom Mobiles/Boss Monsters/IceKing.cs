using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ice daemon corpse" )]
	public class IceKing : BaseCreature
	{
		[Constructable]
		public IceKing () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Damien The Ice King";
			Body = 43;
			BaseSoundID = 357;

			Hue = 1150;

			SetStr( 376, 405 );
			SetDex( 176, 195 );
			SetInt( 201, 225 );

			SetHits( 1226, 1300 );

			SetDamage( 58, 59 );

			SetSkill( SkillName.EvalInt, 201.1, 225.0 );
			SetSkill( SkillName.Magery, 280.1, 290.0 );
			SetSkill( SkillName.MagicResist, 90.1, 95.0 );
			SetSkill( SkillName.Tactics, 280.1, 290.0 );
			SetSkill( SkillName.Wrestling, 297.3, 300.0 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			Fame = 180000;
			Karma = -180000;

			VirtualArmor = 64;

			PackGold( 23000, 24000 );
			PackMagicItems( 8, 9 );

                 }




		public override int Meat{ get{ return 1; } }

		public IceKing( Serial serial ) : base( serial )
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