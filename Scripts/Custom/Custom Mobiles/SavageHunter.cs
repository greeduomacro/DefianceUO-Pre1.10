using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage corpse" )]
	public class SavageHunter : BaseCreature
	{
		[Constructable]
		public SavageHunter() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = "a savage hunter";

			Body = 185;
			SetStr( 198, 210 );
			SetDex( 92, 130 );
			SetInt( 51, 65 );
			SetHits( 450, 525 );

			SetDamage( 30, 36 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Archery, 112.5, 120.0 );
			SetSkill( SkillName.Healing, 60.3, 90.0 );
			SetSkill( SkillName.Macing, 72.5, 95.0 );
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 72.5, 95.0 );
			SetSkill( SkillName.Swords, 72.5, 95.0 );
			SetSkill( SkillName.Tactics, 102.5, 115.0 );

			Fame = 3000;
			Karma = -3000;

			PackGold( 600, 900 );
			PackItem( new Bandage( Utility.RandomMinMax( 1, 15 ) ) );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new BolaBall() );

			AddItem( new TribalBow() );
			AddItem( new BoneArms() );
			AddItem( new BoneLegs() );
			AddItem( new BoneGloves() );
			//AddItem( new bearmask() );

			new SavageRidgeback().Rider = this;
		}

		public override int Meat{ get{ return 1; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( mount is Mobile )
				((Mobile)mount).Delete();

			return base.OnBeforeDeath();
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( m.BodyMod == 183 || m.BodyMod == 184 )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( aggressor.BodyMod == 183 || aggressor.BodyMod == 184 )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				aggressor.BodyMod = 0;
				aggressor.HueMod = -1;
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
				aggressor.SendLocalizedMessage( 1040008 ); // Your skin is scorched as the tribal paint burns away!

				if ( aggressor is PlayerMobile )
					((PlayerMobile)aggressor).SavagePaintExpiration = TimeSpan.Zero;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon )
				damage *= 5;
		}

		public SavageHunter( Serial serial ) : base( serial )
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