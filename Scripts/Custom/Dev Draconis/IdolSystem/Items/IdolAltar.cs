using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Engines.IdolSystem
{
	public class IdolPlatform : BaseAddon
	{
		private IdolPedestal m_Shame, m_Deceit, m_Destard, m_Hythloth, m_Despise, m_Covetous, m_FireLord, m_DragonKing, m_Undeath, m_Custom;

		[Constructable]
		public IdolPlatform()
		{
			AddonComponent comp = new AddonComponent( 0x7A3 );
			comp.Hue = 0x4AA;
			AddComponent( comp, 0, 0, -2 );

			comp = new AddonComponent( 0x7A7 );
			comp.Hue = 0x4AA;
			AddComponent( comp, -1, 0, 0 );

			comp = new AddonComponent( 0x7A4 );
			comp.Hue = 0x4AA;
			AddComponent( comp, 0, 1, 0 );

			comp = new AddonComponent( 0x7A5 );
			comp.Hue = 0x4AA;
			AddComponent( comp, 1, 0, 0 );

			comp = new AddonComponent( 0x7A6 );
			comp.Hue = 0x4AA;
			AddComponent( comp, 0, -1, 0 );

			comp = new AddonComponent( 0x12D9);
			comp.Hue = 0x4AA;
			comp.Name = "Guardians of the Dark Master";
			AddComponent( comp, -4, 0, 0 );

			comp = new AddonComponent( 0x12D9);
			comp.Hue = 0x4AA;
			comp.Name = "Guardians of the Dark Master";
			AddComponent( comp, 4, 0, 0 );

			AddComponent( m_Shame = new IdolPedestal( this, IdolType.Shame ),  -4, -2, 0 );

			AddComponent( m_Deceit = new IdolPedestal( this, IdolType.Deceit ),  -4, 2, 0 );

			AddComponent( m_Destard = new IdolPedestal( this, IdolType.Destard ), -2,  4, 0 );

			AddComponent( m_Hythloth = new IdolPedestal( this, IdolType.Hythloth ),  0,  4, 0 );

			AddComponent( m_Despise = new IdolPedestal( this, IdolType.Despise ), 2,  4, 0 );

			AddComponent( m_Covetous = new IdolPedestal( this, IdolType.Covetous ),  4,  2, 0 );

			AddComponent( m_FireLord = new IdolPedestal( this, IdolType.FireLord ),  -2,  -4, 0 );

			AddComponent( m_DragonKing = new IdolPedestal( this, IdolType.DragonKing ),  2,  -4, 0 );

			AddComponent( m_Undeath = new IdolPedestal( this, IdolType.Undeath ),  0, -4, 0 );

			AddComponent( m_Custom = new IdolPedestal( this, IdolType.Wrong ),  4, -2, 0 );
		}

		public void Validate()
		{
			if (Validate(m_Custom) && Validate(m_Shame) && Validate(m_Deceit) && Validate(m_Destard) && Validate(m_Hythloth) && Validate(m_Despise) && Validate(m_Covetous) && Validate(m_FireLord) && Validate(m_DragonKing) && Validate(m_Undeath))
			{
				Mobile darkmaster = DarkMaster.Spawn( new Point3D( X, Y, Z + 3 ), this.Map );

				if ( darkmaster == null )
					return;

				Clear( m_Shame );
				Clear( m_Deceit );
				Clear( m_Destard );
				Clear( m_Hythloth );
				Clear( m_Despise );
				Clear( m_Covetous );
				Clear( m_FireLord );
				Clear( m_DragonKing );
				Clear( m_Undeath );
				Clear( m_Custom );
			}
		}

		public void Clear( IdolPedestal Pedestal )
		{
			if ( Pedestal != null )
			{
				Effects.SendBoltEffect( Pedestal );

				if ( Pedestal.Idol != null )
					Pedestal.Idol.Delete();
			}
		}

		public bool Validate( IdolPedestal Pedestal )
		{
			return ( Pedestal != null && Pedestal.Idol != null && !Pedestal.Idol.Deleted );
		}

		public IdolPlatform( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Shame );
			writer.Write( m_Deceit );
			writer.Write( m_Destard );
			writer.Write( m_Hythloth );
			writer.Write( m_Despise );
			writer.Write( m_Covetous );
			writer.Write( m_FireLord );
			writer.Write( m_DragonKing );
			writer.Write( m_Undeath );
			writer.Write( m_Custom );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Shame = reader.ReadItem() as IdolPedestal;
					m_Deceit = reader.ReadItem() as IdolPedestal;
					m_Destard = reader.ReadItem() as IdolPedestal;
					m_Hythloth = reader.ReadItem() as IdolPedestal;
					m_Despise = reader.ReadItem() as IdolPedestal;
					m_Covetous = reader.ReadItem() as IdolPedestal;
					m_FireLord = reader.ReadItem() as IdolPedestal;
					m_DragonKing = reader.ReadItem() as IdolPedestal;
					m_Undeath = reader.ReadItem() as IdolPedestal;
					m_Custom = reader.ReadItem() as IdolPedestal;

					break;
				}
			}
		}
	}

	public class IdolPedestal : AddonComponent
	{
		private IdolPlatform m_Platform;
		private IdolType m_Type;
		private Item m_Idol;

		[CommandProperty( AccessLevel.GameMaster )]
		public IdolPlatform Platform{ get{ return m_Platform; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public IdolType Type{ get{ return m_Type; } set{ m_Type = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Item Idol{ get{ return m_Idol; } set{ m_Idol = value; if ( m_Platform != null ) m_Platform.Validate(); } }

		public IdolPedestal( IdolPlatform platform, IdolType type ) : base( 0x1F2A )
		{
			Hue = 0x4AA;
			Light = LightType.Circle300;
			Name = "Pedestal of " + type;
			m_Platform = platform;
			m_Type = type;
		}

		public IdolPedestal( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Platform != null )
				m_Platform.Validate();

			BeginSacrifice( from );
		}

        public static bool DarkMasterActive()
        {
            return (EthyDragChamp.Active || EthyElementalChamp.Active || EthyLichChamp.Active || DarkMaster.Active || EthyDarkMaster.Active);
        }

		public void BeginSacrifice( Mobile from )
		{
			if ( Deleted )
				return;

			if ( m_Idol != null && m_Idol.Deleted )
				Idol = null;

			if ( from.Map != this.Map || !from.InRange( GetWorldLocation(), 3 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( DarkMasterActive() )
			{
                from.SendMessage("The Dark Master has already been summoned.");
			}
			else if ( m_Idol == null )
			{
				from.SendLocalizedMessage( 1049485 ); // What would you like to sacrifice?
				from.Target = new SacrificeTarget( this );
			}
			else
			{
				from.SendMessage( "I already hold that Idol!" );
			}
		}

		public void EndSacrifice( Mobile from, Idol idol )
		{
			if ( Deleted )
				return;

			if ( m_Idol != null && m_Idol.Deleted )
				Idol = null;

			if ( from.Map != this.Map || !from.InRange( GetWorldLocation(), 3 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( DarkMasterActive() )
			{
                from.SendMessage("The Dark Master has already been summoned.");
			}
			else if ( idol == null )
			{
				from.SendMessage( "That is not my Idol!" );
			}
			else if ( m_Idol != null )
			{
				from.SendMessage( "I already hold that Idol!" );
			}
			else if ( !idol.IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "You can only sacrifice items that are in your backpack!" );
			}
			else
			{
				if ( idol.Type == this.Type )
				{
					idol.Movable = false;
					idol.MoveToWorld( GetWorldTop(), this.Map );

					this.Idol = idol;
				}
				else
				{
					from.SendMessage( "That is not my Idol!" );
				}
			}
		}

		private class SacrificeTarget : Target
		{
			private IdolPedestal m_Pedestal;

			public SacrificeTarget( IdolPedestal Pedestal ) : base( 12, false, TargetFlags.None )
			{
				m_Pedestal = Pedestal;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				m_Pedestal.EndSacrifice( from, targeted as Idol );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Type );
			writer.Write( m_Platform );
			writer.Write( m_Idol );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Type = (IdolType)reader.ReadInt();
					m_Platform = reader.ReadItem() as IdolPlatform;
					m_Idol = reader.ReadItem();

					if ( m_Platform == null )
						Delete();

					break;
				}
			}

			if ( Hue == 0x497 )
				Hue = 0x455;

			if ( Light != LightType.Circle300 )
				Light = LightType.Circle300;
		}
	}
}