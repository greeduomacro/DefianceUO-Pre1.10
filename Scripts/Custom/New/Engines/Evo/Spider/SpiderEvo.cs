using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Xanthos.Evo
{
	[CorpseName( "a spider corpse" )]
	public class EvoSpider : BaseEvoMount, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return EvoSpiderSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new EvoSpiderEvoEgg();
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( EvoSpiderEvoDust ); }

		public override bool HasBreath{ get{ return false; } }
		public override int BreathRange{ get{ return RangePerception / 2; } }

		public EvoSpider( string name ) : base( name, 793, 0x3EBB )
		{
		}

		public EvoSpider( Serial serial ) : base( serial )
		{
		}

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		public override bool SubdueBeforeTame{ get{ return true; } } // Must be beaten into submission

		public override int GetAngerSound()
		{
			return 0x27D;
		}

		public override int GetIdleSound()
		{
			return 0x493;
		}

		public override int GetAttackSound()
		{
			return 0x25A;
		}

		public override int GetHurtSound()
		{
			return 0x288;
		}

		public override int GetDeathSound()
		{
			return 0x284;
		}

        // Part of the stamina System (StamSystem.cs)
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (StamSystem.active && StamSystem.LichSteedFeedBonus != 0 && /* stam system and lichbonus active */
                this.Stam < this.StamMax &&                                /* lichsteed has not full stamina */
                dropped != null && dropped is RawRibs &&                   /* item is RawRibs */
                this.Aggressed.Count == 0 && this.Aggressors.Count == 0)   /* lichsteed is not in a fight */
            {
                // give the stamina bonus
                this.Stam = (this.Stam + StamSystem.LichSteedFeedBonus >= this.StamMax)
                    ? this.StamMax : this.Stam + StamSystem.LichSteedFeedBonus;

                // send the owner a noticication
                if (this.ControlMaster != null && this.ControlMaster.CanSee(this))
                    this.ControlMaster.SendMessage("Your pet feels regenerated.");
            }
            return base.OnDragDrop(from, dropped);
        }

		public override void OnDoubleClick( Mobile from )
		{
			return;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}