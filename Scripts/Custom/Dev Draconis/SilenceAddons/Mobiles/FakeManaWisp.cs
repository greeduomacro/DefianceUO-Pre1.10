using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
//using Server.Engines.SilenceAddon;

namespace Server.Mobiles
{
	[CorpseName( "a mana wisp corpse" )]
	public class FakeManaWisp : BaseFakeMob
	{
		[Constructable]
		public FakeManaWisp() : base( AIType.AI_Melee )
		{
			Name = "a mana wisp";
			Body = 58;
			Kills = 5;
			Hue = 1266;
			BaseSoundID = 466;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 200);

			SetHits( 400 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.MagicResist, 300 );
			SetSkill( SkillName.Tactics, 120 );
			SetSkill( SkillName.Wrestling, 120 );

			Fame = 4000;
			Karma = 0;

			VirtualArmor = 40;

			AddItem( new LightSource() );
		}

		public override void OnThink() // Taken from Tree.cs and changed to drain mana from pets and players but is not ment to add it to its own mana
		{
				ArrayList list = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 12 ) )
				{
					if ( m == this || !CanBeHarmful( m ) || m.Mana < 10 )
						continue;

					if ( m is PlayerMobile || m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
						list.Add( m );
				}

				foreach ( Mobile m in list )
				{
					m.PlaySound( 0x231 );
					m.SendMessage( "You feel your mana draining away!" );
					m.Mana -= 10;
				}
			base.OnThink();
		}


		public FakeManaWisp( Serial serial ) : base( serial )
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