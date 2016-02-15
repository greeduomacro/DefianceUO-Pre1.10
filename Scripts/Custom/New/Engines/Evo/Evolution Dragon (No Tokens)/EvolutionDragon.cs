    //////////////////////////////////
   //			           //
  //      Scripted by Raelis      //
 //		             	 //
//////////////////////////////////
using System;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using Xanthos.Evo;

namespace Server.Mobiles
{
	[CorpseName( "a evolution dragon corpse" )]
	public class EvolutionDragon : BaseCreature
	{
		private Timer m_BreatheTimer;
		private DateTime m_EndBreathe;
		private Timer m_MatingTimer;
		private DateTime m_EndMating;

		public DateTime EndMating{ get{ return m_EndMating; } set{ m_EndMating = value; } }

		public int m_Stage;
		public int m_KP;
		public bool m_AllowMating;
		public bool m_HasEgg;
		public bool m_Pregnant;

		public bool m_S1;
		public bool m_S2;
		public bool m_S3;
		public bool m_S4;
		public bool m_S5;
		public bool m_S6;

		public bool S1
		{
			get{ return m_S1; }
			set{ m_S1 = value; }
		}
		public bool S2
		{
			get{ return m_S2; }
			set{ m_S2 = value; }
		}
		public bool S3
		{
			get{ return m_S3; }
			set{ m_S3 = value; }
		}
		public bool S4
		{
			get{ return m_S4; }
			set{ m_S4 = value; }
		}
		public bool S5
		{
			get{ return m_S5; }
			set{ m_S5 = value; }
		}
		public bool S6
		{
			get{ return m_S6; }
			set{ m_S6 = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowMating
		{
			get{ return m_AllowMating; }
			set{ m_AllowMating = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasEgg
		{
			get{ return m_HasEgg; }
			set{ m_HasEgg = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Pregnant
		{
			get{ return m_Pregnant; }
			set{ m_Pregnant = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int KP
		{
			get{ return m_KP; }
			set{ m_KP = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Stage
		{
			get{ return m_Stage; }
			set{ m_Stage = value; }
		}

		[Constructable]
		public EvolutionDragon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Female = Utility.RandomBool();
			Name = "a dragon hatchling";
			Body = 52;
			Hue = 1112;
			BaseSoundID = 0xDB;
			Stage = 1;

			S1 = true;
			S2 = true;
			S3 = true;
			S4 = true;
			S5 = true;
			S6 = true;

			SetStr( 66, 70 );
			SetDex( 56, 75 );
			SetInt( 76, 96 );

			SetHits( 46, 60 );

			SetDamage( 11, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15 );

			SetSkill( SkillName.Magery, 40.1, 59.0 );
			SetSkill( SkillName.Meditation, 50.1, 70.0 );
			SetSkill( SkillName.EvalInt, 40.1, 50.0 );
			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 19.3, 34.0 );
			SetSkill( SkillName.Wrestling, 19.3, 34.0 );
			SetSkill( SkillName.Anatomy, 19.3, 34.0 );

			Fame = 300;
			Karma = -300;

			VirtualArmor = 1;

			ControlSlots = 4;
		}

		public EvolutionDragon(Serial serial) : base(serial)
		{
		}

		public override bool CanBeControlledBy( Mobile m )
 		{
 		if ( m.Skills[SkillName.AnimalTaming].Base < 95 )
 		return false;
 		return base.CanBeControlledBy( m );
 		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			int kpgainmin, kpgainmax;

			if (ControlMaster != null)
			{
				if (ControlSlots < 3)
				{
					if (((ControlMaster.Followers - ControlSlots) + 3) > ControlMaster.FollowersMax)
					{
						ControlMaster = null;
						Controlled = false;
						ControlSlots = 3;
					}
					else
					{
						ControlMaster.Followers -= ControlSlots + 3;
						ControlSlots = 3;
					}
				}

				if ( (!ControlMaster.Player || ControlMaster.NetState != null) && InRange( ControlMaster.Location, 10 ) )
				{
					if ( this.Stage == 1 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 10;
								kpgainmax = 5 + ( bc.HitsMax ) / 5;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 25000 )
						{
							if ( this.S1 == true )
							{
								this.S1 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 50 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 10 );

								mindamage = this.DamageMin + ( 1 );
								maxdamage = this.DamageMax + ( 1 );

								this.Warmode = false;
								this.Say( "*"+ this.Name +" evolves*");
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = 0x27;
								this.BaseSoundID = 219;
								this.VirtualArmor = va;
								this.Stage = 2;
								this.Hue = 1112;
								this.Name = "a giant hatchling";

								this.SetDamageType( ResistanceType.Physical, 100 );
								this.SetDamageType( ResistanceType.Fire, 25 );
								this.SetDamageType( ResistanceType.Cold, 25 );
								this.SetDamageType( ResistanceType.Poison, 25 );
								this.SetDamageType( ResistanceType.Energy, 25 );

								this.SetResistance( ResistanceType.Physical, 20 );
								this.SetResistance( ResistanceType.Fire, 20 );
								this.SetResistance( ResistanceType.Cold, 20 );
								this.SetResistance( ResistanceType.Poison, 20 );
								this.SetResistance( ResistanceType.Energy, 20 );

								this.RawStr += 100;
								this.RawInt += 30;
								this.RawDex += 20;
							}
						}
					}

					else if ( this.Stage == 2 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 20;
								kpgainmax = 5 + ( bc.HitsMax ) / 10;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 175000 )
						{
							if ( this.S2 == true )
							{
								this.S2 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 2 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 10 );

								mindamage = this.DamageMin + ( 1 );
								maxdamage = this.DamageMax + ( 1 );

								this.Warmode = false;
								this.Say( "*"+ this.Name +" evolves*");
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = 0x4A;
								this.BaseSoundID = 0x5A;
								this.VirtualArmor = va;
								this.Stage = 3;
								this.Hue = 1112;
								this.Name = "a dragon imp";

								this.SetDamageType( ResistanceType.Physical, 100 );
								this.SetDamageType( ResistanceType.Fire, 25 );
								this.SetDamageType( ResistanceType.Cold, 25 );
								this.SetDamageType( ResistanceType.Poison, 25 );
								this.SetDamageType( ResistanceType.Energy, 25 );

								this.SetResistance( ResistanceType.Physical, 40 );
								this.SetResistance( ResistanceType.Fire, 40 );
								this.SetResistance( ResistanceType.Cold, 40 );
								this.SetResistance( ResistanceType.Poison, 40 );
								this.SetResistance( ResistanceType.Energy, 40 );

								this.RawStr += 100;
								this.RawInt += 20;
								this.RawDex += 10;
							}
						}
					}

					else if ( this.Stage == 3 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 30;
								kpgainmax = 5 + ( bc.HitsMax ) / 20;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 275000 )
						{
							if ( this.S3 == true )
							{
								this.S3 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 2 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 10 );

								mindamage = this.DamageMin + ( 1 );
								maxdamage = this.DamageMax + ( 1 );

								this.Warmode = false;
								this.Say( "*"+ this.Name +" evolves*");
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = Utility.RandomList( 60, 61 );
								this.BaseSoundID = 362;
								this.VirtualArmor = va;
								this.Stage = 4;
								this.Hue = 1112;
								this.Name = "a young dragon";

								this.SetResistance( ResistanceType.Physical, 60 );
								this.SetResistance( ResistanceType.Fire, 60 );
								this.SetResistance( ResistanceType.Cold, 60 );
								this.SetResistance( ResistanceType.Poison, 60 );
								this.SetResistance( ResistanceType.Energy, 60 );

								this.RawStr += 150;
								this.RawInt += 120;
								this.RawDex += 10;
							}
						}
					}

					else if ( this.Stage == 4 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 50;
								kpgainmax = 5 + ( bc.HitsMax ) / 40;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 2750000 )
						{
							if ( this.S4 == true )
							{
								this.S4 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 10 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 10 );

								mindamage = this.DamageMin + ( 5 );
								maxdamage = this.DamageMax + ( 5 );

								this.Warmode = false;
								this.Say( "*"+ this.Name +" evolves*");
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = 12;
								this.VirtualArmor = va;
								this.Stage = 5;
								this.Hue = 1112;
								this.Name = "an older dragon";

								this.SetDamageType( ResistanceType.Physical, 100 );
								this.SetDamageType( ResistanceType.Fire, 50 );
								this.SetDamageType( ResistanceType.Cold, 50 );
								this.SetDamageType( ResistanceType.Poison, 50 );
								this.SetDamageType( ResistanceType.Energy, 50 );

								this.SetResistance( ResistanceType.Physical, 80 );
								this.SetResistance( ResistanceType.Fire, 80 );
								this.SetResistance( ResistanceType.Cold, 80 );
								this.SetResistance( ResistanceType.Poison, 80 );
								this.SetResistance( ResistanceType.Energy, 80 );

								this.RawStr += 370;
								this.RawInt += 120;
								this.RawDex += 20;
							}
						}
					}

					else if ( this.Stage == 5 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 160;
								kpgainmax = 5 + ( bc.HitsMax ) / 100;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 4750000 )
						{
							if ( this.S5 == true )
							{
								this.S5 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 5 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 50 );

								mindamage = this.DamageMin + ( 5 );
								maxdamage = this.DamageMax + ( 5 );

								this.AllowMating = true;
								this.Warmode = false;
								this.Say( "*"+ this.Name +" evolves*");
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = 12;
								this.VirtualArmor = va;
								this.Stage = 6;
								this.Hue = 16385;
								this.Name = "a mature dragon";

								this.SetResistance( ResistanceType.Physical, 98 );
								this.SetResistance( ResistanceType.Fire, 98 );
								this.SetResistance( ResistanceType.Cold, 98 );
								this.SetResistance( ResistanceType.Poison, 98 );
								this.SetResistance( ResistanceType.Energy, 98 );

								this.RawStr += 20;
								this.RawInt += 120;
								this.RawDex += 20;
							}
						}
					}

					else if ( this.Stage == 6 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.HitsMax ) / 540;
								kpgainmax = 5 + ( bc.HitsMax ) / 480;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}

