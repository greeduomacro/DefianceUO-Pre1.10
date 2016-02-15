using Server;
using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Items
{
	public class BaseThrowingItem : Item
	{
		public virtual int DamageMin { get { return 1; } }
		public virtual int DamageMax { get { return 2; } }
		public virtual bool Break { get { return false; } }
		public virtual bool DeleteOnThrow { get { return true; } }

		public BaseThrowingItem() : base( 0x1C12 )
		{
			Name = "base throwing item";
			Weight = 5;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from == null||from.Backpack == null )
				return;

			if ( !IsChildOf(from.Backpack) )
			{
				from.SendMessage("You need to pick this up to use it!");
				return;
			}

			from.RevealingAction();
			from.Target = new ThrowTarget( this, DamageMin, DamageMax, Break, DeleteOnThrow );
		}

		private class ThrowTarget : Target
		{
			private BaseThrowingItem m_BaseThrowingItem;
			private int m_DamageMin;
			private int m_DamageMax;
			private bool m_Break;
			private bool m_DeleteOnThrow;

			public ThrowTarget( BaseThrowingItem bti, int min, int max, bool b, bool del ) : base( 8, true, TargetFlags.Harmful )
			{
				m_BaseThrowingItem = bti;
				m_DamageMin = min;
				m_DamageMax = max;
				m_Break = b;
				m_DeleteOnThrow = del;
			}

			protected override void OnTarget( Mobile from, object obj )
			{
				if ( m_BaseThrowingItem.Deleted || m_BaseThrowingItem.Map == Map.Internal)
					return;

				if ( obj is Mobile )
				{
					Mobile to = (Mobile)obj;

					if ( !from.CanBeHarmful( to ) )
					{
					}
					else
					{	from.Direction = from.GetDirectionTo( to );
						from.Animate( 11, 5, 1, true, false, 0 );
						from.MovingEffect( to, m_BaseThrowingItem.ItemID, 10, 0, false, false );

						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishThrow ), new object[]{ from, to, m_DamageMin, m_DamageMax, m_Break, m_BaseThrowingItem } );

						if ( m_DeleteOnThrow || m_Break )
							m_BaseThrowingItem.Delete();
					}
				}
				else
				{
					IPoint3D p = obj as IPoint3D;

					if ( p == null )
						return;

					Map map = from.Map;

					if ( map == null )
						return;

					IEntity to;

					to = new Entity( Serial.Zero, new Point3D( p ), map );

					from.Direction = from.GetDirectionTo( to );
					Effects.SendMovingEffect( from, to, m_BaseThrowingItem.ItemID & 0x3FFF, 7, 0, false, false, m_BaseThrowingItem.Hue, 0 );
					from.Animate( 11, 5, 1, true, false, 0 );

					if ( m_DeleteOnThrow )
					{
						m_BaseThrowingItem.Delete();
						from.SendMessage( "You miss the target and the {0} is wasted", m_BaseThrowingItem.Name );
					}
					else
					{
						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishMiss ), new object[]{ to, map, m_BaseThrowingItem } );
						from.SendMessage( "You miss the target" );
					}
				}
			}
		}

		private static void FinishThrow( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Mobile to = (Mobile)states[1];
			int m_DamageMin = (int)states[2];
			int m_DamageMax = (int)states[3];
			bool m_Break = (bool)states[4];
			Item obj = (Item)states[5];

			Map map = from.Map;

			to.Damage( Utility.RandomMinMax( m_DamageMin, m_DamageMax ), from );

			if ( m_Break )
			{
				if (map == null)
                    			return;

				int count = Utility.RandomMinMax( 1, 4 );

               			for (int i = 0; i < count; ++i)
                		{
                    			int x = to.X + Utility.RandomMinMax(-1, 1);
                    			int y = to.Y + Utility.RandomMinMax(-1, 1);
                    			int z = to.Z;

                   			if (!map.CanFit(x, y, z, 16, false, true))
                    			{
                        			z = map.GetAverageZ(x, y);

                        			if (z == to.Z || !map.CanFit(x, y, z, 16, false, true))
                           				continue;
                    			}

                    			StoolLeg leg = new StoolLeg();
                    			leg.MoveToWorld(new Point3D(x, y, z), map);
                		}
			}

			if ( obj != null )
				obj.MoveToWorld( to.Location, map );
		}

		private static void FinishMiss( object state )
		{
			object[] states = (object[])state;

			IPoint3D p = (IPoint3D)states[0];
			Map map = (Map)states[1];
			Item obj = (Item)states[2];

			Point3D loc = new Point3D( p );
			obj.MoveToWorld( loc, map );
		}

		public BaseThrowingItem( Serial serial ) : base( serial )
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

	public class ThrowingKnife : BaseThrowingItem
	{
		public override int DamageMin { get { return 7; } }
		public override int DamageMax { get { return 9; } }

		[Constructable]
		public ThrowingKnife() : base()
		{
			ItemID = 0x9F6;
			Name = "throwing knife";
		}

		public ThrowingKnife( Serial serial ) : base( serial )
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

	public class ThrowingStool : BaseThrowingItem
	{
		public override int DamageMin { get { return 9; } }
		public override int DamageMax { get { return 12; } }
		public override bool Break { get { return true; } }
		public override bool DeleteOnThrow { get { return false; } }

		[Constructable]
		public ThrowingStool() : base()
		{
			ItemID = 0xA2A;
			Name = "stool";
		}

		public ThrowingStool( Serial serial ) : base( serial )
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

	public class ThrowingSpittoon : BaseThrowingItem
	{
		public override int DamageMin { get { return 4; } }
		public override int DamageMax { get { return 7; } }

		[Constructable]
		public ThrowingSpittoon() : base()
		{
			ItemID = 0x1003;
			Name = "spittoon";
		}

		public ThrowingSpittoon( Serial serial ) : base( serial )
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

	public class ThrowingMug : BaseThrowingItem
	{
		public override int DamageMin { get { return 3; } }
		public override int DamageMax { get { return 9; } }

		[Constructable]
		public ThrowingMug() : base()
		{
			ItemID = 0xFFF;
			Name = "ale mug";
		}

		public ThrowingMug( Serial serial ) : base( serial )
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

	public class ThrowingPitcher : BaseThrowingItem
	{
		public override int DamageMin { get { return 2; } }
		public override int DamageMax { get { return 8; } }

		[Constructable]
		public ThrowingPitcher() : base()
		{
			ItemID = 0xFF6;
			Name = "an empty pitcher";
		}

		public ThrowingPitcher( Serial serial ) : base( serial )
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

	public class ThrowingChair : BaseThrowingItem
	{
		public override int DamageMin { get { return 7; } }
		public override int DamageMax { get { return 12; } }
		public override bool DeleteOnThrow { get { return false; } }

		[Constructable]
		public ThrowingChair() : base()
		{
			ItemID = 0xB57;
			Name = "a chair";
		}

		public ThrowingChair( Serial serial ) : base( serial )
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

	public class ThrowingBottle : BaseExplosionPotion
	{
		public override int MinDamage { get { return 6; } }
		public override int MaxDamage { get { return 12; } }
		public override double Delay { get{ return 5.0; } }

		private static bool InstantExplosion = true;

		[Constructable]
		public ThrowingBottle() : base( PotionEffect.Explosion )
		{
			ItemID = Utility.RandomList( 2459, 2503, 2463 );
		}

		public ThrowingBottle( Serial serial ) : base( serial )
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