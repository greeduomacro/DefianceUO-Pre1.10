using System;
using Server;
using Server.Items;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class BronzeDragon : BaseRareBoss
	{
		[Constructable]
		public BronzeDragon () : base( AIType.AI_Mage )
		{
			Name = "a bronze dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;
			Hue = 2401;

			SetStr( 800, 900 );
			SetDex( 160, 200 );
			SetInt( 800, 900 );

			SetHits( 1200, 1400 );

			VirtualArmor = 100;
		}

		public override bool HasBreath{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ( Body == 12 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override int DoMoreDamageToPets { get { return 3; } }
		public override int DoLessDamageFromPets { get { return 3; } }
		public override bool DoProvoPets { get { return true; } }

		public BronzeDragon( Serial serial ) : base( serial )
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