						if ( this.KP >= 17750000 )
						{
							if ( this.S6 == true )
							{
								this.S6 = false;
								int hits, va, mindamage, maxdamage;

								hits = ( this.HitsMax + 15 );
								//hits = ( this.HitsMax );

								va = ( this.VirtualArmor + 50 );

								mindamage = this.DamageMin + ( 15 );
								maxdamage = this.DamageMax + ( 15 );

								this.Warmode = false;
								this.Say( "*"+ this.Name +" is now an ancient dragon*");
								this.Title = "the Ancient Dragon";
								this.SetDamage( mindamage, maxdamage );
								this.SetHits( hits );
								this.BodyValue = 46;
								this.VirtualArmor = va;
								this.Stage = 7;
								this.Hue = 16385;
								this.Name = "a ancient dragon";

								this.SetDamageType( ResistanceType.Physical, 100 );
								this.SetDamageType( ResistanceType.Fire, 75 );
								this.SetDamageType( ResistanceType.Cold, 75 );
								this.SetDamageType( ResistanceType.Poison, 75 );
								this.SetDamageType( ResistanceType.Energy, 75 );

								//this.RawStr += 125;
								this.RawInt += 125;
								this.RawDex += 35;
							}
						}
					}

					else if ( this.Stage == 7 )
					{
						if ( defender is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)defender;

							if ( bc.Controlled != true )
							{
								kpgainmin = 5 + ( bc.Hits ) / 740;
								kpgainmax = 5 + ( bc.Hits ) / 660;

								this.KP += Utility.RandomList( kpgainmin, kpgainmax );
							}
						}
					}
				}
			}

			base.OnGaveMeleeAttack( defender );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;

			if ( player != null )
			{
				if ( dropped is DragonDust )
				{
					DragonDust dust = ( DragonDust )dropped;

					int amount = ( dust.Amount * 5 );

					this.PlaySound( 665 );
					this.KP += amount;
					dust.Consume();
					this.Say( "*"+ this.Name +" absorbs the dragon dust*" );

					return false;
				}
			}
			return base.OnDragDrop( from, dropped );
		}


                private void MatingTarget_Callback( Mobile from, object obj )
                {
                           	if ( obj is EvolutionDragon )
                           	{
					BaseCreature bc = (BaseCreature)obj;
					EvolutionDragon ed = (EvolutionDragon)obj;

					if ( ed.Controlled == true && ed.ControlMaster == from )
					{
						if ( ed.Female == false )
						{
							if ( ed.AllowMating == true )
							{
								this.Blessed = true;
								this.Pregnant = true;

								m_MatingTimer = new MatingTimer( this, TimeSpan.FromDays( 3.0 ) );
								m_MatingTimer.Start();

								m_EndMating = DateTime.Now + TimeSpan.FromDays( 3.0 );
							}
							else
							{
								from.SendMessage( "This male dragon is not old enough to mate!" );
							}
						}
						else
						{
							from.SendMessage( "This dragon is not male!" );
						}
					}
					else if ( ed.Controlled == true )
					{
						if ( ed.Female == false )
						{
							if ( ed.AllowMating == true )
							{
								if ( ed.ControlMaster != null )
								{
									ed.ControlMaster.SendGump( new MatingGump( from, ed.ControlMaster, this, ed ) );
									from.SendMessage( "You ask the owner of the dragon if they will let your female mate with their male." );
								}
                           					else
                           					{
                              						from.SendMessage( "This dragon is wild." );
			   					}
							}
							else
							{
								from.SendMessage( "This male dragon is not old enough to mate!" );
							}
						}
						else
						{
							from.SendMessage( "This dragon is not male!" );
						}
					}
                           		else
                           		{
                              			from.SendMessage( "This dragon is wild." );
			   		}
                           	}
                           	else
                           	{
                              		from.SendMessage( "That is not a dragon!" );
			   	}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.Controlled == true && this.ControlMaster == from )
			{
				if ( this.Female == true )
				{
					if ( this.AllowMating == true )
					{
						if ( this.Pregnant == true )
						{
							if ( this.HasEgg == true )
							{
								this.HasEgg = false;
								this.Pregnant = false;
								this.Blessed = false;
								from.AddToBackpack( new DragonEgg() );
								from.SendMessage( "A dragon's egg has been placed in your backpack" );
							}
							else
							{
								from.SendMessage( "The dragon has not yet produced an egg." );
							}
						}
						else
						{
							from.SendMessage( "Target a male dragon to mate with this female." );
                                			from.BeginTarget( -1, false, TargetFlags.Harmful, new TargetCallback( MatingTarget_Callback ) );
						}
					}
					else
					{
						from.SendMessage( "This female dragon is not old enough to mate!" );
					}
				}
			}
		}

		private DateTime m_NextBreathe;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( (combatant.Player ) )
				return;

			if ( DateTime.Now >= m_NextBreathe )
			{
				Breathe( combatant );

				m_NextBreathe = DateTime.Now + TimeSpan.FromSeconds( 12.0 + (3.0 * Utility.RandomDouble()) ); // 12-15 seconds
			}
		}

		public void Breathe( Mobile m )
		{
			DoHarmful( m );

			m_BreatheTimer = new BreatheTimer( m, this, this, TimeSpan.FromSeconds( 1.0 ) );
			m_BreatheTimer.Start();
			m_EndBreathe = DateTime.Now + TimeSpan.FromSeconds( 1.0 );

			this.Frozen = true;

			if ( this.Stage < 6 )
				this.MovingParticles( m, 0x36D4, 7, 0, false, true, ( this.Hue - 1 ), 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );
			else
				this.MovingParticles( m, 0x36D4, 7, 0, false, true, 1174, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );
		}

		private class BreatheTimer : Timer
		{
			private EvolutionDragon ed;
			private Mobile m_Mobile, m_From;

			public BreatheTimer( Mobile m, EvolutionDragon owner, Mobile from, TimeSpan duration ) : base( duration )
			{
				ed = owner;
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				int damagemin = ed.Hits / 38;
				int damagemax = ed.Hits / 28;
				ed.Frozen = false;

				m_Mobile.PlaySound( 0x11D );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( damagemin, damagemax ), 0, 100, 0, 0, 0 );
				Stop();
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)2 );
			writer.Write( m_Pregnant );
			writer.Write( m_HasEgg );
			writer.Write( m_AllowMating );
			writer.Write( m_S1 );
			writer.Write( m_S2 );
			writer.Write( m_S3 );
			writer.Write( m_S4 );
			writer.Write( m_S5 );
			writer.Write( m_S6 );
			writer.Write( (int) m_KP );
			writer.Write( (int) m_Stage );
			writer.WriteDeltaTime( m_EndBreathe );
			writer.WriteDeltaTime( m_EndMating );
			//writer.WriteDeltaTime( m_EndPetLoyalty );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					m_Pregnant = reader.ReadBool();
					m_HasEgg = reader.ReadBool();
					m_AllowMating = reader.ReadBool();
					m_S1 = reader.ReadBool();
					m_S2 = reader.ReadBool();
					m_S3 = reader.ReadBool();
					m_S4 = reader.ReadBool();
					m_S5 = reader.ReadBool();
					m_S6 = reader.ReadBool();
					m_KP = reader.ReadInt();
					m_Stage = reader.ReadInt();

					m_EndBreathe = reader.ReadDeltaTime();
					if ( m_EndBreathe > DateTime.Now )
					{
						m_BreatheTimer = new BreatheTimer( this, this, this, m_EndBreathe - DateTime.Now );
						m_BreatheTimer.Start();
					}

					m_EndMating = reader.ReadDeltaTime();
					if ( m_EndMating > DateTime.Now )
					{
						m_MatingTimer = new MatingTimer( this, m_EndMating - DateTime.Now );
						m_MatingTimer.Start();
					}

					break;
				}
				case 1:
				{
					m_Pregnant = reader.ReadBool();
					m_HasEgg = reader.ReadBool();
					m_AllowMating = reader.ReadBool();
					m_S1 = reader.ReadBool();
					m_S2 = reader.ReadBool();
					m_S3 = reader.ReadBool();
					m_S4 = reader.ReadBool();
					m_S5 = reader.ReadBool();
					m_S6 = reader.ReadBool();
					m_KP = reader.ReadInt();
					m_Stage = reader.ReadInt();

					m_EndBreathe = reader.ReadDeltaTime();
					m_BreatheTimer = new BreatheTimer( this, this, this, m_EndBreathe - DateTime.Now );
					m_BreatheTimer.Start();

					m_EndMating = reader.ReadDeltaTime();
					m_MatingTimer = new MatingTimer( this, m_EndMating - DateTime.Now );
					m_MatingTimer.Start();

					/*m_EndPetLoyalty =*/ reader.ReadDeltaTime();
					//m_PetLoyaltyTimer = new PetLoyaltyTimer( this, m_EndPetLoyalty - DateTime.Now );
					//m_PetLoyaltyTimer.Start();

					break;
				}
				case 0:
				{
					TimeSpan durationbreathe = TimeSpan.FromSeconds( 1.0 );
					TimeSpan durationmating = TimeSpan.FromDays( 3.0 );
					TimeSpan durationloyalty = TimeSpan.FromSeconds( 5.0 );

					m_MatingTimer = new MatingTimer( this, durationmating );
					m_MatingTimer.Start();
					m_EndMating = DateTime.Now + durationmating;

					m_BreatheTimer = new BreatheTimer( this, this, this, durationbreathe );
					m_BreatheTimer.Start();
					m_EndBreathe = DateTime.Now + durationbreathe;
					break;
				}
			}
		}
	}

	public class MatingTimer : Timer
	{
		private EvolutionDragon ed;

		public MatingTimer( EvolutionDragon owner, TimeSpan duration ) : base( duration )
		{
			Priority = TimerPriority.OneSecond;
			ed = owner;
		}

		protected override void OnTick()
		{
			ed.Blessed = false;
			ed.HasEgg = true;
			ed.Pregnant = false;
			Stop();
		}
	}

	public class MatingGump : Gump
	{
		private Mobile m_From;
		private Mobile m_Mobile;
		private EvolutionDragon m_ED1;
		private EvolutionDragon m_ED2;

		public MatingGump( Mobile from, Mobile mobile, EvolutionDragon ed1, EvolutionDragon ed2 ) : base( 25, 50 )
		{
			Closable = false;
			Dragable = false;
			mobile.Frozen = true;

			m_From = from;
			m_Mobile = mobile;
			m_ED1 = ed1;
			m_ED2 = ed2;

			AddPage( 0 );

			AddBackground( 25, 10, 420, 200, 5054 );

			AddImageTiled( 33, 20, 401, 181, 2624 );
			AddAlphaRegion( 33, 20, 401, 181 );

			AddLabel( 125, 148, 1152, m_From.Name +" would like to mate "+ m_ED1.Name +" with" );
			AddLabel( 125, 158, 1152, m_ED2.Name +"." );

			AddButton( 100, 50, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 130, 50, 1152, "Allow them to mate." );
			AddButton( 100, 75, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddLabel( 130, 75, 1152, "Do not allow them to mate." );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			if ( from == null )
				return;

			if ( info.ButtonID == 0 )
			{
				m_From.SendMessage( m_Mobile.Name +" declines your request to mate the two dragons." );
				m_Mobile.SendMessage( "You decline "+ m_From.Name +"'s request to mate the two dragons." );
			}
			if ( info.ButtonID == 1 )
			{
				m_ED1.Blessed = true;
				m_ED1.Pregnant = true;

				MatingTimer mt = new MatingTimer( m_ED1, TimeSpan.FromDays( 3.0 ) );
				mt.Start();
				m_ED1.EndMating = DateTime.Now + TimeSpan.FromDays( 3.0 );

				m_From.SendMessage( m_Mobile.Name +" accepts your request to mate the two dragons." );
				m_Mobile.SendMessage( "You accept "+ m_From.Name +"'s request to mate the two dragons." );
			}
		}
	}
}