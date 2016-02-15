using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;

namespace Server.Regions
{
	public class FeluccaDungeon : DungeonRegion
	{
		public static void Initialize()
		{
			Region.AddRegion( new FeluccaDungeon( "Covetous" ) );
			Region.AddRegion( new FeluccaDungeon( "Deceit" ) );
			Region.AddRegion( new FeluccaDungeon( "Despise" ) );
			Region.AddRegion( new FeluccaDungeon( "Destard" ) );
			Region.AddRegion( new FeluccaDungeon( "Hythloth" ) );
			Region.AddRegion( new FeluccaDungeon( "Shame" ) );
			Region.AddRegion( new FeluccaDungeon( "Wrong" ) );
			Region.AddRegion( new FeluccaDungeon( "Terathan Keep" ) );
			Region.AddRegion( new FeluccaDungeon( "Fire" ) );
			Region.AddRegion( new FeluccaDungeon( "Ice" ) );
		}

		public FeluccaDungeon( string name ) : base( name, Map.Felucca )
		{
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void OnEnter( Mobile m )
		{
			//base.OnEnter( m ); // You have entered the dungeon {0}
		}

		public override void OnExit( Mobile m )
		{
			//base.OnExit( m );
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = LightCycle.DungeonLevel;
		}

		public override bool OnBeginSpellCast( Mobile m, ISpell s )
		{
         ///*if ( s is GateTravelSpell || s is RecallSpell || s is MarkSpell )
         //{
         //   m.SendMessage( "You cannot cast that spell here." );
         //   return false;
         //}
         //else
         //{
         //   return base.OnBeginSpellCast( m, s );
         //}*/
         return true;
     		 }
	}
}