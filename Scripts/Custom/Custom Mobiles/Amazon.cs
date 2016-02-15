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
	public class AmazonHealer : BaseCreature
	{

		private static bool m_Talked;

		string[] kfcsay = new string[]
		{
		"You weak Ronin !",
		"Serve one Master.",
		"We who remain true shall rise again...",
		};
		[Constructable]
		public AmazonHealer() : base( AIType.AI_Healer, FightMode.Weakest, 10, 1, 0.175, 0.3)
		{

			Name = "a Amazon Healer";
			Title= ", Defiance Amazon Clan";
			Hue= 33788;
			Body = 401;
			SpeechHue= 2305;
			BaseSoundID = 0;
			Team = 0;
                        new Horse().Rider = this;

			SetStr( 185, 215);
			SetDex( 130, 140);
			SetInt( 300, 320);

			SetHits(185, 300);

                        SetSkill( SkillName.Magery, 100.7, 100.4);
			SetSkill( SkillName.Tactics, 100.7, 100.4);
			SetSkill( SkillName.MagicResist, 191.4, 191.7);
			SetSkill( SkillName.Swords, 110.4, 110.7);
			SetSkill( SkillName.Anatomy, 110.4, 110.7);
			SetSkill( SkillName.Parry, 75.1, 100.1);

                        Fame=15000;
			Karma=-15000;

			VirtualArmor= 75;

			Item Spellbook = new Spellbook();
			Spellbook.Movable=false;
			Spellbook.Hue=1157;
		        Spellbook.Name="Amazon Healing Guide";
                        EquipItem( Spellbook );

			//Item Buckler = new Buckler();
			//Buckler.Movable=false;
			//Buckler.Hue=1253;
		        //EquipItem( Buckler );

                        Item Bandana = new Bandana();
			Bandana.Movable=false;
			Bandana.Hue=0;
			EquipItem( Bandana );

			Item Doublet = new Doublet();
			Doublet.Movable=false;
			Doublet.Hue=1150;
			Doublet.Name="Amazon Clothing";
			EquipItem( Doublet );

                        Item LeatherArms = new LeatherArms();
			LeatherArms.Movable=false;
			LeatherArms.Hue=0;
                        EquipItem( LeatherArms );

                        Item LeatherSkirt = new LeatherSkirt();
			LeatherSkirt.Movable=false;
			LeatherSkirt.Hue=0;
		        EquipItem( LeatherSkirt );

			Item Boots = new Boots();
			Boots.Movable=false;
			Boots.Hue=1175;
			EquipItem( Boots );

			//Item BodySash = new BodySash();
			//BodySash.Movable=false;
			//BodySash.Hue=4;
			//BodySash.Name="AmazonHealer Clan Member.";
                        //EquipItem( BodySash );

			Item hair = new Item( 0x203D);
			hair.Hue = 0;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			PackGold( 250, 800);
			PackMagicItems( 0, 7);
			PackMagicItems( 0, 7);
			PackMagicItems( 0, 7);
			PackMagicItems( 0, 7);

                                switch ( Utility.Random( 15 ))
        		 {
           			case 0: PackItem( new HoodedShroudOfShadows() ); break;
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




		public AmazonHealer( Serial serial ) : base( serial )
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