using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public class Vampire : BaseCreature
	{
		[Constructable]
		public Vampire() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			switch ( Utility.Random( 16 ))
			{
				case 0: Name = "Asasabonsam"; break;
				case 1: Name = "Bhuta"; break;
				case 2: Name = "Chedipe"; break;
				case 3: Name = "Brahmaparusha"; break;
				case 4: Name = "Churel"; break;
				case 5: Name = "Maneden"; break;
				case 6: Name = "Pisachas"; break;
				case 7: Name = "Polong"; break;
				case 8: Name = "Asema"; break;
				case 9: Name = "Civateto"; break;
				case 10: Name = "Tlahuelpuchi"; break;
				case 11: Name = "Bruxa"; break;
				case 12: Name = "Strigoiu"; break;
				case 13: Name = "Dhampir"; break;
				case 14: Name = "Djadadjii"; break;
				case 15: Name = "Ustrel"; break;
			}
			Title = "the vampire";

			Female = Utility.RandomBool();
			Body = 400 + (Female ? 1 : 0);
			Hue = 0x0;

			SetStr( 80, 100 );
			SetDex( 72, 150 );
			SetInt( 150, 250 );

			SetHits( 700, 1000 );
			Stam = Dex;

			SetDamage( 10, 25 );

			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 30;

			FancyShirt shirt = new FancyShirt();
			shirt.Hue = 1175;
			shirt.Movable = false;
			AddItem( shirt );

			LongPants pants = new LongPants();
			pants.Hue = 1175;
			pants.Movable = false;
			AddItem( pants );

			Boots boots = new Boots();
			boots.Hue = 1175;
			boots.Movable = false;
			AddItem( boots );

			Cloak cloak = new Cloak();
			cloak.Hue = 1175;
			cloak.Movable = false;
			AddItem( cloak );

			Hair hair = new LongHair();
			hair.Hue = 1175;
			hair.Movable = false;
			AddItem( hair );

		}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 50 ) <  1 )
				c.DropItem( new RareBloodCarpet( PieceType.NorthEdge ) );

			base.OnDeath( c );
	  	}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Meager );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			CheckShape();
			if( Utility.RandomDouble() < 0.3 )
			{
				damage = DrainLife( to );
			}
			else
				base.AlterMeleeDamageTo( to, ref damage );
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			CheckShape();
			base.AlterMeleeDamageFrom( from, ref damage );
		}
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			CheckShape();
			base.OnMovement(m, oldLocation);
		}

		protected override bool OnMove( Direction d )
		{
			CheckShape();
			return base.OnMove( d );
		}

		public int DrainLife( Mobile defender )
		{
			int Life = defender.Hits * 4 / 10;
			if( Life > 30 )
				Life = 30;
			Hits += Life;
			if( defender is PlayerMobile )
				defender.SendMessage( "You feel life getting drained out of you." );
			return Life;
		}

		public void CheckShape()
		{
			if( Combatant == null )
				TurnIntoHuman();
			else
			{
				if( (int)GetDistanceToSqrt( Combatant ) <= 1 )
					TurnIntoHuman();
				else
					TurnIntoBat();
			}
		}

		public void TurnIntoHuman()
		{
			if( BodyValue != 400 + (Female ? 1 : 0) )
			{
				BodyValue = 400 + (Female ? 1 : 0);
				this.FixedParticles( 0x3728, 10, 10, 2023, 5, 3, EffectLayer.Waist );
			}
		}

		public void TurnIntoBat()
		{
			if( BodyValue != 317 )
			{
				BodyValue = 317;
				this.FixedParticles( 0x3728, 10, 10, 2023, 5, 3, EffectLayer.Waist );
			}
		}

		public Vampire( Serial serial ) : base( serial )
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