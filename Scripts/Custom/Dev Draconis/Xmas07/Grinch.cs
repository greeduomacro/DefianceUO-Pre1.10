using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class Grinch : BaseBoss
	{
		[Constructable]
		public Grinch() : base( AIType.AI_Mage )
		{
			Name = "the grinch";
			Body = 253;
			Hue = 2006;

			SetStr( 400 );
			SetDex( 150 );
			SetInt( 3000 );

			SetHits( 10000 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.MagicResist, 220.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.EvalInt, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 35 ) < 1 )
			{
				DecemberDeed deed = new DecemberDeed();
				deed.Hue = 2006;
				deed.Name = "a deed from the Grinch";
				deed.Movable = true;
				deed.Weight = 5.0;
				c.DropItem( deed );
			}

            		int cnt = 1;
            		if ( Utility.Random( 4 ) < 1 ) cnt++;
            		if ( Utility.Random( 5 ) < 1 ) cnt++;

            		for (int i = 0; i < cnt; ++i)
            		{
                		switch (Utility.Random(4))
                		{
                    			case 0: c.DropItem( new SpecialHairDye() ); break;
                    			case 1: c.DropItem( new SpecialBeardDye() ); break;
                    			case 2: c.DropItem( new ClothingBlessDeed() ); break;
                    			case 3: c.DropItem( new NameChangeDeed() ); break;
                		}
            		}
			base.OnDeath( c );
		}

		public override int CanCastReflect{ get { return 18; } }
		public override bool DoImmuneToPets { get { return true; } }
		public override int CanBandageSelf{ get { return 40; } }
		public override int DoWeaponsDoMoreDamage{ get { return 3; } }
		public override bool DoDisarmPlayer{ get { return true; } }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && 0.01 >= Utility.RandomDouble() )
			{
				BaseCreature spawn = new ConfusedFairy();

				spawn.Team = this.Team;
				spawn.MoveToWorld( from.Location, from.Map );
				spawn.Combatant = from;
				spawn.Say("Here I am my master");
			}
			base.OnDamage(amount, from, willKill);
		}

		public override bool OnBeforeDeath()
		{
			ArrayList toRevenge = new ArrayList();
			ArrayList rights = BaseCreature.GetLootingRights(this.DamageEntries,this.HitsMax);

			for (int i = rights.Count - 1; i >= 0; --i)
			{
				DamageStore ds = (DamageStore)rights[i];

				if (ds.m_HasRight && CanBeHarmful(ds.m_Mobile))
					toRevenge.Add(ds.m_Mobile);
			}

			foreach ( Mobile m in toRevenge )
			{
				m.Poison = Poison.Lethal;
				m.Freeze( TimeSpan.FromSeconds( 15.0 ) );
				m.SendMessage( "You have slain the grinch, but at what cost?" );
			}
			return base.OnBeforeDeath();
		}

		public Grinch( Serial serial ) : base( serial )
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