using System;
using System.Collections;
using Server.Multis;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseMega : Item
	{
		public abstract int Bonus{ get; }
		public virtual StatType Type{ get{ return StatType.Str; } }

		public BaseMega( int hue ) : base( 0xF0C )
		{
			Movable = false;
			Weight = 500;
			Hue = hue;
		}

		public BaseMega( Serial serial ) : base( serial )
		{
		}

		public virtual bool Apply( Mobile from )
		{
			if (!from.CanBeginAction( typeof( BaseMega ) ) )
			{
				from.SendLocalizedMessage( 502173 ); // You are already under a similar effect.
				return false;
			}

			bool applied = Spells.SpellHelper.AddStatOffset( from, Type, Bonus, TimeSpan.FromSeconds( 30 ) );

			if ( applied )
				from.BeginAction( typeof( BaseMega ) );

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
				from.SendLocalizedMessage( 500446 );
			else if ( Apply( from ) )
			{
				from.FixedEffect( 0x3728, 10, 15 );
				from.PlaySound( 1002 );

				from.SendMessage(1276, "You feel AMAZING!");
				new PowerTimer( from, this ).Start();

				from.Emote( String.Format( "* {0} FLARES IN POWER *" , from.Name ) );
				Delete();
			}
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

		public class PowerTimer : Timer
		{
			private Mobile m_Mobile;
			private BaseMega m_This;
			private int m_Count = 5, m_OldHue;
			private int m_Hue = 1288;

			public PowerTimer( Mobile m, BaseMega mega ) : base( TimeSpan.FromMinutes( 0.30 ), TimeSpan.FromMinutes( 0.30 ) )
			{
				m_This = mega;
				m_Mobile = m;
				m_OldHue = m.Hue;
				m.Hue = m_Hue;
				m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
			}

			protected override void OnTick()
			{
				m_Mobile.Hue = m_Hue;
				m_Count--;
				m_Mobile.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );

				if ( m_Count <= 0 )
				{
					m_Mobile.Hue = m_OldHue;
					m_Mobile.SendMessage(1276, "Your powers are wearing off!");
					m_Mobile.EndAction( typeof( BaseMega ) );
					Stop();
				}
			}
		}
	}

	public class MegaInt : BaseMega
	{
		public override int Bonus{ get{ return 25; } }
		public override StatType Type{ get{ return StatType.Int; } }

		public override int LabelNumber{ get{ return 1041073; } } // prized fish

		[Constructable]
		public MegaInt() : base( 4 )
		{
			Name = "MEGA INT BOOSTER";
		}


		public MegaInt( Serial serial ) : base( serial )
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

			if ( Hue == 4 )
				Hue = 4;
		}
	}

	public class MegaDex : BaseMega
	{
		public override int Bonus{ get{ return 25; } }
		public override StatType Type{ get{ return StatType.Dex; } }

		public override int LabelNumber{ get{ return 1041074; } } // wondrous fish

		[Constructable]
		public MegaDex() : base( 74 )
		{
			Name = "MEGA DEX BOOSTER";
		}

		public MegaDex( Serial serial ) : base( serial )
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

			if ( Hue == 74 )
				Hue = 74;
		}
	}

	public class MegaStr : BaseMega
	{
		public override int Bonus{ get{ return 25; } }
		public override StatType Type{ get{ return StatType.Str; } }

		public override int LabelNumber{ get{ return 1041075; } } // truly rare fish

		[Constructable]
		public MegaStr() : base( 76 )
 		{
                           Name = "MEGA STR BOOSTER";
                }

		public MegaStr( Serial serial ) : base( serial )
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

			if ( Hue == 376 )
				Hue = 76;
		}
	}
}