using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2683, 0x2684 )]
	public class HoodedRobe : BaseOuterTorso
	{
		[Constructable]
		public HoodedRobe() : base( 0x2683 )
		{
			Weight = 5.0;
			Name = "Shroud";
			Layer = Layer.OuterTorso;
			Hue = 1109;
		}


		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( Parent != from )
				from.SendMessage( "You must be wearing the robe to use it!" );
			else
			{
				if ( ItemID == 0x2683 || ItemID == 0x2684 )
				{
					ItemID = 0x1F03;
					from.SendMessage( "You lower the hood." );
					UnMorph( from );
				}
				else if ( ItemID == 0x1F03 || ItemID == 0x1F04 )
				{
					from.SendMessage( "You pull the hood over your head." );
					ItemID = 0x2683;
					Morph( from );
				}
			}
		}

		public override void OnAdded( Object parent )
		{
			if ( ItemID == 0x2683 && parent is Mobile )
			{
				Mobile from = parent as Mobile;
				Morph( from );
			}
			base.OnAdded( parent );
		}

		public override void OnRemoved( Object parent )
		{
			if( parent is Mobile )
			{
				Mobile from = parent as Mobile;
				UnMorph( from );
			}

			base.OnRemoved( parent );
		}

		void Morph( Mobile from )
		{
			from.NameMod = "a shrouded figure";
			from.DisplayGuildTitle = false;
		}

		void UnMorph( Mobile from )
		{
			from.NameMod = null;
			if( from.GuildTitle != null )
				from.DisplayGuildTitle = true;
		}

		public HoodedRobe( Serial serial ) : base( serial )
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