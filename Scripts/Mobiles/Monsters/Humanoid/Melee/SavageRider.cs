using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage corpse" )]
	public class SavageRider : BaseCreature
	{
		[Constructable]
		public SavageRider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = NameList.RandomName( "savage rider" );

			if ( Female = Utility.RandomBool() )
				Body = 186;
			else
				Body = 185;

			SetStr( 151, 164 );
			SetDex( 92, 108 );
			SetInt( 52, 65 );

			SetDamage( 29, 34 );

			SetSkill( SkillName.Fencing, 80.5, 86.0 );
			SetSkill( SkillName.Healing, 60.3, 90.0 );
			SetSkill( SkillName.MagicResist, 81.5, 85.0 );
			SetSkill( SkillName.Tactics, 80.5, 86.0 );

			Fame = 1250;
			Karma = -1250;

			VirtualArmor = 10;

			PackGold( 100, 150 );
			PackItem( new Bandage( Utility.RandomMinMax( 10, 15 ) ) );

			if ( 0.4 > Utility.RandomDouble() )
				PackItem( new BolaBall() );

			AddItem( new TribalSpear() );
			AddItem( new BoneArms() );
			AddItem( new BoneLegs() );
			AddItem( new BearMask() );

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
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon || to is EvolutionDragon )
				damage *= 5;
		}

		public SavageRider( Serial serial ) : base( serial )
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