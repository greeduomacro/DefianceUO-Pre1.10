using System;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using System.Reflection;

namespace Server.Items
{
	public class EtherealStaff : Item
	{
		EtherealStaffMob m_StaffCharacter;
		bool m_StaffCharacterVisible;
		bool m_Renew;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RenewDummy
		{
			get{ return m_Renew; }
			set{ m_Renew = value; }
		}

		[Constructable]
		public EtherealStaff()
		{
			ItemID = 8397;

			Layer = Layer.Invalid;

			m_StaffCharacter = null;
			m_StaffCharacterVisible = false;
			m_Renew = false;

			LootType = LootType.Blessed;
		}

		public EtherealStaff( Serial serial ) : base( serial )
		{
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnSingleClick( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.Counselor )
				this.Delete();
			//base.OnSingleClick( from );
			if( m_StaffCharacter != null )
				LabelTo( from, "Ethereal {0} Statuette", m_StaffCharacter.Name );
			else
				LabelTo( from, "Ethereal Staff Statuette" );
			LabelTo( from, "In Use: {0}", m_StaffCharacterVisible );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.Counselor )
				this.Delete();
			if ( m_StaffCharacter == null )
			{
				m_StaffCharacter = new EtherealStaffMob( this, from );
			}
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( Multis.DesignContext.Check( from ) )
			{
				EtherealSpell spell = new EtherealSpell( this, m_StaffCharacter, from );
				spell.Cast();
			}
		}

		public override void Delete()
		{
			if (m_StaffCharacter != null)
				m_StaffCharacter.Delete();
			base.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (EtherealStaffMob)m_StaffCharacter );
			writer.Write( m_StaffCharacterVisible );
			writer.Write( m_Renew );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			m_StaffCharacter = (EtherealStaffMob)reader.ReadMobile();
			m_StaffCharacterVisible = reader.ReadBool();
			m_Renew = reader.ReadBool();
		}

		private class EtherealSpell : Spell
		{
			public EtherealStaff m_Obj;
			public Mobile m_Caster;
			public Mobile m_Mobile;
			private static SpellInfo m_Info = new SpellInfo( "Ethereal Staff", "", SpellCircle.Second, 230 );

			public EtherealSpell( EtherealStaff obj, Mobile mob, Mobile caster )  : base( caster, null, m_Info )
			{
				m_Obj = obj;
				m_Caster = caster;
				m_Mobile = mob;
			}

			public override bool ClearHandsOnCast{ get{ return false; } }
			public override bool RevealOnCast{ get{ return false; } }

			public override TimeSpan GetCastRecovery()
			{
				return TimeSpan.Zero;
			}

			public override TimeSpan GetCastDelay()
			{
				return TimeSpan.FromSeconds( 1.0 );
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

			public override bool CheckDisturb( DisturbType type, bool checkFirst, bool resistable )
			{
				return false;
			}

			public override void OnDisturb( DisturbType type, bool message )
			{
				if ( message )
					Caster.SendLocalizedMessage( 1049455 ); // You have been disrupted while attempting to summon your ethereal mount!
			}

			public override void OnCast()
			{
				if ( m_Mobile == null )
				{
					m_Caster.SendMessage( "Your dude doesn't exist." );
					return;
				}
				if ( !m_Obj.m_StaffCharacterVisible )
				{
					if ( m_Obj.m_Renew )
					{
						m_Mobile.Delete();
						m_Mobile = null;
						m_Mobile = new EtherealStaffMob( m_Obj, m_Caster );
						m_Obj.m_StaffCharacter = (EtherealStaffMob)m_Mobile;
					}
					m_Obj.m_StaffCharacterVisible = true;
					m_Caster.Hidden = true;
					m_Mobile.Direction = m_Caster.Direction;
					m_Mobile.MoveToWorld( m_Caster.Location, m_Caster.Map );
					m_Mobile.Hidden = false;
				}
				else
				{
					m_Obj.m_StaffCharacterVisible = false;
					m_Mobile.Hidden = true;
					m_Mobile.MoveToWorld( new Point3D( 0, 0, 0 ), Map.Internal );
				}
				FinishSequence();
			}
		}
		public class EtherealStaffMob : Mobile
		{
			EtherealStaff m_Parent;
			public EtherealStaffMob( EtherealStaff parent, Mobile owner )
			{
				m_Parent = parent;

				Hits = HitsMax;
				Hue = owner.Hue;
				Body = owner.BodyValue;
				Name = owner.Name;
				Title = owner.Title;
				Blessed = owner.Blessed;
				NameHue = owner.NameHue;
				Hidden = true;
				for(int i = 0; i < 30; i++)
					EquipOneOfThese( owner.FindItemOnLayer( (Layer)i ) );
			}

			public void EquipOneOfThese( Item item )
			{
				if ( item == null )
					return;
				else
				{
					Item newitem = new Item();
					newitem.ItemID = item.ItemID;
					newitem.Hue = item.Hue;
					newitem.Name = item.Name;
					newitem.LootType = item.LootType;
					newitem.Layer = item.Layer;
					newitem.Movable = false;
					AddItem( newitem );
				}
			}

			public EtherealStaffMob( Serial serial ) : base( serial )
			{
			}

			public override void Delete()
			{
				if( m_Parent != null )
				{
					m_Parent.m_StaffCharacter = null;
					m_Parent.m_StaffCharacterVisible = false;
				}
				base.Delete();
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version

				writer.Write( m_Parent );

			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				m_Parent = (EtherealStaff)reader.ReadItem();
			}
		}
	}
}