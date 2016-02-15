using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Xanthos.Evo
{
	public class FreshPointsTarget : Target
	{
		private FreshEvoCarrot m_Deed;

		public FreshPointsTarget( FreshEvoCarrot deed ) : base( 18, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target )
		{
			if ( target is BaseEvoMount )

			{
				BaseEvoMount mob = (BaseEvoMount)target;

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
				from.SendMessage( "You must feed an evolution mount." );
		}
	}

	public class FreshEvoCarrot : Item
	{
		private int m_Points;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Points{ get{ return m_Points; } set{ m_Points = value; } }

		[Constructable]
		public FreshEvoCarrot() : this( 100000 )
		{
		}

		[Constructable]
		public FreshEvoCarrot( int points ) : base( 0xC77 )
		{
			Weight = 7.0;
			Hue = 2211;
			Name = "fresh evolution carrot";
			m_Points = points;
		}

		public FreshEvoCarrot( Serial serial ) : base( serial )
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
				from.Target = new FreshPointsTarget( this );
			}
		}
	}
}