using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a mimic corpse" )]
	public class Mimic : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }

		[Constructable]
		public Mimic() : base( AIType.AI_Mage, FightMode.Agressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a mimic";
			Body = 58;
			Hue = 1175;
			BaseSoundID = 466;

			SetStr( 196, 225 );
			SetDex( 106, 125 );
			SetInt( 236, 245 );

			SetDamage( 15, 19 );

			SetSkill( SkillName.EvalInt, 60.0 );
			SetSkill( SkillName.Magery, 95.0 );
			SetSkill( SkillName.MagicResist, 130.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.Wrestling, 95.0 );
			SetSkill( SkillName.Meditation, 60.0 );

			Fame = 13000;
			Karma = -13000;

			VirtualArmor = 40;

			AddItem( new DarkSource() );

			PackGem( 5, 10 );
			PackGold( 500, 900 );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 4 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

		}

		public override void OnDamagedBySpell( Mobile from )
		{
			if (Combatant == null)
				return;

			if (Body == 58)
				Say( "I will steal your Body and now your Soul..." );

			if (Body != from.Body)
            {
                BoltEffect( 0 );

                Body = from.Body;
                Hue = from.Hue;
                Name = from.Name;

                Fame = from.Fame;
                Karma = (0 -from.Karma);
                Title = from.Title;

                Str = from.Str;
                Int = from.Int;
                Dex = from.Dex;

                Hits = from.Hits;

                Dex = from.Dex;
                Mana = from.Mana;
                Stam = from.Stam;

                Female = from.Female;

                VirtualArmor = (from.VirtualArmor);

                Item hair = new Item( Utility.RandomList( 8265 ) );
                hair.Hue = 1153;
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );

                JesterHat hat = new JesterHat();
                hat.Hue = 1175;
                hat.Movable = false;
                AddItem( hat );

                DeathRobe robe = new DeathRobe();
                robe.Hue = 1175;
                robe.Movable = false;
                AddItem( robe );

                Sandals sandals = new Sandals();
                sandals.Hue = 1;
                sandals.Movable = false;
                AddItem( sandals );

                Spellbook book = new Spellbook( UInt64.MaxValue );
                book.Hue = 1175;
                book.Movable = false;
                AddItem( book );

                BoltEffect( 0 );
			}
			switch ( Utility.Random( 6 ) )
			{
				case 0: Say( "You can not win with magic..." ); break;
				case 1: Say( "Your image is weak as is your mind..." ); break;
				case 2: Say( "It feels good to wear your skin..." ); break;
				case 3: Say( "I'll take over your life as soon as I end it..." ); break;
				case 4: Say( "Don't you like having a twin..." ); break;
				case 5: Say( "You lack the skills to defeat me..." ); break;
			}
			from.BoltEffect( 0 );
			from.Damage( Utility.Random( 20, 40 ) );
			Hits += ( Utility.Random( 20, 40 ) );
		}

		public Mimic( Serial serial ) : base( serial )
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