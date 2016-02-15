using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a corpse of nessa" )]
	public class Nessa : BaseCreature
	{
		[Constructable]
		public Nessa () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.2 )
		{
			Name = "Nessa the Shadow Beast";
			Body = 104;
			Hue = 0x4001; //Shadow
			BaseSoundID = 0x488;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 27558, 29599 );
			SetMana( 25000, 25000 );

			SetDamage( 29, 35 );

			SetSkill( SkillName.EvalInt, 77.1, 80.0 );
			SetSkill( SkillName.Magery, 100.1, 125.8 );
			SetSkill( SkillName.MagicResist, 128.3, 132.7 );
			SetSkill( SkillName.Tactics, 100.6, 125.0 );
			SetSkill( SkillName.Wrestling, 100.6, 125.0 );

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 75;

			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGold( 10500, 12900 );
			PackArmor( 2, 5 );
			PackWeapon( 2, 5 );
			PackArmor( 2, 5 );
			PackWeapon( 2, 5 );
			PackArmor( 2, 5 );
			PackWeapon( 2, 5 );
			PackArmor( 2, 5 );
			PackWeapon( 2, 5 );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override int Meat{ get{ return 30; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Hides{ get{ return 75; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			defender.Paralyze( TimeSpan.FromSeconds( 15.0 ) );
			PerformAttack( defender );
			base.OnGaveMeleeAttack( defender );
		}

		public Nessa( Serial serial ) : base( serial )
		{
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			PerformAttack( attacker );
		}

		public void PerformAttack( Mobile attacker )
		{
			if ( 0.50 >= Utility.RandomDouble() && attacker is BaseCreature )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.Controlled && ( c.ControlMaster != null || c.SummonMaster != null ) )
				{

					ArrayList list = new ArrayList();

					foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
						if ( m != this && m.Player && m.Alive && c.CanBeHarmful( m ) )
							list.Add( m );

					if ( list.Count > 0 )
					{
						Mobile target = (Mobile)list[Utility.Random( list.Count )];

						c.ControlTarget = target; //Fix an exploit where players use multiclients to help pets while the owners are not present.  Bypasses this ability completely.
						c.ControlOrder = OrderType.Attack;
						c.Combatant = target;
						BaseCreature.CannotControl.Add( c );

						Timer.DelayCall( TimeSpan.FromSeconds( 7.5 + ( Utility.RandomDouble() * 2.5 ) ), new TimerStateCallback( StopCounterAttack ), c );
					}
				}
			}
		}

		public void StopCounterAttack( object state )
		{
			BaseCreature c = state as BaseCreature;
			if ( c != null )
			{
				c.Controlled = true; //Give control back to the master.
				c.Combatant = this;
				BaseCreature.CannotControl.Remove( c );
			}
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