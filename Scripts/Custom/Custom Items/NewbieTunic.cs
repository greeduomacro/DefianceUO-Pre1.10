using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;

namespace Server.Items
{
	public enum AllowedSkills
	{
		Anatomy = 1,
		Parry = 5,
		EvalInt = 16,
		Healing = 17,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Archery = 31,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Meditation = 46,
	}

	public class NewbieTunic : Tunic
	{
		private AllowedSkills m_Skill;

		[CommandProperty( AccessLevel.GameMaster )]
		public AllowedSkills Skill{ get{ return m_Skill; } set{ m_Skill = value; } }

		private SkillMod m_SkillMod;

		[Constructable]
		public NewbieTunic() : base( 1148 )
		{
			Weight = 1.0;
			LootType = LootType.Newbied;
			Name = "a newbie tunic";
			m_Skill = AllowedSkills.Anatomy;
		}

		public override void OnSingleClick( Mobile from )
		{
			this.LabelTo( from, Name + " [" + m_Skill.ToString() + "]" );
		}

		public NewbieTunic( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( (int) m_Skill );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Skill = (AllowedSkills)reader.ReadInt();
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				Mobile from = parent as Mobile;
				if ( m_SkillMod != null )
					m_SkillMod.Remove();
				int amount = 50;
				if (from.SkillsCap < from.SkillsTotal + (amount * 10))
					amount = (from.SkillsCap - from.SkillsTotal) / 10;
				m_SkillMod = new DefaultSkillMod( ((SkillName)((int)m_Skill)), true, amount );
				m_SkillMod.ObeyCap = true;
				from.AddSkillMod( m_SkillMod );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( m_SkillMod != null )
				m_SkillMod.Remove();

			m_SkillMod = null;
		}

		public override bool OnDroppedInto( Mobile from, Container target, Point3D p )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedInto( from, target, p );
		}

		public override bool OnDroppedOnto( Mobile from, Item target )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.Parent != from)
				return false;
			return base.OnDroppedOnto( from, target );
		}

		public override bool OnDroppedToMobile( Mobile from, Mobile target )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && target.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToMobile( from, target );
		}

		public override bool OnDroppedToWorld( Mobile from, Point3D p )
		{
			if (from.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.OnDroppedToWorld( from, p );
		}

		public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if (from.AccessLevel < AccessLevel.GameMaster && to.AccessLevel < AccessLevel.GameMaster)
				return false;
			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}

		public override void GetContextMenuEntries( Mobile from, ArrayList list )
		{
			base.GetContextMenuEntries( from, list );
			if ( !from.Alive)
				return;

			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Anatomy ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Parry ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.EvalInt ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Healing ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Magery ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.MagicResist ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Tactics ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Archery ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Swords ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Macing ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Fencing ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Wrestling ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Lumberjacking ) );
			list.Add( new AllowedSkillEntry( from, this, AllowedSkills.Meditation ) );
		}
	}

	public class AllowedSkillEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private NewbieTunic m_Tunic;
		private AllowedSkills m_Skill;

		public AllowedSkillEntry( Mobile from, NewbieTunic tunic, AllowedSkills skill ) : base( 6000 + (int)skill, 4 )
		{
			m_From = from;
			m_Tunic = tunic;
			m_Skill = skill;
		}

		public override void OnClick()
		{
			if ( m_Tunic.Deleted || !m_Tunic.Movable || !m_From.CheckAlive() )
				return;

			m_Tunic.Skill = m_Skill;
		}
	}
}