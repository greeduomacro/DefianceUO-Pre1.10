using Server;
using Server.Items;
using Server.Network;
using System;
using System.Collections;

namespace Server.Items
{
	public abstract class BaseTreasureChest : LockableContainer
	{
		private TreasureLevel m_TreasureLevel;
		private short m_MaxSpawnTime = 60;
		private short m_MinSpawnTime = 45;
		private TreasureResetTimer m_ResetTimer;
		private bool m_IsTrapable;
		private int m_TrapChance = 0; //As a percent of 100

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsTrapable{ get{ return m_IsTrapable; } set{ m_IsTrapable = value; SetTrap(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapChance{ get{ return m_TrapChance; } set{ m_TrapChance = Math.Max( Math.Min( value, 100 ), 0 ); SetTrap(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TreasureLevel Level
		{
			get
			{
				return m_TreasureLevel;
			}
			set
			{
				m_TreasureLevel = value;
				ClearContents();
				SetLockLevel();
				SetTrap();
				GenerateTreasure();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public short MaxSpawnTime
		{
			get
			{
				return m_MaxSpawnTime;
			}
			set
			{
				m_MaxSpawnTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public short MinSpawnTime
		{
			get
			{
				return m_MinSpawnTime;
			}
			set
			{
				m_MinSpawnTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Locked
		{
			get { return base.Locked; }
			set
			{
				if ( base.Locked != value )
				{
					base.Locked = value;

					if ( !value )
						StartResetTimer();
				}
			}
		}

		public override bool CanStore( Mobile m )
		{
			return true;
		}

//		public override bool IsDecoContainer{ get{ return false; } }
		public override bool TrapOnOpen{ get{ return true; } }

		public BaseTreasureChest( int itemID ) : this( itemID, TreasureLevel.Level2 )
		{
		}

		public BaseTreasureChest( int itemID, TreasureLevel level ) : base( itemID )
		{
			m_TreasureLevel = level;
			Locked = true;
			Movable = false;

			SetLockedName();
			SetLockLevel();
			SetTrap();
			GenerateTreasure();
		}

		public BaseTreasureChest( Serial serial ) : base( serial )
		{
		}

		protected virtual void SetLockedName()
		{
			Name = "a locked treasure chest";
		}

		protected virtual void SetUnlockedName()
		{
			Name = "a treasure chest";
		}

		public override void LockPick( Mobile from )
		{
			SetUnlockedName();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			//Version 1
			writer.Write( m_IsTrapable );
			writer.Write( m_TrapChance );

			//Version 0
			writer.Write( (byte) m_TreasureLevel );
			writer.Write( m_MinSpawnTime );
			writer.Write( m_MaxSpawnTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_IsTrapable = reader.ReadBool();
					m_TrapChance = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_TreasureLevel = (TreasureLevel)reader.ReadByte();
					m_MinSpawnTime = reader.ReadShort();
					m_MaxSpawnTime = reader.ReadShort();
					break;
				}
			}

			SetLockLevel();

			if( !Locked )
				StartResetTimer();
		}

		protected virtual void SetLockLevel()
		{
			switch( m_TreasureLevel )
			{
				case TreasureLevel.Level1:
					this.RequiredSkill = 5;
					break;

				case TreasureLevel.Level2:
					this.RequiredSkill = 20;
					break;

				case TreasureLevel.Level3:
					this.RequiredSkill = 50;
					break;

				case TreasureLevel.Level4:
					this.RequiredSkill = 70;
					break;

				case TreasureLevel.Level5:
					this.RequiredSkill = 90;
					break;
				default:
					this.RequiredSkill = 100;
					break;
			}

			Locked = true;
			LockLevel = RequiredSkill - 10;
			MaxLockLevel = RequiredSkill + 40;
			if ( m_TreasureLevel > TreasureLevel.Level5 )
				MaxLockLevel = RequiredSkill + (((int)m_TreasureLevel-4)*50);
		}

		protected virtual void SetTrap()
		{
			if ( m_IsTrapable && m_TrapChance > Utility.Random( 100 ) )
			{
				int level = (int)m_TreasureLevel;

				if ( level > Utility.Random( 8 ) )
				{
					TrapType = TrapType.PoisonTrap;
					TrapLevel = ((level+1)/2)+1;
				}
				else
				{
					TrapType = Utility.RandomBool() ? TrapType.ExplosionTrap : TrapType.MagicTrap;
					TrapLevel = 0;
				}

				TrapPower = (level+1) * Utility.RandomMinMax( 5, 10 );

				TrapOnLockpick = true;
			}
			else
			{
				TrapType = TrapType.None;
				TrapPower = 0;
				TrapLevel = 0;
				TrapOnLockpick = false;
			}
		}

		private void StartResetTimer()
		{
			if( m_ResetTimer == null )
				m_ResetTimer = new TreasureResetTimer( this );
			else
				m_ResetTimer.Delay = TimeSpan.FromMinutes( Utility.Random( m_MinSpawnTime, m_MaxSpawnTime ));

			m_ResetTimer.Start();
		}

		protected virtual void GenerateTreasure()
		{
			int MinGold = 1;
			int MaxGold = 2;
			bool trapped = TrapLevel > 0;

			switch( m_TreasureLevel )
			{
				case TreasureLevel.Level1:
					MinGold = 100;
					MaxGold = 300;
					break;

				case TreasureLevel.Level2:
					MinGold = 300;
					MaxGold = 600;
					break;

				case TreasureLevel.Level3:
					MinGold = 600;
					MaxGold = 900;
					break;

				case TreasureLevel.Level4:
					MinGold = 900;
					MaxGold = 1200;
					break;

				case TreasureLevel.Level5:
					MinGold = 1200;
					MaxGold = 5000;
					break;
				case TreasureLevel.Level6:
					MinGold = 5000;
					MaxGold = 9000;
					break;
			}
			DropItem( new Gold( MinGold, MaxGold ) );
		}

		public void ClearContents()
		{
			for ( int i = Items.Count - 1; i >= 0; --i )
				((Item)Items[i]).Delete();
		}

		public void Reset()
		{
			if( m_ResetTimer != null )
			{
				if( m_ResetTimer.Running )
					m_ResetTimer.Stop();
			}

			Locked = true;
			SetTrap();
			ClearContents();
			GenerateTreasure();
		}

		public enum TreasureLevel
		{
			Level1,
			Level2,
			Level3,
			Level4,
			Level5,
			Level6,
			Level7,
			Level8
		};

		private class TreasureResetTimer : Timer
		{
			private BaseTreasureChest m_Chest;

			public TreasureResetTimer( BaseTreasureChest chest ) : base ( TimeSpan.FromMinutes( Utility.Random( chest.MinSpawnTime, chest.MaxSpawnTime ) ) )
			{
				m_Chest = chest;
				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				m_Chest.Reset();
			}
		}
	}
}