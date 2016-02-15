using System;
using Server.Misc;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Accounting;

namespace Server.Mobiles
{
	public class EtherealMount : Item, IMount, IMountItem, Engines.VeteranRewards.IRewardItem
	{
		private int m_MountedID;
		private int m_RegularID;
		private Mobile m_Rider, m_Owner;
		private string m_Account;
		private bool m_IsRewardItem, m_Donation;
		private int m_Stamina = 250;
		private int m_OriginalHue;
		private int m_HueSequence;
		private bool m_Updated = false;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Donation
		{
			get{ return m_Donation; }
			set{ m_Donation = value; }
		}

		public int Stamina
		{

			get{ return m_Stamina; }
			set{ m_Stamina = value; }
		}

		public static void StopMounting( Mobile mob )
		{
			if ( mob.Spell is EtherealSpell )
				((EtherealSpell)mob.Spell).Stop();
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		[Constructable]
		public EtherealMount( int itemID, int mountID ) : base( itemID )
		{
			m_MountedID = mountID;
			m_RegularID = itemID;
			m_Rider = null;

			Layer = Layer.Invalid;

			LootType = LootType.Blessed;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MountedID
		{
			get
			{
				return m_MountedID;
			}
			set
			{
				if ( m_MountedID != value )
				{
					m_MountedID = value;

					if ( m_Rider != null )
						ItemID = value;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RegularID
		{
			get
			{
				return m_RegularID;
			}
			set
			{
				if ( m_RegularID != value )
				{
					m_RegularID = value;

					if ( m_Rider == null )
						ItemID = value;
				}
			}
		}

		/*public override bool OnDroppedInto( Mobile from, Container target, Point3D p )
		{
			if (m_Donation && from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedInto( from, target, p );
		}*/

		/**public override bool OnDroppedOnto( Mobile from, Item target )
		{
			if (m_Donation && from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedOnto( from, target );
		}/

		/*public override bool OnDroppedToMobile( Mobile from, Mobile target )
		{
			if (m_Donation && from.AccessLevel < AccessLevel.GameMaster && target.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToMobile( from, target );
		}*/

		/*public override bool OnDroppedToWorld( Mobile from, Point3D p )
		{
			if (m_Donation && from.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToWorld( from, p );
		}*/

		/*public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if (m_Donation && from.AccessLevel < AccessLevel.GameMaster && to.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}*/

		public EtherealMount( Serial serial ) : base( serial )
		{
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnSingleClick( Mobile from )
		{
			if ( !m_IsRewardItem )
				LabelTo( from, String.Format( "[no age{0}]", Hue != 0 ? " rainbow" : "" ) );
			base.OnSingleClick( from );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( m_IsRewardItem && !Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( from, this, null ) )
				return;
			else if ( !from.CanBeginAction( typeof( BaseMount ) ) )
				from.SendLocalizedMessage( 1040024 ); // You are still too dazed from being knocked off your mount to ride!
			else if ( from.Mounted )
				from.SendLocalizedMessage( 1005583 ); // Please dismount first.
			else if ( EventFlag.ExistsOn( from ) )
				from.SendLocalizedMessage( 1061632 ); // EventFlag Add by XLX.
                        else if ( from.IsBodyMod && !from.Body.IsHuman )
				from.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
			else if ( Multis.DesignContext.Check( from ) )
				new EtherealSpell( this, from ).Cast();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 6 ); // version

			writer.Write( m_OriginalHue ); // 6
			writer.Write( m_HueSequence );
			writer.Write( m_Updated );

			writer.Write( m_Owner ); // 5

			writer.Write( m_Account ); // 4

			writer.Write( (bool) m_Donation ); // 3

			writer.Write( (bool) m_IsRewardItem ); // 2

			writer.Write( (int)m_MountedID );
			writer.Write( (int)m_RegularID );
			writer.Write( m_Rider );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 6:
				{
					m_OriginalHue = reader.ReadInt();
					m_HueSequence = reader.ReadInt();// % DonationSystem.DonationHues.Length;
					m_Updated = reader.ReadBool();
					goto case 5;
				}
				case 5:
				{
					m_Owner = reader.ReadMobile();
					goto case 4;
				}
				case 4:
				{
					m_Account = reader.ReadString();
					goto case 3;
				}
				case 3:
				{
					m_Donation = reader.ReadBool();
					m_IsRewardItem = reader.ReadBool();
					goto case 0;
				}
				case 2:
				{
					m_Donation = false;
					m_IsRewardItem = reader.ReadBool();
					goto case 0;
				}
				case 1: reader.ReadInt(); goto case 0;
				case 0:
				{
					m_MountedID = reader.ReadInt();
					m_RegularID = reader.ReadInt();
					m_Rider = reader.ReadMobile();

					if ( m_MountedID == 0x3EA2 )
						m_MountedID = 0x3EAA;

					break;
				}
			}
		}

		public override DeathMoveResult OnParentDeath( Mobile parent )
		{
			Rider = null;//get off, move to pack

			return DeathMoveResult.RemainEquiped;
		}

		public static void Dismount( Mobile m )
		{
			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Rider
		{
			get
			{
				return m_Rider;
			}
			set
			{
				if ( value != m_Rider )
				{
					if ( value == null )
					{
						if ( m_Updated && m_OriginalHue == 0 )
							Hue = m_OriginalHue;

						Internalize();
						UnmountMe();

						m_Rider = value;
					}
					else
					{
						if ( m_Rider != null )
							Dismount( m_Rider );

						Dismount( value );

						m_Rider = value;

						if ( m_Rider is PlayerMobile && ((PlayerMobile)m_Rider).HasDonated && ((PlayerMobile)m_Rider).DonationTimeLeft >= TimeSpan.FromDays( 90.0 ) )
						{
							m_Updated = true;
							m_OriginalHue = Hue;
							if ( m_OriginalHue == 0 )
							{
								Hue = DonationSystem.GetHorseHue( (Account)m_Rider.Account );//DonationSystem.DonationHues[m_HueSequence++ % DonationSystem.DonationHues.Length];
							}
						}


						MountMe();
					}
				}
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public string Account
		{
			get
			{
				return m_Account;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get
			{
				return m_Owner;
			}
			set
			{
				m_Owner = value;
				m_Account = ((Account)value.Account).Username;
			}
		}

		public void UnmountMe()
		{
			Container bp = m_Rider.Backpack;

			ItemID = m_RegularID;
			Layer = Layer.Invalid;
			Movable = true;

			if ( Hue == 0x4001 )
				Hue = 0;

			if ( bp != null )
			{
				bp.DropItem( this );
			}
			else
			{
				Point3D loc = m_Rider.Location;
				Map map = m_Rider.Map;

				if ( map == null || map == Map.Internal )
				{
					loc = m_Rider.LogoutLocation;
					map = m_Rider.LogoutMap;
				}

				MoveToWorld( loc, map );
			}
		}

		public void MountMe()
		{
			ItemID = m_MountedID;
			Layer = Layer.Mount;
			Movable = false;

			//if ( Hue == 0 )
			//	Hue = 0x4001;

			//if ( ItemID == 0x3E92 )
			//	Hue = 0x0;

			//if ( ItemID == 0x3E91 )
			//	Hue = 0x0;

			//if ( ItemID == 0x3E90 )
			//	Hue = 0x0;

			ProcessDelta();
			m_Rider.ProcessDelta();
			m_Rider.EquipItem( this );
			m_Rider.ProcessDelta();
			ProcessDelta();

            if (m_Rider is PlayerMobile)
                ((PlayerMobile)m_Rider).EtherealStam = StamSystem.etherealstam;
		}

		public IMount Mount
		{
			get
			{
				return this;
			}
		}

		private class EtherealSpell : Spell
		{
			private static SpellInfo m_Info = new SpellInfo( "Ethereal Mount", "", SpellCircle.Second, 230 );

			private EtherealMount m_Mount;
			private Mobile m_Rider;

			public EtherealSpell( EtherealMount mount, Mobile rider ) : base( rider, null, m_Info )
			{
				m_Rider = rider;
				m_Mount = mount;

				mount.Stamina = 100;
			}

			public override bool ClearHandsOnCast{ get{ return false; } }
			public override bool RevealOnCast{ get{ return false; } }

			public override TimeSpan GetCastRecovery()
			{
				return TimeSpan.Zero;
			}

			public override TimeSpan GetCastDelay()
			{
				return TimeSpan.FromSeconds( 1.5 );
			}

			public override int GetMana()
			{
				return 0;
			}

			public override bool ConsumeReagents()
			{
				return true;
			}

			public override bool CheckFizzle()
			{
				return true;
			}

			private bool m_Stop;

			public void Stop()
			{
				m_Stop = true;
				Disturb( DisturbType.Hurt, false, false );
			}

			public override bool CheckDisturb( DisturbType type, bool checkFirst, bool resistable )
			{
//				if ( type == DisturbType.EquipRequest || type == DisturbType.UseRequest/* || type == DisturbType.Hurt*/ )
					return false;

//				return true;
			}

			public override void OnDisturb( DisturbType type, bool message )
			{
//				if ( message )
//					Caster.SendLocalizedMessage( 1049455 ); // You have been disrupted while attempting to summon your ethereal mount!

				//m_Mount.UnmountMe();
			}

			public override void OnCast()
			{
				if ( !m_Mount.Deleted && m_Mount.Rider == null && m_Mount.IsChildOf( m_Rider.Backpack ) )
					m_Mount.Rider = m_Rider;

				FinishSequence();
			}
		}
	}

	public class EtherealHorse : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041298; } } // Ethereal Horse Statuette

		[Constructable]
		public EtherealHorse() : base( 0x20DD, 0x3EAA )
		{
            Name = "an ethereal horse";
        }

		public EtherealHorse( Serial serial ) : base( serial )
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
            Name = "an ethereal horse";
			if ( ItemID == 0x2124 )
				ItemID = 0x20DD;
		}
	}

	public class EtherealLlama : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041300; } } // Ethereal Llama Statuette

		[Constructable]
		public EtherealLlama() : base( 0x20F6, 0x3EAB )
		{
            Name = "an ethereal llama";
        }

		public EtherealLlama( Serial serial ) : base( serial )
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
            Name = "an ethereal llama";
		}
	}

