using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x26C0, 0x26CA )]
	public class UORLance : BaseSword
	{
		public override int OldStrengthReq{ get{ return 80; } }
		public override int OldMinDamage{ get{ return 10; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 20; } }

		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		public override int OldMaxRange { get { return 2; } }


		[Constructable]
		public UORLance() : base( 0x26C0 )
		{
			Name = "a lance";
			Weight = 12.0;
		}

		public UORLance( Serial serial ) : base( serial )
		{
		}

        public override bool OnEquip(Mobile from)
        {
            SkillName sk;

            double swrd = from.Skills[SkillName.Swords].Value;
            double fenc = from.Skills[SkillName.Fencing].Value;
            double mcng = from.Skills[SkillName.Macing].Value;
            double wres = from.Skills[SkillName.Wrestling].Value;
            double arch = from.Skills[SkillName.Archery].Value;
            double val;

            sk = SkillName.Swords;
            val = swrd;

            if (fenc > val) { sk = SkillName.Fencing; val = fenc; }
            if (mcng > val) { sk = SkillName.Macing; val = mcng; }
            if (wres > val) { sk = SkillName.Wrestling; val = wres; }
            if (arch > val) { sk = SkillName.Archery; val = arch; }

            this.Skill = sk;

            return true;
        }

		public override void OnHit( Mobile attacker, Mobile defender )
		{
            base.OnHit(attacker, defender);

            if (defender.Mounted && defender.Mount != null)
            {
                int chance = (attacker.Str + attacker.Dex - defender.VirtualArmor) / 2;
                IMount mt = defender.Mount;
                if (mt.Rider != null && (Utility.Random(100) < chance))
                {
                    mt.Rider = null;
                    defender.SendMessage("You have been dismounted!");
                    attacker.SendMessage("You dismounted your foe!");
                }
            }
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