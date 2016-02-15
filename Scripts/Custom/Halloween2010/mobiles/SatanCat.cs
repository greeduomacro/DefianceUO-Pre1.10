using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
	public class SatanCat : BaseCreature
	{
		[Constructable]
		public SatanCat() : base( AIType.AI_Mage, FightMode.Agressor, 20, 1, 0.3, 0.5 )
		{
			Name = "Satan's favorite cat";
			Body = 0x7F;
			BaseSoundID = 0x69;
			Hue = 0x3E8;

			SetStr(300, 500);
			SetDex(120, 130);
			SetInt(400, 600);

			SetHits( 20000 );

			SetDamage(20, 40);

			SetSkill( SkillName.MagicResist, 17.6, 25.0 );
			SetSkill( SkillName.Tactics, 67.6, 85.0 );
			SetSkill( SkillName.Wrestling, 120.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 100;
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 50 ) < 1 )
			c.DropItem( new HalloweenCandle() );

			if ( Utility.Random( 10 ) < 1 )
			c.DropItem( new HalloweenStatue() );

			base.OnDeath( c );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

 		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool AutoDispel { get { return true; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is HellCat || from is PredatorHellCat )
				damage *= 150;
		}

		public override void OnDamagedBySpell( Mobile from )
		{
			this.Combatant = from;

			if (from.Combatant == null)
				return;

			Mobile m = from.Combatant;

			if (m.Combatant == null)
				return;

			if ( Alive )
			switch ( Utility.Random( 3 ) )
			{
				case 0: m.Hits +=( Utility.Random( 1000 ) ); break;
				case 1: m.Hits += ( Utility.Random( 1500 ) ); break;
				case 2: m.Hits +=( Utility.Random( 2000 ) ); break;
			}
			from.PlaySound( 0x1F2 );
			from.SendMessage("This creature is immune to everything, purely evil!");
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 20;
			}

			if ( to is HellCat || to is PredatorHellCat )
				damage /= 20;
		}

		public SatanCat(Serial serial) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}