	public class EtherealOstard : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041299; } } // Ethereal Ostard Statuette

		[Constructable]
		public EtherealOstard() : base( 0x2135, 0x3EAC )
		{
            Name = "an ethereal ostard";
        }

		public EtherealOstard( Serial serial ) : base( serial )
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
            Name = "an ethereal ostard";
		}
	}

	public class EtherealSkeletal : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041298; } } // Ethereal ethereal skeletal steed

		[Constructable]
		public EtherealSkeletal() : base( 0x2617, 0x3EBB )
		{
            Name = "an ethereal skeletal steed";
        }

		public EtherealSkeletal( Serial serial ) : base( serial )
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
            Name = "an ethereal skeletal steed";
		}
	}


        public class EtherealLongManeHorse : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041298; } } // Ethereal Horse Statuette

		[Constructable]
		public EtherealLongManeHorse() : base( 0x20DD, 0x3EA9 )
		{
            Name = "an ethereal long mane horse";
        }

		public EtherealLongManeHorse( Serial serial ) : base( serial )
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
            Name = "an ethereal long mane horse";
			if ( ItemID == 0x2124 )
				ItemID = 0x20DD;
		}
	}

        public class DonationColourHorse : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041298; } } // Ethereal Horse Statuette

		[Constructable]
		public DonationColourHorse() : base( 0x20DD, 0x3EA9 )
		{
            Name = "an ethereal long mane horse";
        }

		public DonationColourHorse( Serial serial ) : base( serial )
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
            Name = "an ethereal horse";
			if ( ItemID == 0x2124 )
				ItemID = 0x20DD;
		}
	}



	// Elf Fun! -------------------------------------

	public class EtherealWarSteed : EtherealMount
	{
		[Constructable]
		public EtherealWarSteed() : base( 0x2D9C, 0x3E92 )
		{
                Name = "charger of the fallen";
        }

		public EtherealWarSteed( Serial serial ) : base( serial )
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
            Name = "an ethereal war horse";
		}
	}


	public class EtherealWolfRider : EtherealMount
	{
		public override int LabelNumber{ get{ return 1041298; } } // Ethereal ethereal skeletal steed

		[Constructable]
		public EtherealWolfRider() : base( 0x2D96, 0x3E91 )
		{
            Name = "an ethereal wolf rider";
        }

		public EtherealWolfRider( Serial serial ) : base( serial )
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
			Name = "an ethereal wolf rider";
		}
	}

	public class EtherealDragonSteed : EtherealMount
	{
		[Constructable]
		public EtherealDragonSteed() : base( 0x2D95, 0x3E90 )
		{
            Name = "an ethereal dragon steed";
        }

		public EtherealDragonSteed( Serial serial ) : base( serial )
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
            Name = "an ethereal dragon steed";
		}
	}

 	// End of Elf Fun! -------------------------------------
	public class EtherealRidgeback : EtherealMount
	{
		[Constructable]
		public EtherealRidgeback() : base( 0x2615, 0x3E9A )
		{
            Name = "an ethereal ridgeback";
        }

		public EtherealRidgeback( Serial serial ) : base( serial )
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
            Name = "an ethereal ridgeback";
		}
	}

	public class EtherealUnicorn : EtherealMount
	{
		public override int LabelNumber{ get{ return 1049745; } } // Ethereal Unicorn Statuette

		[Constructable]
		public EtherealUnicorn() : base( 0x25CE, 0x3E9B )
		{
            Name = "an ethereal unicorn";
        }

		public EtherealUnicorn( Serial serial ) : base( serial )
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
            Name = "an ethereal unicorn";
		}
	}

	public class EtherealBeetle : EtherealMount
	{
		public override int LabelNumber{ get{ return 1049748; } } // Ethereal Beetle Statuette

		[Constructable]
		public EtherealBeetle() : base( 0x260F, 0x3E97 )
		{
            Name = "an ethereal beetle";
		}

		public EtherealBeetle( Serial serial ) : base( serial )
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
            Name = "an ethereal beetle";
		}
	}

	public class EtherealKirin : EtherealMount
	{
		public override int LabelNumber{ get{ return 1049746; } } // Ethereal Ki-Rin Statuette

		[Constructable]
		public EtherealKirin() : base( 0x25A0, 0x3E9C )
		{
            Name = "an ethereal kirin";
        }

		public EtherealKirin( Serial serial ) : base( serial )
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
            Name = "an ethereal kirin";
		}
	}

	public class EtherealSwampDragon : EtherealMount
	{
		public override int LabelNumber{ get{ return 1049749; } } // Ethereal Swamp Dragon Statuette

		[Constructable]
		public EtherealSwampDragon() : base( 0x2619, 0x3E98 )
		{
			Name = "an ethereal swamp dragon";
		}

		public EtherealSwampDragon( Serial serial ) : base( serial )
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
            Name = "an ethereal swamp dragon";
        }
	}

    public class EtherealPolarBear : EtherealMount
    {
        [Constructable]
        public EtherealPolarBear() : base(8417, 16069)
        {
            Name = "an ethereal polar bear";
        }

        public EtherealPolarBear(Serial serial) : base(serial)
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

    public class EtherealPolarBearFrostmane : EtherealPolarBear
    {
        [Constructable]
        public EtherealPolarBearFrostmane() : base()
        {
            Name = "an ethereal frostmane bear";
            Hue = 1150;
        }

        public EtherealPolarBearFrostmane(Serial serial) : base(serial)
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

    public class EtherealPolarBearIcerugged : EtherealPolarBear
    {
        [Constructable]
        public EtherealPolarBearIcerugged() : base()
        {
            Name = "an ethereal icerugged bear";
            Hue = 1154;
        }

        public EtherealPolarBearIcerugged(Serial serial)
            : base(serial)
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

    public class EtherealPolarBearColdbane : EtherealPolarBear
    {
        [Constructable]
        public EtherealPolarBearColdbane() : base()
        {
            Name = "an ethereal coldbane bear";
            Hue = 1151;
        }

        public EtherealPolarBearColdbane(Serial serial) : base(serial)
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

    public class GameEthereal : EtherealHorse
    {
		[Constructable]
		public GameEthereal() : base()
		{
            this.Name = "Game Ethereal";
		}

        public GameEthereal( Serial serial ) : base( serial )
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

        public override void OnSingleClick(Mobile from)
        {
           LabelTo(from, "Game Ethereal Horse");
        }
    }
}