#region AuthorHeader
//
//	EvoSystem version 1.6, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
//jakob add for evo region check.
using Server.Regions;
//jakob add end for evo region check.

namespace Xanthos.Evo
{
	[CorpseName( "an evolution creature corpse" )]
	public abstract class BaseEvoMount : BaseMount, IEvoCreature
	{
		private static double kOverLimitLossChance = 0.02;	// Chance that loyalty will be lost if over followers limit

		private int m_Ep;
		private int m_Stage;
		private bool m_Pregnant;
		private bool m_HasEgg;
		private DateTime m_DeliveryDate;

		private int m_FinalStage;
		private int m_EpMinDivisor;
		private int m_EpMaxDivisor;
		private int m_DustMultiplier;
		private int m_NextEpThreshold;
		private TimeSpan m_InitialTerm;
		private bool m_ProducesYoung;
		//protected bool m_AlwaysHappy;
		protected DateTime m_NextHappyTime;

		private string m_Breed;
		private PregnancyTimer m_PregnancyTimer;

		// Implement these 3 in your subclass to return BaseEvoSpec & BaseEvoEgg subclasses & Dust Type
		public abstract BaseEvoSpec GetEvoSpec();
		public abstract BaseEvoEgg GetEvoEgg();
		public abstract Type GetEvoDustType();
		// Implement these 2 in your subclass to control where exp points are accumulated
		public abstract bool AddPointsOnDamage { get; }
		public abstract bool AddPointsOnMelee { get; }


		public int FinalStage
		{
			get { return m_FinalStage; }
		}

