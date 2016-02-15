using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;


namespace Server.Items
{
	public class VetDeerMask : BaseHat , Engines.VeteranRewards.IRewardItem
	{
		string m_VetDeerMask;
		private bool m_IsRewardItem;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		[Constructable]
		public VetDeerMask() : base( 0x1547 )
		{	
			Name = "a special deer mask";
			Weight = 9.0;
			LootType = LootType.Blessed;
			
		}

		public VetDeerMask( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_VetDeerMask );
			writer.Write( (bool) m_IsRewardItem );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		
			switch ( version )
			{
				case 0:
				{
					m_VetDeerMask = reader.ReadString(); break;
					m_IsRewardItem = reader.ReadBool(); break;
				}
			}
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}


		public override bool OnEquip( Mobile from )
		{	
			if ( m_IsRewardItem && !Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( from, this, null ) )
				return false;

			else
				m_VetDeerMask = GetAccountDuration( from );
				Name = String.Format("This deer mask shows the whole Defiance community that my account is {0}", m_VetDeerMask);
				Hue = 1150;

				return true;
			
		}

		private static string GetAccountDuration( Mobile m )
		{
			Account a = m.Account as Account;

			if ( a == null )
				return "";

			TimeSpan ts = DateTime.Now - a.Created;			

			string v;

			if ( Format( ts.TotalDays, "{0} day{1} old.", out v ) )
				return v;

			return v;

		}

		
		public static bool Format( double value, string format, out string op )
		{
			if ( value >= 1.0 )
			{
				op = String.Format( format, (int)value, (int)value != 1 ? "s" : "" );
				return true;
			}

			op = null;
			return false;
		}

		public override void OnRemoved( object parent )
		{
			Name = "a special deer mask";
			LootType = LootType.Blessed;
			Hue = 0;
		}
	}
}