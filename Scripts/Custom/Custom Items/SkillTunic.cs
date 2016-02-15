using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;

namespace Server.Items
{
	/*public enum AllowedSkills
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
	}*/

	public class SkillTunic : Tunic
	{
		private AllowedSkills m_Skill;

		[CommandProperty( AccessLevel.GameMaster )]
		public AllowedSkills Skill{ get{ return m_Skill; } set{ m_Skill = value; } }

		private SkillMod m_SkillMod;

		[Constructable]
		public SkillTunic() : base( 1148 )
		{
			Weight = 1.0;
			Hue = 2401;
                        LootType = LootType.Newbied;
			Name = "a skill tunic";
			string[] names = Enum.GetNames( typeof(AllowedSkills) );
			m_Skill = (AllowedSkills)Enum.Parse(typeof(AllowedSkills), names[Utility.Random( names.Length )], false );
		}

		public override void OnSingleClick( Mobile from )
		{
			this.LabelTo( from, Name + " [" + m_Skill.ToString() + "]" );
		}

		public SkillTunic( Serial serial ) : base( serial )
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
	}
}