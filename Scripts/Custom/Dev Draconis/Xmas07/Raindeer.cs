using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a reindeer corpse" )]

	public class Raindeer : BaseCreature
	{
		[Constructable]
		public Raindeer() : base( AIType.AI_Animal, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a reindeer";
			Body = 0xEA;
			Hue = 1717;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 50 );

			SetHits( 200 );
			SetMana( 0 );

			SetDamage( 8, 12 );

			SetSkill( SkillName.MagicResist, 140.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Necromancy, 120.0 );
			SetSkill( SkillName.SpiritSpeak, 160.0 );

			Fame = 8000;
			Karma = 6000;

			VirtualArmor = 40;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 120 ) < 1 )
				c.DropItem( new RaindeerStatue() );

			base.OnDeath( c );
		}

		public override int Meat{ get{ return 8; } }
		public override int Hides{ get{ return 10; } }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if ( from != null )
                {
                                         if ( willKill == true )
		         {
				from.SendMessage( "Killing this pure and innocent creature has turned you into a criminal!" );
				from.Criminal = true;

				if ( Utility.Random( 5 ) < 1 )
				{
					Revenant rev = new Revenant( this, from, TimeSpan.FromSeconds( 180 ) );

					int x = from.X + Utility.RandomMinMax( -2, 2 );
					int y = from.Y + Utility.RandomMinMax( -2, 2 );
					int z = from.Z;

					Point3D loc = new Point3D( x, y ,z );

					rev.DamageMin = 16;
					rev.DamageMax = 20;
					rev.MoveToWorld( loc, from.Map );
					from.SendMessage( "A revenant has been sent to revenge the raindeer's death!" );
                                                      }
				}
			}

			base.OnDamage(amount, from, willKill);
		}

		public Raindeer(Serial serial) : base(serial)
		{
		}

		public override int GetAttackSound()
		{
			return 0x82;
		}

		public override int GetHurtSound()
		{
			return 0x83;
		}

		public override int GetDeathSound()
		{
			return 0x84;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}