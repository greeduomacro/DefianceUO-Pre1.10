using System;
using System.Reflection;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class DupeMob
	{
		public static void Initialize()
		{
			Server.Commands.Register( "DupeMob", AccessLevel.GameMaster, new CommandEventHandler( DupeMob_OnCommand ) );
		}

		[Usage( "DupeMob" )]
		[Description( "Dupes a targeted mobile." )]
		private static void DupeMob_OnCommand( CommandEventArgs e )
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
				bool done = false;
				if ( !(targ is Mobile) )
				{
					from.SendMessage( "You can only dupe mobiles." );
					return;
				}
				if ( targ is PlayerMobile )
				{
					from.SendMessage( "Duping players won't work." );
					return;
				}
				Mobile orgMob = (Mobile)targ;
				Type t = orgMob.GetType();
				ConstructorInfo[] info = t.GetConstructors();
				Mobile newMob = null;

				foreach ( ConstructorInfo c in info )
				{
					ParameterInfo[] paramInfo = c.GetParameters();

					if ( paramInfo.Length == 0 )
					{
						object[] objParams = new object[0];

						try
						{
							object o = c.Invoke( objParams );

							if ( o != null && o is Mobile )
							{
								newMob = (Mobile)o;
								CopyMobProperties( newMob, orgMob );
								for(int i = 0; i < 30; i++)
								{
									Item findItem = newMob.FindItemOnLayer( (Layer)i );
									if ( findItem != null )
										findItem.Delete();
									if ( (Layer)i == Layer.Backpack )
									{
										Backpack newBackpack = (Backpack)dupeAll( orgMob.FindItemOnLayer( (Layer)i ) );
										newMob.AddItem( newBackpack );
									}
									else
										EquipOneOfThese( newMob, orgMob.FindItemOnLayer( (Layer)i ) );
								}
								for(int i = 0; i < 52; i++)
								{
									Skill orgSkill = orgMob.Skills[i];
									Skill newSkill = newMob.Skills[i];
									newSkill.BaseFixedPoint = orgSkill.BaseFixedPoint;
								}
							}
							from.SendMessage( "Done." );
							done = true;
						}
						catch
						{
							from.SendMessage( "Error!" );
							return;
						}
					}
				}

				if ( done && newMob != null )
				{
					newMob.MoveToWorld( from.Location, from.Map );
				}
				else
				{
					from.SendMessage( "Unable to dupe. Mobile must have a 0 parameter constructor." );
				}
			}

			public void EquipOneOfThese( Mobile mobile, Item orgItem )
			{
				if ( orgItem == null )
					return;
				else
				{
					Item newItem = DupeThis( orgItem );
					if ( newItem == null )
						return;
					newItem.LootType = orgItem.LootType;
					mobile.AddItem( newItem );
				}
			}

			public Item dupeAll( Item org )
			{
				if ( org is Container )
				{
					//Backpack newCont = new Backpack();
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
						((Container)newCont).DropItem( dupeAll( i ) );
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

			private static void CopyMobProperties ( Mobile dest, Mobile src )
			{
				PropertyInfo[] props = src.GetType().GetProperties();

				for ( int i = 0; i < props.Length; i++ )
				{
					if ( props[i].CanRead && props[i].CanWrite )
						props[i].SetValue( dest, props[i].GetValue( src, null ), null );
				}
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