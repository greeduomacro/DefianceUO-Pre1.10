using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Xanthos.Evo
{
	public class TenderPointsTarget : Target
	{
		private TenderEvoMeat m_Deed;

		public TenderPointsTarget( TenderEvoMeat deed ) : base( 18, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target )
		{
			if ( target is BaseEvo )
			{
				BaseEvo mob = (BaseEvo)target;

				if ( mob.Controlled && mob.ControlMaster == from )
				{
					mob.Ep += m_Deed.Points;
					m_Deed.Delete(); // Delete the evo deed
					from.SendMessage( "Your pet consumes the food and gains 100.000 EP!" );

          			if ( mob.Stage < mob.FinalStage && mob.Ep >= mob.NextEpThreshold )
          				mob.Evolve( false );
				}
			}
			else
				from.SendMessage( "You must feed an evolution creature." );
		}
	}

	public class TenderEvoMeat : Item
	{
		private int m_Points;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Points{ get{ return m_Points; } set{ m_Points = value; } }

		[Constructable]
		public TenderEvoMeat() : this( 100000 )
		{
		}

		[Constructable]
		public TenderEvoMeat( int points ) : base( 0x977 )
		{
			Weight = 7.0;
			Name = "tender evolution bacon";
			Hue = 2117;
			m_Points = points;
		}

		public TenderEvoMeat( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 );
			writer.Write( m_Points );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadEncodedInt();
			m_Points = reader.ReadInt();
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				 from.SendLocalizedMessage( 1042001 );
			else
			{
				from.SendMessage( "Select your evolution pet." );
				from.Target = new TenderPointsTarget( this );
			}
		}
	}
}