using System;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class EvilPaladin : BaseRareBoss
	{
		[Constructable]
		public EvilPaladin(): base( AIType.AI_Melee )
		{
			Body = 0x190;
			Name = "Paladin for Evil";
			Hue = 0;

			SetSkill( SkillName.Swords, 120.1, 130.0 );

			VikingSword weapon = new VikingSword();
			weapon.Hue = 1108;
			weapon.Movable = false;
			AddItem( weapon );

			MetalShield shield = new MetalShield();
			shield.Hue = 1108;
			shield.Movable = false;
			AddItem( shield );

			BoneHelm helm = new BoneHelm();
			helm.Hue = 1108;
			helm.Movable = false;
			AddItem( helm );

			BoneArms arms = new BoneArms();
			arms.Hue = 1108;
			arms.Movable = false;
			AddItem( arms );

			BoneGloves gloves = new BoneGloves();
			gloves.Hue = 1108;
			gloves.Movable = false;
			AddItem( gloves );

			BoneChest tunic = new BoneChest();
			tunic.Hue = 1108;
			tunic.Movable = false;
			AddItem( tunic );

			BoneLegs legs = new BoneLegs();
			legs.Hue = 1108;
			legs.Movable = false;
			AddItem( legs );

			Shoes shoes = new Shoes();
			shoes.Hue = 1108;
			shoes.Movable = false;
			AddItem( shoes );
		}

		public override void OnDeath( Container c )
		{
            	//if (0.10 >= Utility.RandomDouble())
           		//c.DropItem(new CursedClothingBlessDeed()); break;
			base.OnDeath( c );
		}

		public override int GetIdleSound()
		{
			return 0x184;
		}

		public override int GetAngerSound()
		{
			return 0x286;
		}

		public override int GetDeathSound()
		{
			return 0x288;
		}

		public override int GetHurtSound()
		{
			return 0x19F;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool DoDisarmPlayer{ get { return true; } }
		public override bool DoSpawnEvil{ get { return true; } }

		public EvilPaladin( Serial serial ) : base( serial )
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