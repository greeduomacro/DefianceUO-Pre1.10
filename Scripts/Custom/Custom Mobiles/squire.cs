using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
  [CorpseName( "the remains a young warrior" )]
  public class Squire : BaseCreature
    {
     private static string GetName()
	{
	switch ( Utility.Random( 6 ) )
		{
			default:
			case 0: return "Lonya";
			case 1: return "Immoni";
			case 2: return "Katherine";
			case 3: return "Mandy";
			case 4: return "Chastity";
			case 5: return "Samantha";
		}
	}

[Constructable]
	public Squire(): base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
	  {
		Name = GetName();
		Body = 401;
		BaseSoundID = 0x31B;
	     	Hue = Utility.RandomSkinHue();
		SpeechHue = 1170;
		CanHearGhosts = true;

		Summoned = false;
		SummonMaster = null;

		SetStr( 150 );
		SetDex( 110 );
		SetInt( 200 );
 		SetHits( 215 );
		SetStam( 110 );
		SetMana( 100 );

		SetDamage( 15, 45 );

		SetDamageType( ResistanceType.Physical, 100 );

		SetResistance( ResistanceType.Physical, 70, 90 );
		SetResistance( ResistanceType.Fire, 80, 85 );
		SetResistance( ResistanceType.Poison, 95, 100 );
		SetResistance( ResistanceType.Energy, 25, 30 );

		SetSkill( SkillName.Swords, 25.0, 35.0 );
		SetSkill( SkillName.Parry, 170.1, 175.0 );
		SetSkill( SkillName.Tactics, 150.0 );

		AddItem( new TwoPigTails( Utility.RandomHairHue() ) );
		AddItem( new NeonWep() );
		AddItem( new LongPants( Utility.RandomNeutralHue() ) );
                AddItem( new FancyShirt( Utility.RandomNeutralHue() ) );
                AddItem( new Boots( Utility.RandomNeutralHue() ) );
	        AddItem( new Cloak( Utility.RandomNeutralHue() ) );
	        AddItem( new BodySash( Utility.RandomNeutralHue() ) );

		new Horse().Rider = this;
		ControlSlots = 2;

		Container pack = Backpack;
		if ( pack != null )
		pack.Delete();
		pack = new StrongBackpack();
		pack.Movable = false;
		AddItem( pack );
	}


public override bool BardImmune{ get{ return true; } }
public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

public override void OnSpeech( SpeechEventArgs e )
	{
	base.OnSpeech( e );
	Mobile from = e.Mobile;
	string message;
	RoastPig roastpork = new RoastPig();
		if ( from.InRange( this, 4 ))
		{
			if (e.Speech.ToLower() == "slave" || e.Speech.ToLower() == "servant" || e.Speech.ToLower() == "squire")
			{
       				message = "How might I serve thee, My Lord?";
       				this.Say( message );
			}
			else if (e.Speech.ToLower() == "follow")
			{
        			message = "As you command.";
        			this.Say( message );
			}
			else if (e.Speech.ToLower() == "tithe" || e.Speech.ToLower() == "donate")
			{
				from.SendGump( new TithingGump( from, 0 ) );
        			message = "I admire your faith.";
        			this.Say( message );
			}
			else if (e.Speech.ToLower() == "make camp" || e.Speech.ToLower() == "setup camp")
			{
				new Campfire().MoveToWorld( this.Location, this.Map );
				message = "I am securing thy camp now.";
			        this.Say( message );
			}
			else if (e.Speech.ToLower() == "master" || e.Speech.ToLower() == "who's your daddy")
			{
				message = String.Format( from.Name );
				this.Say( message );
				this.Say( "is my master");

			}
			else if (e.Speech.ToLower() == "food" || e.Speech.ToLower() == "cook" || e.Speech.ToLower() == "dinner")
			{
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( Gold ), 100 ) )
					{
						from.AddToBackpack( roastpork );
			                    	message = "Dinner is served, my lord.";
                    				this.Say( message );
					}
					else
					{
						message = "But we haven't the money to buy food, Lord.";
						this.Say( message );
					}
			}
			else if (e.Speech.ToLower() == "reg" || e.Speech.ToLower() == "reagent")
			{
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( Gold ), 1000 ) )
					{
						from.AddToBackpack( new BagOfReagents( 75 ));
						message = "Your total comes to 1000gp.";
						this.Say( message );
					}
					else
					{
						message = "I'm sorry my Lord, but you haven't the 1000gp for that";
						this.Say( message );
					}

			}










			else if (e.Speech.ToLower() == "smelt" || e.Speech.ToLower() == "ingot")
			{
				int i = 0;
				//Iron
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( IronOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new IronIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( IronOre ), 1 ));
					}
				i = 0;
				//DullCopper
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( DullCopperOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new DullCopperIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( DullCopperOre ), 1 ));
					}
				i = 0;
				//ShadowIron
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( ShadowIronOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new ShadowIronIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( ShadowIronOre ), 1 ));
					}
				i = 0;
				//Copper
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( CopperOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new CopperIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( CopperOre ), 1 ));
					}
				i = 0;
				//Bronze
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( BronzeOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new BronzeIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( BronzeOre ), 1 ));
					}
				i = 0;
				//Gold
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( GoldOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new GoldIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( GoldOre ), 1 ));
					}
				i = 0;
				//Agapite
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( AgapiteOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new AgapiteIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( AgapiteOre ), 1 ));
					}
				i = 0;
				//Verite
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( VeriteOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new VeriteIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( VeriteOre ), 1 ));
					}
				i = 0;
				//Valorite
				if ( from.Backpack != null && from.Backpack.ConsumeTotal( typeof( ValoriteOre ), 1 ) )
					{
						do
						{
							from.AddToBackpack( new ValoriteIngot( 2 ));
							i = ( i + 1 );
						}
						while( from.Backpack.ConsumeTotal( typeof( ValoriteOre ), 1 ));
					}
				i = 0;

				// To add custom ore types just copy the above ^^^
			}
			else if (e.Speech.ToLower() == "res" || e.Speech.ToLower() == "resurrect")
			{
				if ( !from.Alive && from.Map != null && from.Map.CanFit( from.Location, 16, false, false ) )
					{
						from.PlaySound( 0x214 );
						from.FixedEffect( 0x376A, 10, 16 );
						from.SendGump( new ResurrectGump( from ) );
						message = "I shall summon thee back from the abyss, my lord";
						this.Say( message );
					}
					else
					{
					from.SendLocalizedMessage( 502391 );
					}
		}
	}}

	public override void OnDamage( int amount, Mobile from, bool willKill )
	{
		if ( from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random( 100 ) )
		{
			string[] toSay = new string[]
				{
					"{0}!!  My Master Shall Avenge Me!",
					"{0}!!  I Shall fight for my master's honor!",
					"{0}!!  It's hardly worth dulling my blade on the likes of you!",
					"{0}!!  My sword shall bring justice, even after my death."
				};
				this.Say( true, String.Format( toSay[Utility.Random( toSay.Length )], from.Name ) );
		}
		base.OnDamage( amount, from, willKill );
		}
		private DateTime m_NextPickup;
		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.Now < m_NextPickup )
				return;

			m_NextPickup = DateTime.Now + TimeSpan.FromSeconds( 2.5 + (2.5 * Utility.RandomDouble()) );

			Container pack = this.Backpack;

			if ( pack == null )
				return;

			ArrayList list = new ArrayList();

			foreach ( Item item in this.GetItemsInRange( 2 ) )
			{
				if ( item.Movable )
					list.Add( item );
			}

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = (Item)list[i];

				if ( !pack.CheckHold( this, item, false, true ) )
					return;

				bool rejected;
				LRReason reject;

				NextActionTime = DateTime.Now;

				Lift( item, item.Amount, out rejected, out reject );

				if ( rejected )
					continue;

				Drop( this, Point3D.Zero );
			}
		}

		#region Pack Animal Methods
		public override bool OnBeforeDeath()
		{

			if ( !base.OnBeforeDeath() )
				return false;

			PackAnimal.CombineBackpacks( this );

			return true;

		}

		public override bool IsSnoop( Mobile from )
		{
			if ( PackAnimal.CheckAccess( this, from ) )
				return false;

			return base.IsSnoop( from );
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			if ( CheckFeed( from, item ) )
				return true;

			if ( PackAnimal.CheckAccess( this, from ) )
			{
				AddToBackpack( item );
				return true;
			}

			return base.OnDragDrop( from, item );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			attacker.Criminal = true;
			attacker.Kills = 5;
			attacker.Title = "the Coward";
			base.OnGotMeleeAttack( attacker );
			if ( attacker == this.ControlMaster)
				this.IsBonded = false;
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			return PackAnimal.CheckAccess( this, from );
		}

		public override bool CheckNonlocalLift( Mobile from, Item item )
		{
			return PackAnimal.CheckAccess( this, from );
		}

		public override void OnDoubleClick( Mobile from )
		{
			PackAnimal.TryPackOpen( this, from );
		}

		public override void GetContextMenuEntries( Mobile from, System.Collections.ArrayList list )
		{
			base.GetContextMenuEntries( from, list );

			PackAnimal.GetContextMenuEntries( this, from, list );
		}
		#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

