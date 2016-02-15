using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Spells.Seventh;
using Server.Spells.Fifth;

namespace Server.Mobiles
{
	public class Werewolf : BaseCreature
	{
		[Constructable]
		public Werewolf(): base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "A werewolf";
			Body = 250;
			Hue = 2312;
			BaseSoundID = 0xE5;

			SetStr(586, 700);
			SetHits(1500, 2000);

			SetDamage (30, 45);

			SetDamageType(ResistanceType.Physical, 120);

			SetResistance(ResistanceType.Physical, 20, 25);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 5, 10);
			SetResistance(ResistanceType.Poison, 5, 10);
			SetResistance(ResistanceType.Energy, 10, 15);

			SetSkill(SkillName.MagicResist, 150.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 55;
			//Say("AaRrrrooooooooo!");
		}

		protected override void OnMapChange( Map oldMap )
		{
			Say("AaRrrrooooooooo!");
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 95 ) <  1 )
				c.DropItem( new WerewolfTooth( ToothType.Cursed ));

			Container bag = new Bag();
			c.DropItem( bag );
			bag.DropItem( new Gold( 400, 500 ) );

			if ( Utility.Random( 95 ) < 1 )
				bag.DropItem( new WerewolfTooth( ToothType.Normal ) );

			base.OnDeath( c );
		}
/*
		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}
*/
		public override bool OnBeforeDeath()
		{
			foreach ( Mobile m in GetMobilesInRange( 20 ) )
			{
				m.Poison = Poison.Lethal;
				m.Freeze( TimeSpan.FromSeconds( 25.0 ) );
				m.SendMessage( "The werewolf has been slain, but ..... !?" );
			}

			return base.OnBeforeDeath();
		}

		//Melee damage from controlled mobiles is divided by 30
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
					damage /= 30;
			}
		}

		//Melee damage to controlled mobiles is multiplied by 15
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
					damage *= 15;
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.60 >= Utility.RandomDouble())
				Polymorph( defender );
		}

		public void Polymorph( Mobile m ) //taken from evilvampire.cs and the values changed
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 250;
				m.HueMod = 0;
				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 1.5 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

		public Werewolf( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
		}
	}
}