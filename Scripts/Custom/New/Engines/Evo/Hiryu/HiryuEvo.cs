using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Xanthos.Evo
{
	[CorpseName( "a mutant corpse" )]
	public class EvoHiryu : BaseEvoMount, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return HiryuEvoSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new HiryuEvoEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( HiryuEvoDust ); }

		public override bool HasBreath{ get{ return true; } }
		public override int BreathRange{ get{ return RangePerception / 2; } }

		public EvoHiryu( string name ) : base( name, 793, 0x3EBB )
		{
		}

		public EvoHiryu( Serial serial ) : base( serial )
		{
		}

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		public override bool SubdueBeforeTame{ get{ return true; } } // Must be beaten into submission

		public override int GetAngerSound()
		{
			return 0x4FF;
		}

		public override int GetIdleSound()
		{
			return 0x4FE;
		}

		public override int GetAttackSound()
		{
			return 0x4FD;
		}

		public override int GetHurtSound()
		{
			return 0x500;
		}

		public override int GetDeathSound()
		{
			return 0x4FC;
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