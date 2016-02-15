using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
	[CorpseName( "a daemon corpse" )]
	public class DoomBaron : BaseCreature
	{
		[Constructable]
		public DoomBaron () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 3.0, 5.0 )
		{
			Name = "Baron of Doom";
			Hue = 1160;
			Body = 9;
			BaseSoundID = 357;

			SetStr( 500, 550 );
			SetDex( 120, 150 );
			SetInt( 400, 450 );

			SetHits( 1000 );
			SetMana( 1000 );

			SetDamage( 1, 2 );

			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.MagicResist, 160.0 );
			SetSkill( SkillName.Tactics, 0.0 );
			SetSkill( SkillName.Wrestling, 200.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 200 ) < 1 )
			c.DropItem( new DoomBaronStatue() );

			if ( Utility.Random( 150 ) < 1 )
			c.DropItem( new GoldenPrizeToken() );


			base.OnDeath( c );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool BardImmune{ get{ return true;} }

		public override void Damage( int amount, Mobile from )
		{
			if ( from != null )
			{
				if ( amount > 50 )
				{
					this.Hits += amount;
					amount = (int)(0);
				}
			}

			base.Damage( amount, from );
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			this.Hits += defender.Hits;
			this.Stam += defender.Stam;
			this.Mana += defender.Stam;
			defender.Hits = 0;
			defender.Stam = 0;
			defender.Mana = 0;
			defender.SendMessage( "Your energies are drained by the Baron!" );
			Thrown( defender );
		}

		private int xaxis;
		private int yaxis;

		private void Thrown( Mobile m )
		{
			Map map = this.Map;

			if ( map != null )
			{

				for ( int i = 0; i < 10; ++i )
				{
					if ( this.Direction == Direction.Up || this.Direction == Direction.North || this.Direction == Direction.Right )
					{
						yaxis = this.Y - 6;
					}
					else if ( this.Direction == Direction.Down || this.Direction == Direction.South || this.Direction == Direction.Left )
					{
						yaxis = this.Y + 6;
					}
					else
					{
						yaxis = this.Y;
					}

					if ( this.Direction == Direction.Right || this.Direction == Direction.East || this.Direction == Direction.Down )
					{
						xaxis = this.X + 6;
					}
					else if ( this.Direction == Direction.Left || this.Direction == Direction.West || this.Direction == Direction.Up )
					{
						xaxis = this.X - 6;
					}
					else
					{
						xaxis = this.X;
					}

					int z = this.Z;

					if ( !map.CanFit(xaxis, yaxis, z, 16, false, false ) )
						continue;

					Point3D from = this.Location;
					Point3D to = new Point3D( xaxis, yaxis, z );

					if ( !InLOS( to ) )
						continue;

					m.Location = to;
					m.ProcessDelta();
				}
			}
		}

		public DoomBaron( Serial serial ) : base( serial )
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