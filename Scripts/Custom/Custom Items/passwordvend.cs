using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Prompts;
using Server.Mobiles;
using Server.Targeting;
using Server.Menus;
using Server.Menus.Questions;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Items
{
	public class PasswordVend :  Item
	{
		private bool m_Active;
		private bool m_Timer;
		private bool m_Effect;
		private Type m_Type;
		private string m_Words;
		private string m_Message;
		private string m_FailMessage;
		private int m_Range;
		private int m_Sound;
		private int m_EffectID;
		private TimeSpan m_MinDelay;
		private DateTime m_End;

		[CommandProperty(AccessLevel.GameMaster )]
		public bool Active{ get {return m_Active;} set { m_Active = value; InvalidateProperties();} }

		[CommandProperty(AccessLevel.GameMaster )]
		public bool Timer{ get {return m_Timer;} set { m_Timer = value; InvalidateProperties();} }

		[CommandProperty(AccessLevel.GameMaster )]
		public string Words { get {  return m_Words;} set { m_Words = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string Message { get {  return m_Message;} set { m_Message = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string FailMessage { get {  return m_FailMessage;} set { m_FailMessage = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime End { get {  return m_End;}}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Range { get {  return m_Range;} set { m_Range = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EffectID { get {  return m_EffectID;} set { m_EffectID = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster )]
		public bool Effect{ get {return m_Effect;} set { m_Effect = value; InvalidateProperties();} }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SoundID { get {  return m_Sound;} set { m_Sound = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.Seer )]
		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get { return m_MinDelay; }
			set { m_MinDelay = value; InvalidateProperties(); }
		}






		[Constructable]
		public PasswordVend() : base( 0x13AC )
		{
			Weight = 1.0;
			Movable=false;
			Visible=false;
			m_Range=4;
			m_End=DateTime.Now;

		}
		public PasswordVend( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Effect );
			writer.Write( m_Timer);
			writer.Write( m_EffectID );
			writer.Write( m_MinDelay );
			writer.Write( m_FailMessage );
			writer.Write( m_Active );
			writer.Write( m_Words );
			writer.Write( m_Type == null ? null : m_Type.FullName );
			writer.Write( m_Message );
			writer.Write( m_Range );
			writer.Write( m_Sound );




		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Effect = reader.ReadBool();
					m_Timer = reader.ReadBool();
					m_EffectID = reader.ReadInt();
					m_MinDelay = reader.ReadTimeSpan();
					m_FailMessage = reader.ReadString();
					goto case 0;
				}
				case 0:
				{
					m_Active = reader.ReadBool();
					m_Words = reader.ReadString();

					string type = reader.ReadString();

					if ( type != null )
						m_Type = ScriptCompiler.FindTypeByFullName( type );

					m_Message = reader.ReadString();
					m_Range = reader.ReadInt();
					m_Sound = reader.ReadInt();
					break;
				}
					//m_End=DateTime.Now;
			}

		}
		public override bool HandlesOnSpeech{ get{ return true; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );
			if(m_End<=DateTime.Now&&m_Timer)
			{
				if (e.Mobile.Player&&e.Mobile.Alive&&e.Mobile.InRange( this.Location, m_Range ) )
				{
					if (e.Speech.ToLower()==m_Words )
					{
						if(m_Message!=null)
							e.Mobile.SendMessage(m_Message);
						if(m_Type!=null)
						{
							try
							{
								object o = Activator.CreateInstance(m_Type );

								if ( o is Item )
								{
									Item item = (Item)o;

									e.Mobile.AddToBackpack( item );
									InvalidateProperties();


								}
							}
							catch
							{
							}
						}

						if(m_Sound>0)
							Effects.PlaySound( this.Location, this.Map, m_Sound );
						if(m_Effect&&m_EffectID!=0)
						{
							IEntity from = new Entity( Serial.Zero, new Point3D( this.X, this.Y, this.Z ), this.Map );
							IEntity to = new Entity( Serial.Zero, new Point3D( e.Mobile.X, e.Mobile.Y, e.Mobile.Z ), e.Mobile.Map );
							Effects.SendMovingParticles( from, to, m_EffectID, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

						}
						if(m_Timer)
							m_End=(DateTime.Now+m_MinDelay);

					}
				}
			}
			else if(!m_Timer)
			{
				if (e.Mobile.Player&&e.Mobile.Alive&&e.Mobile.InRange( this.Location, m_Range ) )
				{
					if (e.Speech.ToLower()==m_Words )
					{
						if(m_Message!=null)
							e.Mobile.SendMessage(m_Message);
						if(m_Type!=null)
						{
							try
							{
								object o = Activator.CreateInstance(m_Type );

								if ( o is Item )
								{
									Item item = (Item)o;

									e.Mobile.AddToBackpack( item );
									InvalidateProperties();


								}
							}
							catch
							{
							}
						}

						if(m_Sound>0)
							Effects.PlaySound( this.Location, this.Map, m_Sound );
						if(m_Effect&&m_EffectID!=0)
						{
							IEntity from = new Entity( Serial.Zero, new Point3D( this.X, this.Y, this.Z ), this.Map );
							IEntity to = new Entity( Serial.Zero, new Point3D( e.Mobile.X, e.Mobile.Y, e.Mobile.Z ), e.Mobile.Map );
							Effects.SendMovingParticles( from, to, m_EffectID, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

						}

						m_End=(DateTime.Now+m_MinDelay);

					}
				}
			}
			else
			{
				if(m_FailMessage!=null)
					e.Mobile.SendMessage(m_FailMessage);
			}
		}
	}
}