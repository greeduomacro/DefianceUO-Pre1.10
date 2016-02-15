using System;
using System.Collections;
using Server.Engines.Craft;
using Server.Network;
using Server.Targeting;
using Server.Engines.Poker;
using Server.Mobiles;
using Server.Regions;
using Server.Spells;
using Server.Spells.Third;

using Server.Logging; //buguse logging
using Server.Scripts.Commands; //buguse logging

namespace Server.Items
{
	public enum GemType
	{
		None,
		StarSapphire,
		Emerald,
		Sapphire,
		Ruby,
		Citrine,
		Amethyst,
		Tourmaline,
		Amber,
		Diamond
	}

	public abstract class BaseJewel : Item, ICraftable
	{
		private AosAttributes m_AosAttributes;
		private AosElementAttributes m_AosResistances;
		private AosSkillBonuses m_AosSkillBonuses;
		private CraftResource m_Resource;
		private GemType m_GemType;

		// Added for Ability Charges
		private JewelChargedAbility m_ChargedAbility;
		private int m_AbilityCharges;
		private bool m_Identified;

		[CommandProperty(AccessLevel.GameMaster)]
		public JewelChargedAbility ChargedAbility
		{
			get { return m_ChargedAbility; }
			set { m_ChargedAbility = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int AbilityCharges
		{
			get { return m_AbilityCharges; }
			set { m_AbilityCharges = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Identified
		{
			get { return m_Identified; }
			set { m_Identified = value; InvalidateProperties(); }
		}

		public static string GetGemType( GemType type )
		{
			switch (type)
			{
				case GemType.StarSapphire: return "star sapphire";
				case GemType.Emerald: return "emerald";
				case GemType.Sapphire: return "sapphire";
				case GemType.Ruby: return "ruby";
				case GemType.Citrine: return "citrine";
				case GemType.Amethyst: return "amethyst";
				case GemType.Tourmaline: return "tourmaline";
				case GemType.Amber: return "amber";
				case GemType.Diamond: return "diamond";
				default: return "";
			}
		}

		public static string GetChargedAbilityName(JewelChargedAbility ability)
		{
			switch (ability)
			{
				case JewelChargedAbility.Cunning: return "cunning";
				case JewelChargedAbility.Strength: return "strength";
				case JewelChargedAbility.Agility: return "agility";
				case JewelChargedAbility.Bless: return "blessing";
				case JewelChargedAbility.Invisibility: return "invisibility";
				case JewelChargedAbility.Teleport: return "teleportation";
				default: return "";
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get{ return m_AosResistances; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosSkillBonuses SkillBonuses
		{
			get{ return m_AosSkillBonuses; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public GemType GemType
		{
			get{ return m_GemType; }
			set{ m_GemType = value; InvalidateProperties(); }
		}

		public override int PhysicalResistance{ get{ return m_AosResistances.Physical; } }
		public override int FireResistance{ get{ return m_AosResistances.Fire; } }
		public override int ColdResistance{ get{ return m_AosResistances.Cold; } }
		public override int PoisonResistance{ get{ return m_AosResistances.Poison; } }
		public override int EnergyResistance{ get{ return m_AosResistances.Energy; } }
		public virtual int BaseGemTypeNumber{ get{ return 0; } }

		public override int LabelNumber
		{
			get
			{
				if ( m_GemType == GemType.None )
					return base.LabelNumber;

				return BaseGemTypeNumber + (int)m_GemType - 1;
			}
		}

		public virtual int ArtifactRarity{ get{ return 0; } }

		public BaseJewel( int itemID, Layer layer ) : base( itemID )
		{
			m_AosAttributes = new AosAttributes( this );
			m_AosResistances = new AosElementAttributes( this );
			m_AosSkillBonuses = new AosSkillBonuses( this );
			m_Resource = CraftResource.Iron;
			m_GemType = GemType.None;

			Layer = layer;
		}

		private EatChargesTimer m_EatChargesTimer;
		private class EatChargesTimer : Timer
		{
			BaseJewel m_Jewel;
			public EatChargesTimer(BaseJewel jewel)
				: base(TimeSpan.FromSeconds(120.0), TimeSpan.FromSeconds(120.0))
			{
				m_Jewel = jewel;
				Start();
			}

			protected override void OnTick()
			{
				if (m_Jewel == null || m_Jewel.Deleted || m_Jewel.Parent == null)
				{
					Stop();
					return;
				}
				else if (m_Jewel.AbilityCharges <= 0 || m_Jewel.ChargedAbility == JewelChargedAbility.Regular)
				{
					if (m_Jewel.Parent is Mobile)
					{
						Mobile parent = (Mobile)m_Jewel.Parent;
						parent.RemoveItem(m_Jewel);
						parent.AddItem(m_Jewel);
					}
					Stop();
					return;
				}
				else
				{
					m_Jewel.AbilityCharges--;
				}
			}
		}

		public override void OnAdded( object parent )
		{
			/*if ( Core.AOS && parent is Mobile )
			{
				Mobile from = (Mobile)parent;

				m_AosSkillBonuses.AddTo( from );

				int strBonus = m_AosAttributes.BonusStr;
				int dexBonus = m_AosAttributes.BonusDex;
				int intBonus = m_AosAttributes.BonusInt;

				if ( strBonus != 0 || dexBonus != 0 || intBonus != 0 )
				{
					string modName = this.Serial.ToString();

					if ( strBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

					if ( dexBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

					if ( intBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
				}
				from.CheckStatTimers();
			}
			else*/ if (m_ChargedAbility != JewelChargedAbility.Regular && m_ChargedAbility != JewelChargedAbility.Teleport && m_AbilityCharges > 0 && parent is Mobile)
			{
				Mobile from = (Mobile)parent;

				if ( m_ChargedAbility == JewelChargedAbility.Invisibility )
					from.Hidden = true;
				else
				{
					int strBonus = 0;
					int dexBonus = 0;
					int intBonus = 0;

					switch (m_ChargedAbility)
					{
						case JewelChargedAbility.Cunning: intBonus = 10; break;
						case JewelChargedAbility.Strength: strBonus = 10; break;
						case JewelChargedAbility.Agility: dexBonus = 10; break;
						case JewelChargedAbility.Bless: intBonus = strBonus = dexBonus = 10; break;
					}

					if (strBonus != 0 || dexBonus != 0 || intBonus != 0)
					{
						string modName = this.Serial.ToString();

						if (strBonus != 0)
							from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

						if (dexBonus != 0)
							from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

						if (intBonus != 0)
							from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
					}
				}

				m_AbilityCharges--;

				m_EatChargesTimer = new EatChargesTimer(this);
			}
		}

		public override void OnRemoved( object parent )
		{
/*
			if ( Core.AOS && parent is Mobile )
			{
				Mobile from = (Mobile)parent;

				m_AosSkillBonuses.Remove();

				string modName = this.Serial.ToString();

				from.RemoveStatMod( modName + "Str" );
				from.RemoveStatMod( modName + "Dex" );
				from.RemoveStatMod( modName + "Int" );

				from.CheckStatTimers();
			}
			else*/ if (m_ChargedAbility != JewelChargedAbility.Regular && m_ChargedAbility != JewelChargedAbility.Teleport && parent is Mobile)
			{
				Mobile from = (Mobile)parent;

				if (m_ChargedAbility == JewelChargedAbility.Invisibility)
					from.Hidden = false;
				else
				{
					string modName = this.Serial.ToString();

					from.RemoveStatMod(modName + "Str");
					from.RemoveStatMod(modName + "Dex");
					from.RemoveStatMod(modName + "Int");
				}

				if (m_EatChargesTimer != null)
					m_EatChargesTimer.Stop();
				m_EatChargesTimer = null;
			}
		}

		public BaseJewel( Serial serial ) : base( serial )
		{
		}

		private static Mobile DummyCaster
		{
			get
			{
				Mobile dummy = new Mobile();
				dummy.Skills[SkillName.Magery].Base = 75.0;
				dummy.Skills[SkillName.EvalInt].Base = 75.0;
				return dummy;
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if ( Parent == from && m_ChargedAbility == JewelChargedAbility.Teleport && m_AbilityCharges > 0 )
				from.Target = new TeleportTarget(this, from);
		}

		public class TeleportTarget : Target
		{
			private BaseJewel m_Owner;
			private Mobile m_Mobile;

			public TeleportTarget(BaseJewel owner, Mobile mobile)
				: base(12, true, TargetFlags.None)
			{
				m_Owner = owner;
				m_Mobile = mobile;
			}

			protected override void OnTarget(Mobile m, object o)
			{
				IPoint3D p = o as IPoint3D;

				if ( p == null )
					return;

				IPoint3D orig = p;
				Point3D to = new Point3D( p );
				Map map = m_Mobile.Map;

				SpellHelper.GetSurfaceTop(ref p);

				CustomRegion customregion = m.Region as CustomRegion;
				CustomRegion tregion = CustomRegion.Find( to, map ) as CustomRegion;

				if ( m_Owner.AbilityCharges <= 0 )
					m_Mobile.SendMessage( "This is out of charges." );
                else if (o is Item && ((Item)o).Parent != null)
                {
                    GeneralLogging.WriteLine("TeleportRingBuguse",
                        "{0} ({1}/{2} hits, criminal: {3}) tried to teleport target an item inside container {4} using item {5}.",
                        m, m.Hits, m.HitsMax, m.Criminal, ((Item)o).Parent, m_Owner);
                    m.PrivateOverheadMessage(MessageType.Regular, 0x26, false, "This incident has been logged.", m.NetState);
                    m_Owner.Delete();
                    return;
                }
                else if (customregion != null && customregion.Controller != null && customregion.Controller.IsRestrictedSpell(typeof(TeleportSpell)))
					m_Mobile.SendMessage( "You cannot teleport here." );
				else if ( tregion != null && tregion.Controller != null && tregion.Controller.IsRestrictedSpell( typeof( TeleportSpell ) ) )
					m_Mobile.SendMessage( "You cannot teleport there." );
				else if ( PokerDealer.IsPokerPlayer(m_Mobile) >= 0 )
					m_Mobile.SendMessage( "You cannot travel while playing poker." );
				else if ( m_Mobile is PlayerMobile && (((PlayerMobile)m_Mobile).LastTeleTime + TimeSpan.FromSeconds( 3.0 )) > DateTime.Now )
					m_Mobile.SendMessage( "You must wait 3 seconds before using this item again." );
				else if ( Server.Factions.Sigil.ExistsOn(m_Mobile) )
					m_Mobile.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
				else if ( Server.Misc.WeightOverloading.IsOverloaded(m_Mobile) )
					m_Mobile.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
				else if ( !SpellHelper.CheckTravel(m_Mobile, TravelCheckType.TeleportFrom) )
				{
				}
				else if ( !SpellHelper.CheckTravel(m_Mobile, map, to, TravelCheckType.TeleportTo) )
				{
				}
				//Al: Fix for Teleport on SnowPile. Has to be done here because
				//      CanSpawnMobile is a core function.
				else if ( o is SnowPile || o is SnowEventPile )
					m_Mobile.SendLocalizedMessage(501942); // That location is blocked.
				else if ( map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z) )
					m_Mobile.SendLocalizedMessage(501942); // That location is blocked.
				else if ( SpellHelper.CheckMulti(new Point3D(p), map) )
					m_Mobile.SendLocalizedMessage(501942); // That location is blocked.
				else if ( m_Mobile.Alive && m_Mobile.Items.Contains(m_Owner) )
				{
					Server.Spells.SpellHelper.Turn(m_Mobile, orig);

					Point3D from = m.Location;

					m.Location = to;
					m.ProcessDelta();

					if ( m.Player )
					{
						Effects.SendLocationParticles(EffectItem.Create(from, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
						Effects.SendLocationParticles(EffectItem.Create(to, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
					}
					else
						m.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);

					m.PlaySound(0x1FE);

					if( m is PlayerMobile )
					   ((PlayerMobile)m).LastTeleTime = DateTime.Now;

					m_Owner.AbilityCharges--;
				}
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			ArrayList attrs = new ArrayList();

			if (DisplayLootType)
			{
				if (LootType == LootType.Blessed)
					attrs.Add(new EquipInfoAttribute(1038021)); // blessed
				else if (LootType == LootType.Cursed)
					attrs.Add(new EquipInfoAttribute(1049643)); // cursed
			}

			string name = Name;
			bool hasability = ( m_ChargedAbility != JewelChargedAbility.Regular ) && ( m_AbilityCharges > 0 );

			if ( !m_Identified && hasability )
				attrs.Add(new EquipInfoAttribute(1038000)); // Unidentified
			else if ( String.IsNullOrEmpty( Name ) )
			{
				string gemtype = GetGemType( m_GemType );
				string chargeability = GetChargedAbilityName( m_ChargedAbility );
				name = String.Format( "{0}{1}{2}", String.IsNullOrEmpty( gemtype ) ? "" : String.Format( "{0} ", gemtype ), ItemData.Name.ToLower(), hasability ? String.Format( " of {0}: {1}", chargeability, m_AbilityCharges ) : "" );
			}

			this.LabelTo(from, name);
			int number = 1041000;

			EquipmentInfo eqInfo = new EquipmentInfo(number, null, false, (EquipInfoAttribute[])attrs.ToArray(typeof(EquipInfoAttribute)));
			from.Send(new DisplayEquipmentInfo(this, eqInfo));
		}
/*
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			m_AosSkillBonuses.GetProperties( list );

			int prop;

			if ( (prop = ArtifactRarity) > 0 )
				list.Add( 1061078, prop.ToString() ); // artifact rarity ~1_val~

			if ( (prop = m_AosAttributes.WeaponDamage) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( (prop = m_AosAttributes.DefendChance) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusDex) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( (prop = m_AosAttributes.EnhancePotions) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( (prop = m_AosAttributes.CastRecovery) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( (prop = m_AosAttributes.CastSpeed) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( (prop = m_AosAttributes.AttackChance) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusHits) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( (prop = m_AosAttributes.BonusInt) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( (prop = m_AosAttributes.LowerManaCost) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( (prop = m_AosAttributes.LowerRegCost) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( (prop = m_AosAttributes.Luck) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( (prop = m_AosAttributes.BonusMana) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( (prop = m_AosAttributes.RegenMana) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( (prop = m_AosAttributes.NightSight) != 0 )
				list.Add( 1060441 ); // night sight

			if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( (prop = m_AosAttributes.RegenStam) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( (prop = m_AosAttributes.RegenHits) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( (prop = m_AosAttributes.SpellDamage) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusStam) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( (prop = m_AosAttributes.BonusStr) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( (prop = m_AosAttributes.WeaponSpeed) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

			base.AddResistanceProperties( list );
		}
*/
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write(m_Identified); // version 3
			writer.Write((int)m_ChargedAbility); // version 3
			writer.Write(m_AbilityCharges); // version3

			writer.WriteEncodedInt( (int) m_Resource );
			writer.WriteEncodedInt( (int) m_GemType );

			m_AosAttributes.Serialize( writer );
			m_AosResistances.Serialize( writer );
			m_AosSkillBonuses.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				{
					m_Identified = reader.ReadBool();
					m_ChargedAbility = (JewelChargedAbility)reader.ReadInt();
					m_AbilityCharges = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Resource = (CraftResource)reader.ReadEncodedInt();
					m_GemType = (GemType)reader.ReadEncodedInt();

					goto case 1;
				}
				case 1:
				{
					m_AosAttributes = new AosAttributes( this, reader );
					m_AosResistances = new AosElementAttributes( this, reader );
					m_AosSkillBonuses = new AosSkillBonuses( this, reader );

					if ( Core.AOS && Parent is Mobile )
						m_AosSkillBonuses.AddTo( (Mobile)Parent );

					int strBonus = m_AosAttributes.BonusStr;
					int dexBonus = m_AosAttributes.BonusDex;
					int intBonus = m_AosAttributes.BonusInt;

					if ( Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
					{
						Mobile m = (Mobile)Parent;

						string modName = Serial.ToString();

						if ( strBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

						if ( dexBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

						if ( intBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
					}

					if ( Parent is Mobile )
						((Mobile)Parent).CheckStatTimers();

					break;
				}
				case 0:
				{
					m_AosAttributes = new AosAttributes( this );
					m_AosResistances = new AosElementAttributes( this );
					m_AosSkillBonuses = new AosSkillBonuses( this );

					break;
				}
			}

			if ( version < 2 )
			{
				m_Resource = CraftResource.Iron;
				m_GemType = GemType.None;
			}
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			Resource = CraftResources.GetFromType( resourceType );

			if ( 1 < craftItem.Ressources.Count )
			{
				resourceType = craftItem.Ressources.GetAt( 1 ).ItemType;

				if ( resourceType == typeof( StarSapphire ) )
					GemType = GemType.StarSapphire;
				else if ( resourceType == typeof( Emerald ) )
					GemType = GemType.Emerald;
				else if ( resourceType == typeof( Sapphire ) )
					GemType = GemType.Sapphire;
				else if ( resourceType == typeof( Ruby ) )
					GemType = GemType.Ruby;
				else if ( resourceType == typeof( Citrine ) )
					GemType = GemType.Citrine;
				else if ( resourceType == typeof( Amethyst ) )
					GemType = GemType.Amethyst;
				else if ( resourceType == typeof( Tourmaline ) )
					GemType = GemType.Tourmaline;
				else if ( resourceType == typeof( Amber ) )
					GemType = GemType.Amber;
				else if ( resourceType == typeof( Diamond ) )
					GemType = GemType.Diamond;
			}

			return 1;
		}

		#endregion
	}
}