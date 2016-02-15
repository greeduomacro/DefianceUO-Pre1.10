using System;
using System.Collections;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
	public abstract class BaseHuntContractVendor : BaseVendor
	{
		public static double SkillNeeded = 60.0;
		public static double LargeBodSkillNeeded = 80.0;

		public BaseHuntContractVendor( string title ) : base( title )
		{
		}

		public BaseHuntContractVendor( Serial serial ) : base( serial )
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

		#region Hunting Contracts
		public override bool IsValidBulkOrder( Item item )
		{
			return ( item is SmallHuntBOD || item is LargeHuntBOD );
		}

		public override bool SupportsBulkOrders( Mobile from )
		{
			return true;
		}

		public override TimeSpan GetNextBulkOrder( Mobile from )
		{
			if ( from is PlayerMobile )
				return ((PlayerMobile)from).NextHuntContract;

			return TimeSpan.Zero;
		}

		public static bool CanUseContract( Mobile from )
		{
			if( from is PlayerMobile && from.Skills[ HuntBodUtility.GetBestFightingSkill( from ) ].Base >= SkillNeeded )
				return true;
			return false;
		}

		public override Item CreateBulkOrder( Mobile from, bool fromContextMenu )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null && pm.NextHuntContract == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()) )
			{
				double theirSkill = pm.Skills[ HuntBodUtility.GetBestFightingSkill( from ) ].Base;

				if ( theirSkill >= 100 )
					pm.NextHuntContract = TimeSpan.FromHours( 6.0 );
				else if ( theirSkill >= 80 )
					pm.NextHuntContract = TimeSpan.FromHours( 2.0 );
				else
					pm.NextHuntContract = TimeSpan.FromHours( 1.0 );

				if ( theirSkill >= LargeBodSkillNeeded && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble() )
					return LargeHuntBOD.CreateRandomFor( from, theirSkill );

				return SmallHuntBOD.CreateRandomFor( from, theirSkill );
			}
			return null;
		}

        public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if( IsValidBulkOrder( dropped ) && !CanUseContract( from ) )
			{
				SayTo( from, "You have to low fighting skills to turn in this order." );
				return false;
			}

			return base.OnDragDrop( from, dropped );
		}
		#endregion
	}
}