[CorpseName( "a Vengefull Spirit" )]
	public class VengefullSpirit : BaseCreature
	{
		[Constructable]
		public VengefullSpirit () : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a Vengefull Spirit";
			Body = 403;
			BaseSoundID = 0x375;

			SetStr( 488, 620 );
			SetDex( 121, 170 );
			SetInt( 498, 657 );

			SetHits( 150, 350 );
			SpeechHue = 1161;
			SetDamage( 25, 45 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 200.5, 250.0 );
			SetSkill( SkillName.Tactics, 118.0, 190.0 );
			SetSkill( SkillName.Wrestling, 105.9, 190.0 );
			SetSkill( SkillName.Poisoning, 80.1, 100.0 );

			Fame = 24000;
			Karma = 24000;
			AddItem ( new HoodedShroudOfShadows() );
			VirtualArmor = 50;
			PackGold( 1000, 7000 );
			PackMagicItems( 6, 8 );
			PackMagicItems( 5, 8 );
			PackMagicItems( 8, 8 );
			PackMagicItems( 6, 8 );

		}
		public override bool ShowFameTitle{ get{ return false; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return (0.2 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel your self being drawn into death." );

				int toDrain = Utility.RandomMinMax( 10, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			if ( Utility.RandomDouble() >= 0.8 )
				this.Say( true, String.Format( "The Sword you weild is tainted with innocent blood. Now you shall die.") );
			if ( 0.2 >= Utility.RandomDouble() )
				DrainLife();
			if ( 0.2 >= Utility.RandomDouble() )
				SpawnSkeleton( defender );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}

		public void SpawnSkeleton( Mobile from )
		{
			BaseCreature skele;
			skele = new Skeleton();
			skele.Map = from.Map;
			skele.Location = from.Location;
			skele.Combatant = from;
		}

		public VengefullSpirit( Serial serial ) : base( serial )
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class NeonWep : BaseSword
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ShadowStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int AosStrengthReq{ get{ return 75; } }
		public override int AosMinDamage{ get{ return 25; } }
		public override int AosMaxDamage{ get{ return 35; } }
		public override int AosSpeed{ get{ return 65; } }

		public override int OldStrengthReq{ get{ return 75; } }
		public override int OldMinDamage{ get{ return 25; } }
		public override int OldMaxDamage{ get{ return 30; } }
		public override int OldSpeed{ get{ return 45; } }

		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

    		private static int GetNeonHue()
    		{
			switch ( Utility.Random( 4 ) )
			{
				default:
				case 0: return 1170;
				case 1: return 1370;
				case 2: return 1161;
				case 3: return 1160;
			}
		}


		[Constructable]
		public NeonWep() : base( 0xF5E )
		{
			this.Weight = 0.0;
			this.Hue = GetNeonHue();
			this.Name = "A Neon Sword";

			if (this.Hue == 1170)
				{
					this.WeaponAttributes.HitLightning = 50;
					this.WeaponAttributes.HitEnergyArea = 25;
					this.Name = "the Squire's Blade of Power";
					this.Attributes.AttackChance = 35;
				}
				else if (this.Hue == 1370)
				{
					this.WeaponAttributes.HitPoisonArea = 65;
					this.Name = "the Squire's Toxic Blade" ;
					this.Attributes.AttackChance = 35;
				}
				else if (this.Hue == 1161)
				{
					this.Attributes.SpellChanneling = 1;
					this.Attributes.CastSpeed = 2;
					this.Attributes.CastRecovery = 2;
					this.Attributes.RegenMana = 8;
					this.WeaponAttributes.MageWeapon = 1;
					this.Name = "the Squires Blade of Sorcery";
				}
				else if (this.Hue == 1160)
				{
					this.WeaponAttributes.HitColdArea = 65;
					this.Attributes.AttackChance = 35;
					this.Name = "the Squire's Freezing Blade";
				}
		}

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			if ( Utility.RandomDouble() >= 0.9 )
				if ( Utility.RandomDouble() >= 0.5 )
					if ( attacker.Criminal )
						Spawnhell( attacker );
			base.OnHit( attacker, defender );
		}

		public override void OnDoubleClick( Mobile from )
		{
		}

		public void Spawnhell( Mobile from )
		{
			BaseCreature hell;
			hell = new VengefullSpirit();
			hell.Map = from.Map;
			hell.Location = from.Location;
			hell.Combatant = from;
		}

		public NeonWep( Serial serial ) : base( serial )
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class SquireBrace : GoldBracelet
	{
		public override int ArtifactRarity{ get{ return 11; } }

		[Constructable]
		public SquireBrace()
		{
			Name = "a Bracelet of Squire Summoning";
			Hue = 1373;
		}



		public override void OnDoubleClick( Mobile from )
		{
			if ( this.Hue != 1170 )
			{
				//SpellHelper.Summon( new Squire(), from, 0x217, TimeSpan.FromDays( 10.0 ), false, false );
				MakeSquire( from );
				this.Hue = 1170;
			}

		}


		public void MakeSquire( Mobile from )
		{
			BaseCreature newsquire;
			newsquire = new Squire();
			newsquire.Map = from.Map;
			newsquire.Location = from.Location;
			newsquire.Controlled = true;
			newsquire.ControlMaster = from;
			newsquire.IsBonded = true;
			newsquire.ControlOrder = OrderType.Follow;
		}

		public SquireBrace( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


		public Squire( Serial serial ) : base( serial )
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