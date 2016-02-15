/* Special release for paid support area only.
 * Please do not distribute, including upload to RunUO.com.
 * Thank you.
 * Originally created by Dian, modified by Tejster24
 */
using System;
using Server.Network;
using Server;
using System.Collections;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Spells.IceStaff;
using Server.Targeting;

namespace Server.Items
{
	public enum StaffEffect
	{
		Freeze,
		IceStrike,
		IceBall
	}

	[FlipableAttribute( 0xDF1, 0xDF0 )]
	public class IceStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override int AosSpeed{ get{ return 41; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 3; } }
		public override int OldMaxDamage{ get{ return 12; } }
		public override int OldSpeed{ get{ return 30; } }

		private bool m_CanUseIceStrike;
		private bool m_CanUseFreeze;
		private bool m_CanUseIceBall;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseIceStrike
		{
			get { return m_CanUseIceStrike; }
			set { m_CanUseIceStrike = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseFreeze
		{
			get { return m_CanUseFreeze; }
			set { m_CanUseFreeze = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseIceBall
		{
			get { return m_CanUseIceBall; }
			set { m_CanUseIceBall = value; }
		}

		private DateTime PreviousUse;
		private StaffEffect m_StaffEffect;
		private int m_Charges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get
			{
				return m_Charges;
			}
			set
			{
				m_Charges = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public StaffEffect Effect
		{
			get
			{
				return m_StaffEffect;
			}
			set
			{
				m_StaffEffect = value;
			}
		}

		[Constructable]
		public IceStaff() : base(  0xDF1 )
		{
			int firsteffect = Utility.Random ( 1, 3 );
			int secondeffect = Utility.Random ( 1, 3 );
			while ( firsteffect == secondeffect )
				secondeffect = Utility.Random ( 1, 3 );
			switch ( firsteffect )
			{
				case 1: m_CanUseIceStrike = true; break;
				case 2: m_CanUseFreeze = true; break;
				case 3: m_CanUseIceBall = true; break;
			}
			switch ( secondeffect )
			{
				case 1: m_CanUseIceStrike = true; break;
				case 2: m_CanUseFreeze = true; break;
				case 3: m_CanUseIceBall = true; break;
			}
			switch ( firsteffect )
			{
				case 1: m_StaffEffect = StaffEffect.IceStrike; break;
				case 2: m_StaffEffect = StaffEffect.Freeze; break;
				case 3: m_StaffEffect = StaffEffect.IceBall; break;
			}
			this.m_Charges = Utility.Random( 12, 40 );
			Hue = 0x480;
			Weight = 3.0;
			// this.M = 1017413;
		}

		public IceStaff( Serial serial ) : base( serial )
		{
		}

		public override bool HandlesOnSpeech{ get{ return true; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile m = e.Mobile;

			if ((Parent == m ) && (m.FindItemOnLayer( Layer.TwoHanded ) == this ))
			{
				if (e.Speech.ToLower() == "an ex del")
				{
					if ( m_CanUseFreeze )
					{
						m.PlaySound( 0xF6 );
						m_StaffEffect = StaffEffect.Freeze;
					}
				}

				if (e.Speech.ToLower() == "in corp del")
				{
					if ( m_CanUseIceStrike )
					{
						m.PlaySound( 0xF7 );
						m_StaffEffect = StaffEffect.IceStrike;
					}
				}

				if (e.Speech.ToLower() == "des corp del")
				{
					if ( m_CanUseIceBall )
					{
						m.PlaySound( 0xF8 );
						m_StaffEffect = StaffEffect.IceBall;
					}
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
				base.GetProperties( list );

				if ( m_Charges != 0 )
					list.Add( 1060584, m_Charges.ToString() );
		}

		public override void OnSingleClick( Mobile from )
		{
			ArrayList attrs = new ArrayList();

			if ( !Identified )
				attrs.Add( new EquipInfoAttribute( 1038000 ) ); // Unidentified
			else
				attrs.Add( new EquipInfoAttribute( 1041367, m_Charges ) );

			EquipmentInfo eqInfo = new EquipmentInfo( 1017413, Crafter, false, (EquipInfoAttribute[])attrs.ToArray( typeof( EquipInfoAttribute ) ) );
			from.Send( new DisplayEquipmentInfo( this, eqInfo ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.TwoHanded ) == this )
			{
				if ( DateTime.Now >= PreviousUse + TimeSpan.FromSeconds(30) )
				{
					PreviousUse = DateTime.Now;
					if ( m_Charges == 0 )
					{
						from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
						return;
					}

					else
					{
						--m_Charges;
					}
						switch ( m_StaffEffect )
					{
						case StaffEffect.Freeze: new FreezeSpell( from, null ).Cast(); break;
						case StaffEffect.IceStrike: new IceStrikeSpell( from, null ).Cast(); break;
						case StaffEffect.IceBall: new IceBallSpell( from, null ).Cast(); break;
					}
				}
				else
				{
					from.SendLocalizedMessage( 1062012 );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
			}
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			string spellcast = from.Spell.GetType().ToString();
			/*switch (spellcast)
			{
				case "Server.Spells.IceStaff.FreezeSpell": return true; break;
				case "Server.Spells.IceStaff.IceStrikeSpell": return true; break;
				case "Server.Spells.IceStaff.IceBallSpell": return true; break;
			}*/
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_CanUseIceStrike );
			writer.Write( m_CanUseFreeze );
			writer.Write( m_CanUseIceBall );

			writer.Write( (int) m_StaffEffect );
			writer.Write( (int) m_Charges );

		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 1 )
			{
				m_CanUseIceStrike = reader.ReadBool();
				m_CanUseFreeze = reader.ReadBool();
				m_CanUseIceBall = reader.ReadBool();
			}
			m_StaffEffect = (StaffEffect)reader.ReadInt();
			m_Charges = (int)reader.ReadInt();
		}
	}
}

namespace Server.Spells.IceStaff
{
	public class FreezeSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Freeze", "",/*"An Ex Del", */
			SpellCircle.First,
			218
			);

		public FreezeSpell( Mobile caster, Server.Items.SpellScroll scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds( 0.0 );
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Caster.HarmfulCheck( m ) && CheckSequence() )
			{
				Mobile source = Caster;
				SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );
				double duration = 7.0;

				m.Paralyze( TimeSpan.FromSeconds( duration ) );
				Caster.Mana = 0;

				if ( Caster != m )
				{
					Caster.Direction = Caster.GetDirectionTo( m );
				}

				m.PlaySound( 0x204 );
				Effects.SendTargetEffect( m, 0x376A, 16 );
				Caster.SendLocalizedMessage( 1008127 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private FreezeSpell m_Owner;

			public InternalTarget( FreezeSpell owner ) : base( 12, true, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}

	public class IceBallSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"IceBall", "",/*"Des Corp Del", */
			SpellCircle.First,
			203
			);

		public IceBallSpell( Mobile caster, Server.Items.SpellScroll scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds( 0.0 );
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( Caster.HarmfulCheck( m ) && CheckSequence() )
			{
				Mobile source = Caster;
				SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );
				double damage = Utility.Random( 12, 15 );

				damage *= GetDamageScalar( m );

				if ( Caster != m )
					Caster.Direction = Caster.GetDirectionTo( m );

				Caster.PlaySound( 0x15E );

				Effects.SendMovingEffect( Caster, m, 0x36D4, 7, 0, false, true ,0x47e,3);
				Caster.PlaySound( 0x440 );

				m.Damage( (int) damage );
				Caster.SendLocalizedMessage( 1008127 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private IceBallSpell m_Owner;

			public InternalTarget( IceBallSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}

	public class IceStrikeSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Ice Strike", "",/* "In Corp Del", */
			SpellCircle.First
			);

		public IceStrikeSpell( Mobile caster, Server.Items.SpellScroll scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds( 0.0 );
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( Caster.HarmfulCheck( m ) && CheckSequence() )
			{
				Mobile source = Caster;
				SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );
				double damage =  Caster.Mana / 2;
				Caster.Mana = 0;

				if ( Caster != m )
					Caster.Direction = Caster.GetDirectionTo( m );

				Caster.PlaySound( 0x208 );
				Effects.SendTargetEffect( m, 0x3709, 32,0x47E,3 );

				m.Damage( (int) damage );
				Caster.SendLocalizedMessage( 1008127 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private IceStrikeSpell m_Owner;

			public InternalTarget( IceStrikeSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}