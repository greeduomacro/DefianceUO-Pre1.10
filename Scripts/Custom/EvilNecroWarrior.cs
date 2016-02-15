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
	public class EvilNecroWarrior : BaseCreature
	{
		[Constructable]
		public EvilNecroWarrior() : base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.08, 0.2 )
		{
			InitStats( 110, 110, 110 );
			Name = "Necromatic Warrior";
			SpeechHue = Utility.RandomDyedHue();
			Kills = 5;
			RangePerception = 40;
			Body = 400;
			Hits = 850;
			Dex = 100;
			SetDamage( 25, 45 );
			VirtualArmor = 65;
			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
			}
			else
			{
				Body = 0x190;

			}

			new SkeletalMount().Rider = this;

			Halberd halberd = new Halberd();
			halberd.Name = "Halberd of the Fallen Souls";
			halberd.Movable = false;
			halberd.Hue = 33;
			halberd.LootType = LootType.Blessed;
			AddItem( halberd );

			PackGold( 450, 550 );

			Skills[SkillName.Anatomy].Base = 130.0;
			Skills[SkillName.Tactics].Base = 100.0;
			Skills[SkillName.Swords].Base = 125.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
			Skills[SkillName.Poisoning].Base = 120;
			Skills[SkillName.Magery].Base = 120;


      			Item shroud = new HoodedShroudOfShadows();

       		        shroud.Name = "Shroud of the Fallen Souls";
         		shroud.Hue = 1157;
         		shroud.Movable = false;

         		AddItem( shroud );

			Item sandals = new Sandals();
			sandals.Name = "Sandals of the Fallen";
			sandals.Hue = 1;
			sandals.Movable = false;
			PackItem( new NecroCrystal( 1 ) );
			AddItem( sandals );

			Item llegs = new StuddedLegs();

			AddItem( llegs );

			Item larms = new StuddedArms();

			AddItem( larms );

			Item ltunic = new StuddedChest();

			AddItem( ltunic );

			Item lgloves = new StuddedGloves();

			AddItem( lgloves );

			Item lgorget = new StuddedGorget();

			AddItem( lgorget );


}
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } } //added


		public override bool HandlesOnSpeech( Mobile from )
		{
			return true;
		}

      private PSCastTimer m_CastTimer;
      private PoisonSpell m_pss;

      public override void OnSpeech( SpeechEventArgs e )
      {
         base.OnSpeech(e);
         Mobile from = e.Mobile;

// if( e.Layer.Helm is NecromaticMask )
	if ( from.FindItemOnLayer( Layer.Helm ) is NecromaticMask )
         {
            if (e.Speech.IndexOf( "nwapslleh" ) >= 0  )

            {
                  m_pss = new PoisonSpell( this, null ); // get your spell
                  if( m_pss.Cast() ) // if it casts the spell
                  {
                     TimeSpan castDelay = m_pss.GetCastDelay();
                     m_CastTimer = new PSCastTimer( m_pss, from, castDelay );
                     m_CastTimer.Start();
                  }
               }
            }
}
     private class PSCastTimer : Timer
      {
         private PoisonSpell m_Spell;
         private Mobile m_Target;

         public PSCastTimer( PoisonSpell spell, Mobile target, TimeSpan castDelay ) : base( castDelay )
         {
            m_Spell = spell;
            m_Target = target;
            Priority = TimerPriority.OneSecond;
         }

         protected override void OnTick()
         {
            m_Spell.Target( m_Target );
         }
      }



		public EvilNecroWarrior( Serial serial ) : base( serial )
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