using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	public class AGhost : BaseCreature
	{
		public override bool DeleteCorpseOnDeath{ get{ return true; } }
		private DateTime m_ExpireTime;

		[Constructable]
		public AGhost() : base( AIType.AI_Melee, FightMode.Agressor, 10, 1, 0.4, 0.8 )
		{
			switch ( Utility.Random( 2 ) )
			{
				case 0: Name = "a tormented spirit"; break;
				case 1: Name = "a tortured soul"; break;
			}
			Body = 970;
			Hue = 22222;
			Blessed = true;

			SetStr( 20 );
			SetDex( 20 );
			SetInt( 20 );

			SetHits( 50 );

			SetDamage( 1, 10 );

			HoodedShroudOfShadows robe = new HoodedShroudOfShadows();
			robe.Hue = 22222;
			robe.Name = "";
			robe.Movable = false;
			robe.LootType = LootType.Blessed;
			AddItem( robe );

			m_ExpireTime = DateTime.Now + TimeSpan.FromMinutes( Utility.RandomMinMax( 10, 15 ));

			VirtualArmor = 30;
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_ExpireTime )
			{
				Blessed = false;
				this.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
				PlaySound(Utility.RandomList(0x305, 0x306, 0x307, 0x308, 0x309));
				Spells.SpellHelper.Damage(TimeSpan.FromSeconds(1), this, 100);
			}
			base.OnThink();
		}

		public AGhost( Serial serial ) : base( serial )
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