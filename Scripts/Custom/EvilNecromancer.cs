using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Misc;
using Server.Regions;
using Server.SkillHandlers;



namespace Server.Mobiles
{
	[CorpseName( "an evil necromancer corpse" )]
	public class EvilNecromancer : BaseCreature
	{
		[Constructable]
		public EvilNecromancer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Necromatic Guard";
			Kills = 5;
			Body = 400;
			BaseSoundID = 0x45A;
			Hue = 33;
			SetStr( 116, 150 );
			SetDex( 91, 115 );
			SetInt( 80, 100 );
			SetHits( 50, 80 );
			SetDamage( 4, 14 );

			SetDamageType( ResistanceType.Physical, 20 );

			SetResistance( ResistanceType.Physical, 28, 47 );
			SetResistance( ResistanceType.Fire, 38, 57 );
			SetResistance( ResistanceType.Cold, 38, 57 );
			SetResistance( ResistanceType.Poison, 38, 57 );
			SetResistance( ResistanceType.Energy, 38, 57 );

			SetSkill( SkillName.EvalInt, 85.1, 89.5 );
			SetSkill( SkillName.Magery, 75.1, 80.5 );
			SetSkill( SkillName.MagicResist, 90.1, 95.0 );
			SetSkill( SkillName.Tactics, 65.1, 95.0 );
			SetSkill( SkillName.Wrestling, 85.1, 105.0 );
			SetSkill( SkillName.Meditation, 85.1, 110.0 );

			Fame = 7000;
			Karma = -4000;

			VirtualArmor = 40;

			PackGem();
			PackGem();

			PackReg( 3 );
			PackItem( new Arrow( 10 ) );
			PackGold( 300, 500 );
			PackScroll( 1, 4 );

			AddItem( new Server.Items.Boots( Utility.RandomRedHue() ) );
			AddItem( new Server.Items.Robe( Utility.RandomRedHue() ) );
		}





		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override int Meat{ get{ return 1; } }


		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is NecromaticMask )
			{
				AOS.Damage( aggressor, 30, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x365 );
			}
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( m.Player && m.FindItemOnLayer( Layer.Helm ) is NecromaticMask )
				return false;

			return base.IsEnemy( m );
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return true;
		}


/////
      private GHCastTimer m_CastTimer;
      private GreaterHealSpell m_ghs;

      public override void OnSpeech( SpeechEventArgs e )
      {
         base.OnSpeech(e);
         Mobile from = e.Mobile;

//if( e.Layer.Helm is NecromaticMask )
	if ( from.FindItemOnLayer( Layer.Helm ) is NecromaticMask )
         {
            if (e.Speech.IndexOf( "nwapslleh" ) >= 0  )
		//if (e.Speech.ToLower().IndexOf( "nwapslleh" ) >= 0  )
            {
              // if ( e.Hits < (e.HitsMax - 50) && from.Poisoned == false )
              //{
                  m_ghs = new GreaterHealSpell( this, null ); // get your spell
                  if( m_ghs.Cast() ) // if it casts the spell
                  {
                     TimeSpan castDelay = m_ghs.GetCastDelay();
                     m_CastTimer = new GHCastTimer( m_ghs, from, castDelay );
                     m_CastTimer.Start();
                  }
               }
            }
         //}
      }

      private class GHCastTimer : Timer
      {
         private GreaterHealSpell m_Spell;
         private Mobile m_Target;

         public GHCastTimer( GreaterHealSpell spell, Mobile target, TimeSpan castDelay ) : base( castDelay )
         {
            m_Spell = spell;
            m_Target = target;
            Priority = TimerPriority.TwentyFiveMS;
         }

         protected override void OnTick()
         {
            m_Spell.Target( m_Target );
         }
      }

//////
		public EvilNecromancer( Serial serial ) : base( serial )
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