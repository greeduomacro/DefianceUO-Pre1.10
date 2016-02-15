using System;
using Server;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a charred corpse" )]
	public class fierygargoyle : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Lizardman; } }

		[Constructable]
		public fierygargoyle () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.3 )
		{
			Name = "a fiery gargolye";
			Body = 4;
			BaseSoundID = 372;
			Hue = 1194;

			SetStr( 500, 600 );
			SetDex( 150, 250 );
			SetInt( 500, 750 );

			SetHits(1050, 1250 );

			SetDamage( 10, 20 );

			SetSkill( SkillName.Anatomy, 75.0, 90.1 );
			SetSkill( SkillName.MagicResist, 150.0, 190.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );
			SetSkill( SkillName.DetectHidden, 100.0);
			SetSkill( SkillName.Magery, 39.9, 50.0);
			
			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 110;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 1, 4 ) );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public virtual int DoMoreDamageToPets { get { return 3; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanDestroyObstacles { get { return true; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{	get{ return 0.10; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		

		public override void OnDeath( Container c )
		{
   			if (Utility.Random( 150 ) <  1 )
   			c.DropItem( new EmberTreeStump() );

             		base.OnDeath( c );
             
             		if (Utility.Random( 150 ) <  1 )
   			c.DropItem( new EmberTree() );

             		base.OnDeath( c );
  	 	}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
  		{
  			 if ( from is BaseCreature )
  		 	{
    			BaseCreature bc = (BaseCreature)from;

    			if ( bc.Controlled )
    			damage /= 20;
  		 	}
  		} 


		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 10;
			}
		}
		

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			if ( caster.Body.IsMale )
				reflect = true; // Always reflect if caster isn't female
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster.Body.IsMale )
				scalar = 3; // Male bodies always reflect.. damage scaled 3x
		}
		
		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.10 >= Utility.RandomDouble() ) // 10% chance to counter attacker
				DoCounter( target );
 		}
		public override void OnDamagedBySpell( Mobile attacker )
		{
			base.OnDamagedBySpell( attacker );

			DoCounter( attacker );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );
		}
		
		private void DoCounter( Mobile attacker )
		{
			if ( this.Map == null || ( attacker is BaseCreature && ((BaseCreature)attacker).BardProvoked ) )
				return;

			if ( 0.4 > Utility.RandomDouble() )
			{
				
				Mobile target = null;

				if ( attacker is BaseCreature )
				{
					Mobile m = ((BaseCreature)attacker).GetMaster();

					if ( m != null )
						target = m;
				}

				if ( target == null || !target.InRange( this, 25 ) )
					target = attacker;

				this.Animate( 10, 4, 1, true, false, 0 );

				ArrayList targets = new ArrayList();

				foreach ( Mobile m in target.GetMobilesInRange( 8 ) )
				{
					if ( m == this || !CanBeHarmful( m ) )
						continue;

					if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
						targets.Add( m );
					else if ( m.Player )
						targets.Add( m );
				}

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					DoHarmful( m );

					AOS.Damage( m, this, Utility.RandomMinMax( 15, 25 ), 0, 0, 0, 100, 0, true );

					m.FixedParticles( 0x36BD, 1, 10, 0x1F78, 0xA6, 0, (EffectLayer)255 );
					m.ApplyPoison( this, Poison.Greater );
				}
			}
		}

		public override void OnThink()
		{

				Point3D p = Location;

				double srcSkill = Skills[SkillName.DetectHidden].Value;
				int range = (int)(srcSkill / 10.0);

				if (!CheckSkill(SkillName.DetectHidden, 0.0, 100.0))
				range /= 2;

				if (range > 0 && Map != null)
				{
					IPooledEnumerable inRange = Map.GetMobilesInRange(p, range);

					foreach (Mobile trg in inRange)
					{
						if (trg.Hidden && this != trg)
						{
							double ss = srcSkill + Utility.Random(21) - 10;
							double ts = trg.Skills[SkillName.Hiding].Value + Utility.Random(21) - 10;

							if (AccessLevel >= trg.AccessLevel && (ss >= ts))
							{
								trg.RevealingAction();
								trg.SendLocalizedMessage(500814); // You have been revealed!
							}
						}
					}
					inRange.Free();
				}
		}

		public fierygargoyle( Serial serial ) : base( serial )
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