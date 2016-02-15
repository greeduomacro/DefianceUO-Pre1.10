using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a noxious corpse" )]
	public class FakeWarlord : BaseFakeMob
	{
		[Constructable]
		public FakeWarlord() : base( AIType.AI_Mage )
		{
			Name = "a noxious warlord";
			Body = 152;
			Hue = 1267;
			BaseSoundID = 0x24D;

			SetStr( 1000 );
			SetDex( 1000 );
			SetInt( 1000 );

			SetHits( 2000 );

			SetDamage( 18, 22 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Magery, 150.0 );
			SetSkill( SkillName.Poisoning, 150.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 100;
			Karma = -100;

			VirtualArmor = 50;
		}

		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage ) // Taken from DragChamp.cs and value changed
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				    damage /= 2;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage ) // Taken from DragChamp.cs and value changed
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				    damage *= 3;
			}
		}

		public FakeWarlord( Serial serial ) : base( serial )
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