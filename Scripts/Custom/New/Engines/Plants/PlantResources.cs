using System;
using Server.Targeting;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class RedLeaf : Item
	{
		public override int LabelNumber{ get{ return 1053123; } } // red leaves

		[Constructable]
		public RedLeaf() : this(1)
		{
		}

		[Constructable]
		public RedLeaf(int amount) : base(0x1E85)
		{
			Hue = 0x21;
			Weight = 0.1;
			Amount = amount;
			Stackable = true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
		if (item != this)
			return base.CheckItemUse(from, item);
		if (from != this.RootParent)
			{
			from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			return false;
			}
		return base.CheckItemUse(from, item);
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new RedLeaf(), amount );
		}

		public override void OnDoubleClick( Mobile from )
		{
		from.SendLocalizedMessage( 1061907 );//Choose a book you wish to seal with the wax from the red leaf..
		from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private Item m_RedLeaf;

			public InternalTarget(Item RedLeaf) : base( 6, true, TargetFlags.None )
			{
			m_RedLeaf = RedLeaf;
			}

			protected override void OnTarget( Mobile from, object o )
			{

				if  ( o is BaseBook )
					{
					BaseBook to = o as BaseBook;
					if ( to.Writable )
						{
						to.Writable = false;
						m_RedLeaf.Consume();
						to.SendLocalizedMessageTo( from, 1061910 );// You seal the ink to the page using wax from the red leaf.
						}
					else
						{
						to.SendLocalizedMessageTo( from, 1061909 );// The ink in this book has already been sealed.
						}
					}
				else
					{
					from.SendLocalizedMessage( 1053090 );// You can only use the red leaves to seal the ink into book pages!
					}

			}

		}

		public RedLeaf(Serial serial) : base(serial)
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



	public class GreenThorn : Item
	{

		public override int LabelNumber{ get{ return 1060837; } } // green thorns

		[Constructable]
		public GreenThorn() : this(1)
		{
		}

		[Constructable]
		public GreenThorn(int amount) : base(0xF42)
		{
			Hue = 0x42;
			Weight = 0.1;
			Amount = amount;
			Stackable = true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
		if (item != this)
			return base.CheckItemUse(from, item);
		if (from != this.RootParent)
			{
			from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			return false;
			}
		return base.CheckItemUse(from, item);
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new GreenThorn(), amount );
		}

		public override void OnDoubleClick( Mobile from )
		{
		if ( from.BeginAction( typeof( GreenThorn ) ) )
			{
			from.SendLocalizedMessage( 1061906 ); // Choose a spot to plant the thorn.
			from.Target = new InternalTarget(this);
			}
		else
			from.SendLocalizedMessage( 1061908 ); // * You must wait a while before planting another thorn. *
		}

		private class InternalTarget : Target
		{
			private Item m_GreenThorn;
			private static int[] m_SwampIDs = new int[]
				{
				0x9c4, 0x9eb,
				0x3d65, 0x3ef0,
				0x3ffc, 0x3ffe,
				0x73f, 0x742
				};
			private static int[] m_FurrowsIDs = new int[]
				{
				0x9, 0x15,
				0x150, 0x15c
				};
			private static int[] m_DirtIDs = new int[]
				{
				0x71, 0xa7,
				0xdc, 0xeb,
				0x141, 0x14f,
				0x169, 0x174,
				0x1dc, 0x1ef,
				0x272, 0x275,
				0x27e, 0x281,
				0x2d0, 0x31f,
				0x32c, 0x32f,
				0x355, 0x358,
				0x367, 0x36e,
				0x3a5, 0x3a8,
				0x547, 0x556,
				0x597, 0x59e,
				0x623, 0x63a,
				0x777, 0x791
				};
			private static int[] m_SandIDs = new int[]
				{
				0x16, 0x4b,
				0x11e, 0x12d,
				0x1a8, 0x1ab,
				0x1b9, 0x1d1,
				0x282, 0x285,
				0x28a, 0x291,
				0x335, 0x354,
				0x359, 0x35c,
				0x3b7, 0x3ca,
				0x5a7, 0x5b2,
				0x64b, 0x652,
				0x657, 0x66a,
				0x66f, 0x672
				};
			private static int[] m_SnowIDs = new int[]
				{
				0x10c, 0x11d,
				0x179, 0x18a,
				0x385, 0x3a4,
				0x3a9, 0x3ac,
				0x5bf, 0x5e2
				};


			public InternalTarget(Item GreenThorn) : base( 6, true, TargetFlags.None )
			{
			m_GreenThorn = GreenThorn;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				int itemID = 0;
				if (o is LandTarget)
				{
				if (from.Map == Map.Malas || from.Map == Map.Ilshenar)
					{
					from.PrivateOverheadMessage( MessageType.Regular, 0x2B2, true, "No solen lairs exist on this facet.  Try again in Trammel or Felucca.", from.NetState);
					return;
					}
				Point3D p = new Point3D( o as IPoint3D );
				itemID = from.Map.Tiles.GetLandTile( ((LandTarget)o).X, ((LandTarget)o).Y ).ID & 0x3FFF;
				bool goodspot = false;
				int effect = 0;
				for ( int i = 0; i < m_DirtIDs.Length; i += 2 )
					{
					if ( itemID >= m_DirtIDs[i] && itemID <= m_DirtIDs[i + 1] )
						{
						goodspot = true;
						effect = 1;
						}
					}
				for ( int i = 0; i < m_SwampIDs.Length; i += 2 )
					{
					if ( itemID >= m_SwampIDs[i] && itemID <= m_SwampIDs[i + 1] )
						{
						goodspot = true;
						effect = 2;
						}
					}
				for ( int i = 0; i < m_SandIDs.Length; i += 2 )
					{
					if ( itemID >= m_SandIDs[i] && itemID <= m_SandIDs[i + 1] )
						{
						goodspot = true;
						effect = 3;
						}
					}
				for ( int i = 0; i < m_SnowIDs.Length; i += 2 )
					{
					if ( itemID >= m_SnowIDs[i] && itemID <= m_SnowIDs[i + 1] )
						{
						goodspot = true;
						effect = 4;
						}
					}
				for ( int i = 0; i < m_FurrowsIDs.Length; i += 2 )
					{
					if ( itemID >= m_FurrowsIDs[i] && itemID <= m_FurrowsIDs[i + 1] )
						{
						goodspot = true;
						effect = 5;
						}
					}
				if ( goodspot )
					{
					m_GreenThorn.Consume();
					from.LocalOverheadMessage( MessageType.Emote, 0x2B2, 1061914 ); // * You push the strange green thorn into the ground *
					from.NonlocalOverheadMessage( MessageType.Emote, 0x2B2, 1061915, from.Name ); // * ~1_PLAYER_NAME~ pushes a strange green thorn into the ground. *
					from.PlaySound(0x106);
					Effects.SendLocationParticles( EffectItem.Create( p, from.Map, EffectItem.DefaultDuration ), 0x3735, 1, 182, 0xBE3 );
					new ThornEffectTimer( from, p, effect ).Start();
					new DelayTimer(from).Start();
					}
				else
					{
					from.SendLocalizedMessage( 1061913 ); // * You sense it would be useless to plant a green thorn there. *
					from.EndAction( typeof( GreenThorn ) );
					}
				}
				else
					{
					from.SendLocalizedMessage( 1061912 ); // * You cannot plant a green thorn there! *
					from.EndAction( typeof( GreenThorn ) );
					}
			}

		}

		private class ThornEffectTimer : Timer
		{
			private Mobile from;
			private Point3D p;
			private int i = 0;
			private int effect;
			private object temp;

			public ThornEffectTimer( Mobile m_from, IPoint3D p1, int m_effect ) : base ( TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 1 ) )
				{
				effect = m_effect;
				from = m_from;
				p = new Point3D( p1 );
				}

			protected override void OnTick()
				{
				i++;
				switch (effect)
					{
					case 1:
					{
					Item randomreg = null;
					switch (i)
						{
						case 4:
							{
							from.PlaySound(0x222);
							break;
							}
						case 8:
							{
							from.PlaySound(0x21F);
							break;
							}
						case 13:
							{
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "*The ground erupts with chaotic growth!*", from.NetState);
							from.PlaySound(0x12D);
							randomreg = RandomReg();
							randomreg.Amount = Utility.RandomMinMax(15,22);
							randomreg.MoveToWorld( p, from.Map);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X-1, p.Y-1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(14,20);
							break;
							}
						case 15:
							{
							from.PlaySound(0x12D);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X, p.Y+1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(13,18);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X+1, p.Y-1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(16,22);
							break;
							}
						case 17:
							{
							from.PlaySound(0x12D);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X, p.Y-1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(16,20);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X+1, p.Y-1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(14,18);
							break;
							}
						case 20:
							{
							from.PlaySound(0x12D);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X+1, p.Y+1, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(16,20);
							randomreg = RandomReg();
							randomreg.MoveToWorld( new Point3D( p.X-1, p.Y, p.Z) , from.Map );
							randomreg.Amount = Utility.RandomMinMax(16,20);
							Stop();
							break;
							}
						}
					break;
					}
					case 2:
					{
					switch (i)
						{
						case 4:
							{
							from.PlaySound(0x222);
							break;
							}
						case 8:
							{
							from.PlaySound(0x21F);
							break;
							}
						case 9:
							{
							from.PlaySound(0x2B0);
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "*Strange green tendrils rise from the ground, whipping wildly!*", from.NetState);
							BaseCreature WV = new WhippingVine();
							WV.Map = from.Map;
							WV.Location = p;
							WV.Combatant = from;
							break;
							}
						}
					break;
					}
					case 3:
					{
					switch (i)
						{
						case 4:
							{
							from.PlaySound(0x222);
							break;
							}
						case 8:
							{
							from.PlaySound(0x21F);
							break;
							}
						case 13:
							{
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "*The sand collapses, revealing a dark hole.*", from.NetState);
							Item SH = new SolenHiveEntrance();
							SH.MoveToWorld( p, from.Map );
							break;
							}
						}
					break;
					}
					case 4://ToDO Ice Worm
					{
					switch (i)
						{
						case 4:
							{
							from.PlaySound(0x222);
							break;
							}
						case 8:
							{
							from.PlaySound(0x21F);
							break;
							}
						case 12:
							{
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "*Slithering ice serpaents rise to the surface to investigate the disturbance!*", from.NetState);
							BaseCreature IW = new GiantIceWorm();
							IW.Map = from.Map;
							IW.Location = p;
							IW.Combatant = from;
							for (int j=0; j < 3; j++)
								{
								BaseCreature IS = new IceSnake();
								IS.Map = from.Map;
								IS.Location = p;
								IS.Combatant = from;
								}
							break;
							}
						}
					break;
					}
					case 5:
					{
					switch (i)
						{
						case 4:
							{
							Item hole = new Item(0X913);
							hole.MoveToWorld( p, from.Map );
							hole.Movable = false;
							temp = hole;
							from.PlaySound(0x222);
							break;
							}
						case 8:
							{
							from.PlaySound(0x21F);
							break;
							}
						case 12:
							{
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "*A magical bunny leaps out of its hole, disturbed by the thorn's effect!*", from.NetState);
							BaseCreature VB = new VorpalBunny();
							VB.Map = from.Map;
							VB.Location = p;
							VB.Combatant = from;
							((Item)temp).Delete();
							break;
							}
						}
					break;
					}
					}
				}

			private Item RandomReg()
				{
				if (Utility.Random(8) == 0)
					return new FertileDirt();
				else
					return Loot.RandomReagent();
				}
		}

		private class DelayTimer : Timer
		{
			private Mobile m_Mobile;

			public DelayTimer( Mobile m ) : base( TimeSpan.FromMinutes( 5.0 ) )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.EndAction( typeof( GreenThorn ) );
			}
		}

		public GreenThorn(Serial serial) : base(serial)
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






	public class SolenHiveEntrance : Item
	{
		[Constructable]
		public SolenHiveEntrance() : base( 0x913 )
		{
		Name = "a hole";
		Movable = false;
		new DelTimer(this).Start();
		}

		private class DelTimer : Timer
		{
			SolenHiveEntrance sd;
			public DelTimer(SolenHiveEntrance s) : base ( TimeSpan.FromMinutes( 1 ) )
				{
				sd = s;
				}

			protected override void OnTick()
				{
				sd.Delete();
				}
		}

		public override void OnDoubleClick( Mobile from )
		{
		from.Location = new Point3D( 5738, 1856, 0 );
		}

		public SolenHiveEntrance( Serial serial ) : base( serial )
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