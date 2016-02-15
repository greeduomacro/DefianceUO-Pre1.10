using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a toxic elemental corpse" )]
	public class ToxicElemental : BaseCreature
	{
		[Constructable]
		public ToxicElemental () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a toxic elemental";
			Body = 13;
			Hue = 0x4001;
			BaseSoundID = 263;

			SetStr( 126, 152 );
			SetDex( 166, 185 );
			SetInt( 102, 121 );

			SetSkill( SkillName.Wrestling, 66, 82 );
			SetSkill( SkillName.Tactics, 62, 78 );
			SetSkill( SkillName.MagicResist, 63, 74 );
			SetSkill( SkillName.Magery, 67, 78 );

			ControlSlots = 2;

			VirtualArmor = 40;
			SetFameLevel( 3 );
			SetKarmaLevel( 3 );

			Katana kat = new Katana();
			kat.Movable = false;
			kat.Crafter = this;
			kat.Quality = WeaponQuality.Exceptional;
			AddItem( kat );

			PackGold( 75, 200 );
			PackMagicItems( 0, 3 );
			PackMagicItems( 0, 4 );
		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicBlueCarpet( PieceType.SouthEdge ) );

			base.OnDeath( c );
	  	}

		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.6; } }

		public ToxicElemental( Serial serial ) : base( serial )
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