using System;
using System.Collections;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Regions;

namespace Server.Items
{
	[DispellableFieldAttribute]
	public class TollMoongate : Moongate
	{
		private object m_AutomaticToll;

		private Type m_TollItem = typeof(NecroCrystal);
		private int m_TollAmount = 1;

		[CommandProperty(AccessLevel.GameMaster)]
		public object AutomaticToll
		{
			get{ return "NecroCrystal"; }
			set{}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Type TollItem
		{
			get { return m_TollItem; }
			set { m_TollItem = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int TollAmount
		{
			get { return m_TollAmount; }
			set { m_TollAmount = value; }
		}

		[Constructable]
		public TollMoongate() : this( Point3D.Zero, null )
		{
		}

		public TollMoongate( Point3D target, Map targetMap ) : base( target, targetMap )
		{
			Name = "Evil Moongate";
			Dispellable = false;
			Movable = false;
			Light = LightType.Circle300;
		}

		public TollMoongate( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add("Toll Moongate");
		}

		public override void OnSingleClick(Mobile from)
		{
			string tolabel = String.Format("{0}\nToll Type: {1}\nToll Amount: {2}",Name,m_TollItem.Name,m_TollAmount);
			LabelTo( from, tolabel );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add(1060658,"{0}\t{1}","Toll Type",m_TollItem);
			list.Add(1060659,"{0}\t{1}","Toll Amount",m_TollAmount);
		}

		public void AutoAssignToll(Mobile from)
		{
			if (IsActive())
			{
				m_TollItem = typeof(Gold);

				int x = Target.X - Location.X;
				int y = Target.Y - Location.Y;
				int hyp = (int)(Math.Sqrt((x*x)+(y*y)));
				m_TollAmount = hyp;
			} else
				from.SendMessage("This moongate has not been initialized.");
		}

		public bool IsActive()
		{
			return (TargetMap != null && TargetMap != Map.Internal && Target != new Point3D(0,0,0));
		}

		public override void UseGate( Mobile m )
		{
			/*if ( m.Kills >= 5 && TargetMap != Map.Felucca )
			{
				m.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else*/
			if ( m.Spell != null )
				m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
			else if ( IsActive() )
			{
				// add toll here
				if (m_TollItem == null)
					m.SendMessage("This moongate is not active.");
				else if (m.Backpack.ConsumeTotal(m_TollItem,m_TollAmount,true))
				{
					BaseCreature.TeleportPets( m, Target, TargetMap);

					m.Map = TargetMap;
					m.Location = Target;

					m.PlaySound( 0x1FE );
				}
				else
					m.SendMessage("You lack the required toll in your bag to travel forth.");
			}
			else
				m.SendMessage( "This moongate does not seem to go anywhere." );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_TollItem.ToString() );
			writer.Write( m_TollAmount );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_TollItem = ScriptCompiler.FindTypeByFullName(reader.ReadString());
			m_TollAmount = reader.ReadInt();
		}
	}
}