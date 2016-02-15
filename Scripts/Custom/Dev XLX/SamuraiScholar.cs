using System;
using System.Collections;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class SamuraiScholar : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"We live to serve.",
		"To acquire knowledge, one must study; but to acquire wisdom, one must observe.",
		"Knowledge comes, but wisdom lingers.",
		"You are not part of the clan."
		};

                public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public SamuraiScholar() : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.175, 0.3)
		{

			Name = "Samurai Scholar";
			Title= ", Defiance Cult Clan";
			Body = 400;
                        Hue = 33820;
			SpeechHue= 1150;
			BaseSoundID = 0;
			Team = 0;
                        //new EtherealHorse().Rider = this;

			SetStr( 155, 175);
			SetDex( 130, 140);
			SetInt( 260, 450);

			SetHits(125, 145);

			SetSkill( SkillName.Tactics, 100.7, 100.4);
                        SetSkill( SkillName.Magery, 110.7, 110.8);
                        SetSkill( SkillName.EvalInt, 115.7, 115.7);
			SetSkill( SkillName.MagicResist, 110.4, 115.7);
			SetSkill( SkillName.Wrestling, 110.4, 110.7);
			SetSkill( SkillName.Anatomy, 110.4, 110.7);
			SetSkill( SkillName.Parry, 75.1, 100.1);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 75;

			Item Spellbook = new Spellbook();
			Spellbook.Movable=false;
			Spellbook.Hue=0;
		        Spellbook.Name="Tome of Ancient Wisdom";
                        EquipItem( Spellbook );

                        Item Kasa = new Kasa();
			Kasa.Movable=false;
			Kasa.Hue=0;
			EquipItem( Kasa );

			Item Shirt = new Shirt();
			Shirt.Movable=false;
			Shirt.Hue=0;
			EquipItem( Shirt );

                        Item LeatherGloves = new LeatherGloves();
			LeatherGloves.Movable=false;
			LeatherGloves.Hue=0;
                        EquipItem( LeatherGloves );

                        Item MaleKimono = new MaleKimono();
			MaleKimono.Movable=false;
			MaleKimono.Hue=0;
		        EquipItem( MaleKimono );

			Item Sandals = new Sandals();
			Sandals.Movable=false;
			Sandals.Hue=0;
			EquipItem( Sandals );

			Item hair = new Item( 0x203D);
			hair.Hue = 0;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

                        Item beard = new Item( 0x203E);
			beard.Hue = 0;
			beard.Layer = Layer.FacialHair;
			beard.Movable = false;
			AddItem( beard );

			PackGold( 140, 255);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);
			PackMagicItems( 3, 7);

                                switch ( Utility.Random( 125 ))
        		 {
           			case 0: PackItem( new HakamaShita() ); break;
        		 }
		}

                public override bool AutoDispel{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }


	        public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && c.ControlMaster != null )
				{
					c.ControlTarget = c.ControlMaster;
					c.ControlOrder = OrderType.Attack;
					c.Combatant = c.ControlMaster;
				}
			}
		}

                public override void OnMovement( Mobile m, Point3D oldLocation )
                {
         		if( m_Talked == false )
        		 {
          		 	 if ( m.InRange( this, 4 ) )
          			  {
          				m_Talked = true;
              				SayRandom( kfcsay, this );
				this.Move( GetDirectionTo( m.Location ) );
				SpamTimer t = new SpamTimer();
				t.Start();
            			}
		}
	        }

                private class SpamTimer : Timer
	        {
		public SpamTimer() : base( TimeSpan.FromSeconds( 5 ) )
		{
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
		m_Talked = false;
		}
	        }

                private static void SayRandom( string[] say, Mobile m )
	        {
		m.Say( say[Utility.Random( say.Length )] );
	        }

		public SamuraiScholar( Serial serial ) : base( serial )
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
		}
	}
}