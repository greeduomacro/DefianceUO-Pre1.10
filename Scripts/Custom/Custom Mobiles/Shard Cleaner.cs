using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a ShardCleaner corpse" )]
	public class ShardCleaner : BaseCreature
	{
		[Constructable]
		public ShardCleaner() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "the Garbage man";
			Hue = 0;
			Body = 777;
			Blessed = true;


			SetStr( 186, 300 );
			SetDex( 181, 295 );
			SetInt( 61, 75 );

			SetDamage( 13, 28 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 65, 75 );
			SetResistance( ResistanceType.Energy, 25, 35 );


			SetSkill( SkillName.MagicResist, 105.0, 107.5 );
			SetSkill( SkillName.Tactics, 85.0, 97.5 );
			SetSkill( SkillName.Wrestling, 85.0, 97.5 );

			Fame = 13000;
			Karma = -13000;

			VirtualArmor = 50;
		}

		private DateTime m_NextPickup;

		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.Now >= m_NextPickup )
			{
				m_NextPickup = DateTime.Now + TimeSpan.FromSeconds( 2.5 + (2.5 * Utility.RandomDouble()) );

				ArrayList Trash = new ArrayList();
				foreach ( Item item in this.GetItemsInRange( 4 ) )
					if ( item.Movable )
						Trash.Add(item);

				for (int i = 0; i < Trash.Count; i++)
					((Item)Trash[i]).Delete();
			}
		}

		public ShardCleaner( Serial serial ) : base( serial )
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