        public int NextEpThreshold
		{
			get { return m_NextEpThreshold; }
			set { m_NextEpThreshold = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Ep
		{
			get { return m_Ep; }
			set { m_Ep = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Stage
		{
			get { return m_Stage; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime DeliveryDate
		{
			get { return m_DeliveryDate; }
			set { m_DeliveryDate = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan RemainingTerm
		{
			get
			{
				return ( DateTime.MinValue == m_DeliveryDate ? m_InitialTerm : m_DeliveryDate.Subtract( DateTime.Now ));
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ProducesYoung
		{
			get { return m_ProducesYoung; }
			set { m_ProducesYoung = value; }
		}

		public string Breed
		{
			get
			{
				return ( null == m_Breed ? m_Breed = Xanthos.Evo.ShrinkItem.GetFormattedBreedString( this ) : m_Breed );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Pregnant
		{
			get { return m_Pregnant; }
			set
			{
				if ( m_Pregnant = Blessed = value )
				{
					m_PregnancyTimer = new PregnancyTimer( this );
					DeliveryDate = DateTime.Now + m_PregnancyTimer.Delay;

				}
				else if ( null != m_PregnancyTimer )
				{
					m_PregnancyTimer.Stop();
					m_PregnancyTimer = null;
					DeliveryDate = DateTime.MinValue;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasEgg
		{
			get { return m_HasEgg; }
			set
			{
				if ( m_HasEgg = value )
					Pregnant = false;
			}
		}

		public BaseEvoMount( string name, int bodyID, int itemID, AIType ai, double dActiveSpeed ) : base( name, bodyID, itemID, ai, FightMode.Closest, 10, 1, dActiveSpeed, 0.4 )
		{
			Name = name;
			Init();
		}

		public BaseEvoMount( string name, int bodyID, int itemID ) : base( name, bodyID, itemID, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = name;
			Init();
		}

		public BaseEvoMount( Serial serial ) : base( serial )
		{
		}

		private void Init()
		{
			BaseEvoSpec spec = GetEvoSpec();

			if ( spec != null && spec.Stages != null )
			{
				Female = Utility.RandomBool();
				m_ProducesYoung = spec.ProducesYoung;
				m_Pregnant = m_HasEgg = false;
				m_InitialTerm = TimeSpan.FromDays( spec.PregnancyTerm );
				m_FinalStage = spec.Stages.Length - 1;
				m_DeliveryDate = DateTime.MinValue;
				Tamable = spec.Tamable;
				SetFameLevel( spec.FameLevel );
				SetKarmaLevel( spec.KarmaLevel );

				if ( null != spec.Skills )
				{
					double skillTotals = 0.0;

					for ( int i = 0;  i < spec.Skills.Length; i++ )
					{
						Skills[spec.Skills[ i ]].Cap = spec.MaxSkillValues[ i ];
						skillTotals += spec.MaxSkillValues[ i ];
						SetSkill( spec.Skills[ i ], (double)(spec.MinSkillValues[ i ]), (double)(spec.MaxSkillValues[ i ]) );
					}

					if ( ( skillTotals *= 10 ) > SkillsCap )
					{
						SkillsCap = (int)skillTotals;
					}
				}

				Evolve( true );

				if ( spec.PackSpecialItemChance > Utility.RandomDouble() )
					PackSpecialItem();
			}
		}

		// Use this to place a surprise in the Evo's pack randomly on creation
		protected virtual void PackSpecialItem() {}

		public override void Damage( int amount, Mobile defender )
		{
			if ( AddPointsOnDamage )
				AddPoints( defender );

			base.Damage( amount, defender );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			if ( AddPointsOnMelee )
				AddPoints( defender );

			base.OnGaveMeleeAttack( defender );
		}

		private void AddPoints( Mobile defender )
		{
			// jakob added this if.
		        if (this.m_Stage < 3 || this.Region is DungeonRegion || this.Region is CustomRegion || this.Region is FeluccaDungeon)
		        {
                                if ( defender is BaseCreature )
        			{
        				BaseCreature bc = (BaseCreature)defender;

        				if ( bc.Controlled != true )
        					m_Ep += Utility.RandomMinMax( 5 + ( bc.HitsMax ) / m_EpMinDivisor, 5 + ( bc.HitsMax ) / m_EpMaxDivisor );
        			}

        			if ( m_Stage < m_FinalStage && m_Ep >= m_NextEpThreshold )
        			{
        				Evolve( false );
        			}
			}
		}

		public virtual void Evolve( bool hatching )
		{
			BaseEvoSpec spec = GetEvoSpec();

			if ( null != spec && null != spec.Stages )
			{
				m_Stage = hatching ? 0 : ++m_Stage;

				BaseEvoStage stage = spec.Stages[ m_Stage ];

				if ( null != stage )
				{
					int OldControlSlots = ControlSlots;

					if ( null != stage.Title )		Title			= stage.Title;
					if ( 0 != stage.BaseSoundID )	BaseSoundID		= stage.BaseSoundID;
					if ( 0 != stage.BodyValue )		Body			= stage.BodyValue;
					if ( 0 != stage.Hue )			Hue			= stage.Hue;
					if ( 0 != stage.VirtualArmor )	VirtualArmor	= stage.VirtualArmor;
					if ( 0 != stage.ControlSlots )	ControlSlots	= stage.ControlSlots;
					if ( 0 != stage.MinTameSkill )	MinTameSkill	= stage.MinTameSkill;
					if ( 0 != stage.EpMinDivisor )	m_EpMinDivisor	= stage.EpMinDivisor;
					if ( 0 != stage.EpMaxDivisor )	m_EpMaxDivisor	= stage.EpMaxDivisor;
					if ( 0 != stage.DustMultiplier )m_DustMultiplier= stage.DustMultiplier;
					if ( 0 != stage.MountID ) InternalItem.ItemID = stage.MountID;
                                        m_NextEpThreshold = stage.NextEpThreshold;

					if ( spec.AbsoluteStatValues )
					{
						SetStr( stage.StrMin, stage.StrMax );
						SetDex( stage.DexMin, stage.DexMin );
						SetInt( stage.IntMin, stage.IntMax );
						SetHits( stage.HitsMin, stage.HitsMax );
						SetDamage( stage.DamageMin, stage.DamageMax );
					}
					else
					{
						SetStr( RawStr + Utility.RandomMinMax( stage.StrMin, stage.StrMax ));
						SetDex( RawDex + Utility.RandomMinMax( stage.DexMin, stage.DexMin ));
						SetInt( RawInt + Utility.RandomMinMax( stage.IntMin, stage.IntMax ));
						SetHits( HitsMax + Utility.RandomMinMax( stage.HitsMin, stage.HitsMax ));
						SetDamage( DamageMin + stage.DamageMin, DamageMax + stage.DamageMax );
					}

					if ( null != stage.DamagesTypes )
					{
						for ( int i = 0; i < stage.DamagesTypes.Length; i++ )
							SetDamageType( stage.DamagesTypes[ i ], Utility.RandomMinMax( stage.MinDamages[ i ], stage.MaxDamages[ i ] ));
					}
					if ( null != stage.ResistanceTypes )
					{
						for ( int i = 0; i < stage.ResistanceTypes.Length; i++ )
							SetResistance( stage.ResistanceTypes[ i ], Utility.RandomMinMax( stage.MinResistances[ i ], stage.MaxResistances[ i ] ));
					}

					PlaySound( 665 );
					if ( !hatching )
					{
						Say( "*" + Name + " {0}*", stage.EvolutionMessage );
						if ( null != ControlMaster && stage.ControlSlots > 0 && ControlSlots > 0 )
						{
							ControlMaster.Followers += stage.ControlSlots - OldControlSlots;
						}
					}
					Warmode = false;
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;

			if ( null != ControlMaster && ControlMaster.Followers > ControlMaster.FollowersMax )
			{
				ControlMaster.SendMessage( Name + " is not interested in that now!" );
				return false;
			}

			if ( null != player && dropped.GetType() == GetEvoDustType() )
			{
				BaseEvoDust dust = dropped as BaseEvoDust;

				if ( null != dust )
				{
					int amount = ( dust.Amount * m_DustMultiplier );

					m_Ep += amount;
					PlaySound( 665 );
					dust.Delete();
					Say( "*"+ this.Name +" absorbs the " + dust.GetType() + " gaining " + amount + " experience points*" );
					return true;
				}
				return false;
			}
			return base.OnDragDrop( from, dropped );
		}

		private void MatingTarget_Callback( Mobile from, object obj )
		{
			BaseEvo evo = obj as BaseEvo;

			if ( null == evo )
				from.SendMessage( "That is not a pet!" );

			else if ( evo.Controlled == false )
				from.SendMessage( "That is wild." );

			else
			{
				if ( evo.Female == true )
					from.SendMessage( "That is not male!" );

				else if ( evo.Stage < m_FinalStage )
					from.SendMessage( "That male is not old enough to mate!" );

				else if ( evo.ControlMaster == from )
				{
					Pregnant = true;
				}
				else
				{
					evo.ControlMaster.SendGump( new MatingGump( from, evo.ControlMaster, this, evo ) );
					from.SendMessage( "You ask the owner of the " + evo.Breed + " if they will let your female mate with their male." );
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{


			if ( m_FinalStage - Stage <= 1 ) //( !ProducesYoung )
                        base.OnDoubleClick( from );
                        //return;

			else if ( Controlled == true && ControlMaster == from && Female == true )
			{
				if ( Stage < m_FinalStage )
					from.SendMessage( "This female is not yet old enough to mate." );

				else if ( Pregnant == true )
					from.SendMessage( "This has not yet produced an egg." );

				else if ( HasEgg == true )
				{
					HasEgg = false;
					from.AddToBackpack( GetEvoEgg() );
					from.SendMessage( "An egg has been placed in your backpack." );
				}
				else
				{
					from.SendMessage( "Target a male to mate with this female." );
					from.BeginTarget( -1, false, TargetFlags.Harmful, new TargetCallback( MatingTarget_Callback ) );
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );
			writer.Write( (int)m_Ep );
			writer.Write( (int)m_Stage );
			writer.Write( (bool)m_Pregnant );
			writer.Write( (bool)m_HasEgg );
			writer.Write( (DateTime)m_DeliveryDate );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Ep = reader.ReadInt();
					m_Stage = reader.ReadInt();
					m_Pregnant = reader.ReadBool();
					m_HasEgg = reader.ReadBool();
					m_DeliveryDate = reader.ReadDateTime();

					BaseEvoSpec spec = GetEvoSpec();

					if ( null != spec && null != spec.Stages )
					{
						BaseEvoStage stage = spec.Stages[ m_Stage ];

						if ( null != stage )
						{
							m_FinalStage	= spec.Stages.Length - 1;
							m_ProducesYoung = spec.ProducesYoung;
							m_InitialTerm	= TimeSpan.FromDays( spec.PregnancyTerm );
							m_EpMinDivisor	= stage.EpMinDivisor;
							m_EpMaxDivisor	= stage.EpMaxDivisor;
							m_DustMultiplier= stage.DustMultiplier;
							m_NextEpThreshold = stage.NextEpThreshold;
							//m_AlwaysHappy	= spec.AlwaysHappy;
						}
					}
					Pregnant = m_Pregnant;	// resets the timer if pregnant
					break;
				}
			}
		}
	}
}