using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a horde minion corpse" )]
	public class HordeMinion : BaseCreature
	{
		[Constructable]
		public HordeMinion () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a horde minion";
			Body = 776;
			BaseSoundID = 357;

			SetStr( 16, 40 );
			SetDex( 31, 60 );
			SetInt( 11, 25 );
			ControlSlots = 2;
			Tamable = true;
			MinTameSkill = 65.0;

			SetHits( 10, 24 );

			SetDamage( 5, 10 );

			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 0.1, 15.0 );
			SetSkill( SkillName.Wrestling, 25.1, 40.0 );

			Fame = 500;
			Karma = -500;

			VirtualArmor = 18;

			AddItem( new LightSource() );

			PackItem( new Bone( 5 ) );
			PackWeapon( 0, 1 );
			// TODO: Body parts
		}

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 195;
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

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			return PackAnimal.CheckAccess( this, from );
		}

		public override bool CheckNonlocalLift( Mobile from, Item item )
		{
			return PackAnimal.CheckAccess( this, from );
		}

		public override void GetContextMenuEntries( Mobile from, System.Collections.ArrayList list )
		{
			base.GetContextMenuEntries( from, list );

			PackAnimal.GetContextMenuEntries( this, from, list );
		}
		#endregion

		public HordeMinion( Serial serial ) : base( serial )
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