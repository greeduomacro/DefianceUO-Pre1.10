using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class FakeFallenHero : BaseFakeMob
	{
		[Constructable]
		public FakeFallenHero() : base( AIType.AI_Melee )
		{
			Hue = 22222;
			Body = 774;
			Name = "a fallen hero";

			SetStr( 400 );
			SetDex( 400 );
			SetInt( 400 );

			SetHits ( 1200 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.MagicResist, 180.0 );
			SetSkill( SkillName.Tactics, 140.0 );
			SetSkill( SkillName.Wrestling, 140.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 100;
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public override void OnThink() // Taken from Tree.cs and changed to drain stamina from pets and players but is not ment to add it to its own stam
		{
				ArrayList list = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 12 ) )
				{
					if ( m == this || !CanBeHarmful( m ) || m.Stam < 5 )
						continue;

					if ( m is PlayerMobile || m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
						list.Add( m );
				}

				foreach ( Mobile m in list )
				{
					m.PlaySound( 0x231 );
					m.SendMessage( "You feel your stamina draining away!" );
					m.Stam -= 2;
				}
			base.OnThink();
		}

		public FakeFallenHero( Serial serial ) : base( serial )
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