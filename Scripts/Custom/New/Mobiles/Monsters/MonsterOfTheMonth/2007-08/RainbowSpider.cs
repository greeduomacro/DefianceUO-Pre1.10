using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Mobiles;
using Xanthos.Evo;

namespace Server.Mobiles
{
	[CorpseName( "a rainbow spider corpse" )]
	public class RainbowSpider : BaseCreature
	{
		[Constructable]
		public RainbowSpider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rainbow spider";
			Body =  173;
			Hue = Utility.RandomMinMax( 2, 200 );
			BaseSoundID = 0x388;

			SetStr( 150, 200 );
			SetDex( 150, 200 );
			SetInt( 150, 200 );

			SetHits( 1200, 1500 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.Anatomy, 100, 110.0 );
			SetSkill( SkillName.Poisoning, 100, 110.0 );
			SetSkill( SkillName.MagicResist, 150.0, 180.0 );
			SetSkill( SkillName.Tactics, 100.0, 110.0 );
			SetSkill( SkillName.Wrestling, 100.0, 110.0 );

			Fame = 9500;
			Karma = -9500;

			VirtualArmor = 50;

			PackItem( new SpidersSilk( 40 ) );
			PackItem( new GreaterPoisonPotion() );
			PackItem( new GreaterPoisonPotion() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 70 ) < 1 )
			c.DropItem( new RainbowSpiderStatue() );

			if ( Utility.Random( 35 ) < 1 )
			c.DropItem( new AugustBook() );

			base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				if ( to is EvolutionDragon || to is EvoHiryu )
				damage *= 20;
			}
		}

		public override void Damage( int amount, Mobile from )
		{
			if ( from is BaseCreature )
			{
				if ( from is EvolutionDragon || from is EvoHiryu )
				amount = 0;
			}

			base.Damage( amount, from );
		}
		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			this.Hue = Utility.RandomMinMax( 2, 200 );

			if ( from != null && from.Player && !from.InRange( this, 4 ) )
			{
				this.Direction = this.GetDirectionTo( from );
				this.MovingEffect( from, 0x10D5, 10, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Paralyse ), from );
			}

			base.OnDamage(amount, from, willKill);
		}

		public void Paralyse( object state )
		{
			Mobile from = (Mobile)state;

			from.Freeze( TimeSpan.FromSeconds( 20 ) );

			SpidersWeb web = new SpidersWeb();
			web.MoveToWorld( from.Location, this.Map );

			Item toDisarm = from.FindItemOnLayer( Layer.OneHanded );

			if ( toDisarm == null || !toDisarm.Movable )
			toDisarm = from.FindItemOnLayer( Layer.TwoHanded );

			Container pack = from.Backpack;

			if ( toDisarm != null && pack != null )
			pack.DropItem( toDisarm );
			from.SendMessage( "You are tangled up in a web!" );
		}

		public RainbowSpider( Serial serial ) : base( serial )
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