using System;
using System.Collections;
using Server.Multis;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class SmallBong : Item
	{
		[Constructable]
		public SmallBong() : base(0x182d)
		{
			Weight = 10;
			Name = "a one foot bong";
			Hue = 1150;
		}

		public SmallBong(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			if ( pack != null && pack.ConsumeTotal( typeof( Opium ), 1 ) )
			{
				Effects.SendLocationEffect( new Point3D( from.X + 1, from.Y, from.Z + 19 ), from.Map, 0x3735, 13 );
				from.Animate( 34, 5, 1, true, false, 0 );
				from.PlaySound( 0x21 );
				from.Paralyze( TimeSpan.FromSeconds( 5 ) );
				from.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Head );
			}
			else
			{
				from.SendMessage(37, "You are out of opium!");
			}
		}
	}

	public class LargeBong : Item
	{
		[Constructable]
		public LargeBong() : base(0x183d)
		{
			Weight = 15;
			Name = "a two foot bong";
			Hue = 1150;
		}

		public LargeBong(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			if ( pack != null && pack.ConsumeTotal( typeof( Marijuana ), 1 ) )
			{
				Effects.SendLocationEffect( new Point3D( from.X + 1, from.Y, from.Z + 19 ), from.Map, 0x3735, 13 );
				from.Animate( 34, 5, 1, true, false, 0 );
				from.PlaySound( 0x21 );
				from.Paralyze( TimeSpan.FromSeconds( 5 ) );
				Server.Spells.SpellHelper.AddStatCurse( from, from, StatType.Dex );
				Server.Spells.SpellHelper.AddStatCurse( from, from, StatType.Str );
				from.Hunger = 0x0;
				from.Thirst = 0x0;
			}
			else
			{
				from.SendMessage(37, "You are out of marjiuana!");
			}
		}
	}

	public class CrackPipe : Item
	{
		[Constructable]
		public CrackPipe() : base(0xe28)
		{
			Weight = 5;
			Name = "a crack pipe";
			Hue = 1150;
		}

		public CrackPipe(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			if ( pack != null && pack.ConsumeTotal( typeof( CrackRock ), 1 ) )
			{
				from.SendMessage("You O.D. on Crack!");
				from.Kill();
			}
			else
			{
				from.SendMessage(37, "You are out of crack rock!");
			}
		}
	}

	public class Syringe : Item
	{
		[Constructable]
		public Syringe() : base(0xfb8)
		{
			Weight = 2;
			Name = "syringe";
			Hue = 1150;
		}

		public Syringe(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage("You O.D. on Heroin!");
			from.Kill();
			this.Delete();
		}
	}

	public class Marijuana : Item
	{
		[Constructable]
		public Marijuana() : this(1)
		{
		}

		[Constructable]
		public Marijuana(int amount) : base(0xf88)
		{
			Stackable=true;
			Amount=amount;
			Weight = 0.5;
			Name = "marijuana";
			Hue = 0x23c;
		}

		public Marijuana(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class GrowableMarijuana : Item
	{
		[Constructable]
		public GrowableMarijuana() : base(0x18e6)
		{
			Weight = 0.5;
			Name = "marijuana";
			Hue = 0x23c;
			Movable = false;
		}

		public GrowableMarijuana(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container bp = from.Backpack;

			this.Delete();
			bp.DropItem( new Marijuana() );
		}
	}

	public class Opium : Item
	{
		[Constructable]
		public Opium() : this(1)
		{
		}

		[Constructable]
		public Opium(int amount) : base(0xf7a)
		{
			Stackable=true;
			Amount=amount;
			Weight = 0.5;
			Name = "opium";
			Hue = 37;
		}

		public Opium(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class CrackRock : Item
	{
		[Constructable]
		public CrackRock() : this(1)
		{
		}

		[Constructable]
		public CrackRock(int amount) : base(0xf8b)
		{
			Stackable=true;
			Amount=amount;
			Weight = 0.5;
			Name = "crack rock";
			Hue = 1150;
		}

		public CrackRock(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class LSD : Item
	{
		[Constructable]
		public LSD() : this(1)
		{
		}

		[Constructable]
		public LSD(int amount) : base(0xf8e)
		{
			Stackable=true;
			Amount=amount;
			Weight = 0.5;
			Name = "LSD";
			Hue = 0x44;
		}

		public LSD(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			pack.ConsumeTotal( typeof( LSD ), 1 );
			from.SendMessage(1276, "You start trippin' on LSD!");
			new Server.Misc.AcidTimer( from ).Start();
		}
	}

	public class PartyPack : Bag
	{
		[Constructable]
		public PartyPack() : this( 5 )
		{
		}

		[Constructable]
		public PartyPack( int amount )
		{
			DropItem( new LSD( amount ) );
			DropItem( new CrackRock( amount ) );
			DropItem( new Marijuana( amount ) );
			DropItem( new Opium( amount ) );
			this.Name="Party Pack";
			this.Hue=0x44;
		}

		public PartyPack( Serial serial ) : base( serial )
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

namespace Server.Misc
{
	public class AcidTimer : Timer
	{
		private Mobile m_Mobile;
		private int m_State, m_Count, m_Old_Hue;
		private int m_Hue = 1675;
		private static int i = 5;

		public AcidTimer( Mobile m ) : this( m, i )
		{
		}

		public AcidTimer( Mobile m, int count ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 10.0 ) )
		{
			m_Mobile = m;
			m_Old_Hue = m.Hue;
			m_Count = count;
			m.Hue=m_Hue;
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
		}

		protected override void OnTick()
		{
			m_Mobile.Hue=m_Hue;
			m_State++;
			m_Mobile.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );

			if ( m_State == m_Count )
			{
				m_Mobile.Hue=m_Old_Hue;
				m_Mobile.SendMessage("You sober up.");
				Stop();
			}
		}
	}
}