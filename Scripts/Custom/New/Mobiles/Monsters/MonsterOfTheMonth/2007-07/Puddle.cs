using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a puddle" )]
	public class Puddle : BaseBoss
	{
		[Constructable]
		public Puddle() : base( AIType.AI_Melee )
		{
			Name = "a puddle";
			Body = 51;
			BaseSoundID = 456;

			Hue = Utility.RandomSlimeHue();

			SetStr( 200, 250 );
			SetDex( 200, 250 );
			SetInt( 200, 250 );

			SetHits( 1200 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.Poisoning, 110, 120.0 );
			SetSkill( SkillName.MagicResist, 500.0 );
			SetSkill( SkillName.Tactics, 110, 120.0 );
			SetSkill( SkillName.Wrestling, 100, 120.0 );
			SetSkill( SkillName.Anatomy, 110.0, 120 );

			PackItem( new SpidersSilk( 5 ) );
			PackItem( new BlackPearl( 5 ) );
			PackItem( new Bloodmoss( 5 ) );
			PackItem( new Garlic( 5 ) );
			PackItem( new Ginseng( 5 ) );
			PackItem( new MandrakeRoot( 5 ) );
			PackItem( new Nightshade ( 5 ) );
			PackItem( new SulfurousAsh( 5 ) );
			PackItem( new LesserPoisonPotion() );
			PackItem( new PoisonPotion() );
			PackItem( new GreaterPoisonPotion() );
			PackItem( new Bone() );
			PackItem( new Arrow( 6 ) );
			PackItem( new Bolt( 6 ) );
			PackItem( new Bandage( 6 ) );

			if ( Utility.Random( 20 ) < 1 )
			PackItem( new JulyBook() );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 1.0; } }
		public override bool DoSkillLoss{ get{ return true; } }
		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool DoDetectHidden { get{ return true; } }
		public override bool DoImmuneToPets { get { return true; } }
		public override bool DoImmuneToPlayers { get { return true; } }
        public override bool BardImmune { get { return true; } }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
            base.OnGaveMeleeAttack(defender);
            if (defender.Frozen == false)
			{
				defender.Freeze( TimeSpan.FromSeconds( 6.0 ) );
				defender.SendMessage( "You are stuck to the spot!" );
			}
		}

		public override void OnThink()
		{
			Container pack = this.Backpack;

			if ( pack.TotalItems < 1 )
			{
				this.Kill();
			}
			base.OnThink();
		}

		public Puddle( Serial serial ) : base( serial )
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