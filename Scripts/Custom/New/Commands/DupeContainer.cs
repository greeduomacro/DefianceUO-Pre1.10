using System;
using System.Reflection;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class DupeContainer
	{
		public static void Initialize()
		{
			Server.Commands.Register( "DupeContainer", AccessLevel.GameMaster, new CommandEventHandler( DupeContainer_OnCommand ) );
			Server.Commands.Register( "DupeBag", AccessLevel.GameMaster, new CommandEventHandler( DupeBag_OnCommand ) );
		}

		[Usage( "DupeContainer" )]
		[Description( "Dupes a targeted container." )]
		private static void DupeContainer_OnCommand( CommandEventArgs e )
		{
			int amount = 1;
			if ( e.Length >= 1 )
				amount = e.GetInt32( 0 );
			e.Mobile.Target = new DupeTarget();
			e.Mobile.SendMessage( "What do you wish to dupe?" );
		}

		[Usage( "DupeBag" )]
		[Description( "Dupes a targeted container." )]
		private static void DupeBag_OnCommand( CommandEventArgs e )
		{
			int amount = 1;
			if ( e.Length >= 1 )
				amount = e.GetInt32( 0 );
			e.Mobile.Target = new DupeTarget();
			e.Mobile.SendMessage( "What do you wish to dupe?" );
		}

		private class DupeTarget : Target
		{
			private bool m_InBag;
			private int m_Amount;

			public DupeTarget( ) : base( 15, false, TargetFlags.None )
			{

			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( !(targ is Container) )
				{
					from.SendMessage( "You can only dupe containers with this. Try [dupe or [dupemob." );
					return;
				}
				from.Backpack.DropItem( dupeAll( (Container)targ ) );
			}

			public Item dupeAll( Item org )
			{
				if ( org is Container )
				{
					Type t = org.GetType();
					Item newCont = null;
					ConstructorInfo[] info = t.GetConstructors();
					foreach ( ConstructorInfo c in info )
					{
						ParameterInfo[] paramInfo = c.GetParameters();
						if ( paramInfo.Length == 0 )
						{
							object[] objParams = new object[0];
							try
							{
								object o = c.Invoke( objParams );
								if ( o != null && o is Item )
								{
									newCont = (Item)o;
									newCont.Hue = org.Hue;
									newCont.Name = org.Name;
									newCont.LootType = org.LootType;
									newCont.Visible = org.Visible;
									newCont.Parent = null;
								}
							}
							catch
							{

							}
						}
					}
					if ( newCont == null )
						newCont = new Backpack();

					Item [] items = ((Container)org).FindItemsByType( typeof( Item ), false );
					foreach ( Item i in items )
					{
						Item dupeditem = dupeAll( i );
						if ( dupeditem != null )
							((Container)newCont).DropItem( dupeditem );
					}
					return newCont;
				}
				else
					return DupeThis( org );
			}

			public Item DupeThis( Item orgItem )
			{
				bool done = false;
				Type t = orgItem.GetType();
				Item newItem = null;

				ConstructorInfo[] info = t.GetConstructors();

				foreach ( ConstructorInfo c in info )
				{
					ParameterInfo[] paramInfo = c.GetParameters();

					if ( paramInfo.Length == 0 )
					{
						object[] objParams = new object[0];

						try
						{
							object o = c.Invoke( objParams );

							if ( o != null && o is Item )
							{
								newItem = (Item)o;
								CopyItemProperties( newItem, orgItem );
								newItem.Parent = null;
							}
							done = true;
						}
						catch
						{
							return null;
						}
					}
				}

				if ( !done )
					return null;
				else
					return newItem;
			}

			private static void CopyItemProperties ( Item dest, Item src )
			{
				PropertyInfo[] props = src.GetType().GetProperties();

				for ( int i = 0; i < props.Length; i++ )
				{
					if ( props[i].CanRead && props[i].CanWrite )
						props[i].SetValue( dest, props[i].GetValue( src, null ), null );
				}
			}
		}
	}
}