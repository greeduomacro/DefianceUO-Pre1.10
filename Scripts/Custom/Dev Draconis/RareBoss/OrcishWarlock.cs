using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class OrcishWarlock : BaseRareBoss
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Orc; } }

		[Constructable]
		public OrcishWarlock () : base( AIType.AI_Mage )
		{
			Name = "an orcish warlock";
			Body = 140;
			Hue = 1109;
			BaseSoundID = 0x45A;

			SetHits( 250, 300 );

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new OrcishKinMask() );
		}

		public override bool DoLeechLife { get { return true; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy( Mobile m )
		{
			Item helm = m.FindItemOnLayer( Layer.Helm );
			if ( m.Player && helm is OrcishKinMask || helm is OrcishKinRPMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
		}

		public OrcishWarlock( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}