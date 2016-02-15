using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Third;
using Server.Spells.Sixth;

namespace Server.Mobiles
{
	public class DungeonGuard : BaseBoss
	{
		[Constructable]
		public DungeonGuard() : base( AIType.AI_Melee )
		{
			Name = NameList.RandomName( "male" );
			Title = "the Guard";
			Fame = 5000;
			Karma = -5000;
			Hue = 1175;
			BodyValue = 400;;

			SetStr( 150 );
			SetDex( 100 );
			SetInt( 200 );

			SetHits( 1000 );

            		SetSkill( SkillName.MagicResist, 200.0 );
            		SetSkill( SkillName.Tactics, 100.0 );
            		SetSkill( SkillName.Swords, 120.0 );
			SetSkill( SkillName.DetectHidden, 100.0, 120.0 );

			PlateArms a = new PlateArms();
			a.Hue = 1109;
			a.LootType = LootType.Blessed;
			AddItem( a );

			Cloak c = new Cloak();
			c.Hue = 1157;
			c.LootType = LootType.Blessed;
			AddItem( c );

			BodySash b = new BodySash();
			b.Hue = 1157;
			b.LootType = LootType.Blessed;
			AddItem( b );

			PlateGloves g = new PlateGloves();
			g.Hue = 1109;
			g.LootType = LootType.Blessed;
			AddItem( g );

			PlateLegs l = new PlateLegs();
			l.Hue = 1109;
			l.LootType = LootType.Blessed;
			AddItem( l );

			PlateChest t = new PlateChest();
			t.Hue = 1109;
			t.LootType = LootType.Blessed;
			AddItem( t );

			PlateGorget n = new PlateGorget();
			n.Hue = 1109;
			n.LootType = LootType.Blessed;
			AddItem( n );

			PlateHelm h = new PlateHelm();
			h.Hue = 1109;
			h.LootType = LootType.Blessed;
			AddItem( h );

			VikingSword w = new VikingSword();
			w.Hue = 1109;
			w.Movable = false;
			w.LootType = LootType.Blessed;
			AddItem( w );

			ChaosShield s = new ChaosShield();
			s.Hue = 1109;
			s.Movable = false;
			s.LootType = LootType.Blessed;
			AddItem( s );
		}

		public override bool DoImmuneToPets { get { return true; } }
		public override int DoMoreDamageToPets { get { return 5; } }
		public override bool BardImmune{ get{ return true; } }
        	public override bool CanRummageCorpses{ get{ return true; } }
        	public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool DoDetectHidden { get { return true; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override bool DoDisarmPlayer{ get { return true; } }
		public override int CanBandageSelf{ get { return 50; } }

		public DungeonGuard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}