using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a royal corpse" )]
	public class KrofinKing : BaseCreature
	{
		[Constructable]
		public KrofinKing() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "King of Krofins";
			Body = 255;
			Hue = 249;

			SetStr( 400, 600 );
			SetDex( 100 );
			SetInt( 200, 300 );

			SetHits( 500, 650 );

			SetDamage( 25, 35 );

			SetDamageType( ResistanceType.Physical, 120 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 40 );


			SetSkill( SkillName.MagicResist, 300.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Tactics, 100 );
			SetSkill( SkillName.Wrestling, 80.0, 90.0 );

			Fame = 13500;
			Karma = -13500;

			VirtualArmor = 40;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 1 ) < 1 )
				c.DropItem( new KrofinBOMB() );

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new KrofinBOMB() );

			if ( Utility.Random( 10 ) < 1 )
				c.DropItem( new KrofinBOMB() );

			if ( Utility.Random( 1 ) < 1 )
				c.DropItem( new OddLookingKey() );

			if (Utility.Random( 125 ) < 1 )
				c.DropItem( new KrofinRoyalFlag() );

            base.OnDeath( c );
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.UltraRich, 1 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool BardImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool DisallowAllMoves{ get{ return true; } }



		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster is PlayerMobile )
			{
				DoHarmful( caster );

				caster.MovingParticles( this, 0x36F4, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0x100 );
				caster.PlaySound( 0x108 );

				int toDrain = (int)(caster.Hits);

				Hits += toDrain;
				caster.Damage( toDrain, this );

				caster.SendMessage( "The life is sucked out of you!" );
			}
			base.OnDamagedBySpell( caster );
		}


		public override void OnThink()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 9 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 248, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				int toDrain = 30;

				Hits += toDrain;
				m.Damage( toDrain, this );

			}

			base.OnThink();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.05 >= Utility.RandomDouble() )
				//new PoolOfAcid( TimeSpan.FromSeconds( 10 ), 10, 20 ).MoveToWorld( Location, Map );
				SpillAcid( TimeSpan.FromSeconds( 10 ), 20, 30, attacker );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, Mobile target)
		{
			if ( Map != null && target != null )
			{
				if ( target.Map != null )
				{
					PoolOfSap sap = new PoolOfSap( duration, minDamage, maxDamage );

					sap.MoveToWorld( target.Location, target.Map );
					this.PlaySound( 37 );
				}
			}
		}

		public KrofinKing( Serial serial ) : base( serial )
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