using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public abstract class BaseElite : BaseCreature
	{
		public BaseElite( AIType aiType ) : this( aiType, FightMode.Closest )
		{
		}

		public BaseElite( AIType aiType, FightMode mode ) : base( aiType, mode, 18, 1, 0.2, 0.4 )
		{
		}

		public BaseElite( Serial serial ) : base( serial )
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

		public virtual bool HasAntiTaming
		{
			get { return false; }
		}

		public bool HasExpired(DamageEntry de)
		{
		       return(de.LastDamage + TimeSpan.FromMinutes(5.0) < DateTime.Now);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			base.OnDamage(amount, from, willKill);

			if (from is BaseCreature)
			{
				BaseCreature c = (BaseCreature)from;
				if (c.Controlled)//&& HasAntiTaming())
				{
					if (c.ControlMaster != null)
					{
						c.ControlTarget = c.ControlMaster;
						c.ControlOrder = OrderType.Attack;
						c.Combatant = c.ControlMaster;
					}
				}
			}

		}
/*
		public override void AggressiveAction( Mobile aggressor )
		{
		       if(aggressor is BaseCreature)
		       {
		       BaseCreature c = (BaseCreature)aggressor;
		       if(c.Controlled )//&& HasAntiTaming())
		       {
		                 if(c.ControlMaster != null)
		                 {
		                        c.Combatant = c.ControlMaster;
		                        Combatant = c.ControlMaster;
		                 }
		       }
		       }
		}
*/

		public void DistributeGold()
		{
			ArrayList toGive = new ArrayList();

			ArrayList list = DamageEntries;

			int totalDamage = 0;
			foreach(DamageEntry de in list)
			{
				if(!HasExpired(de))
				{
					toGive.Add(de);
                    totalDamage += de.DamageGiven;
				}
			}

			foreach(DamageEntry de in toGive)
			{

				int percetageToGive = de.DamageGiven * 100 / totalDamage;
				if(de.Damager.Alive)
				{
				        BankCheck check = new BankCheck(TotalGold * percetageToGive / 100);
				        check.LootType = LootType.Regular;
					de.Damager.AddToBackpack(check);

			        }
				else
					de.Damager.BankBox.DropItem(new BankCheck(TotalGold * percetageToGive / 100));
			}

			this.Backpack.ConsumeTotal(typeof(Gold), (int)(TotalGold*0.9));
		}

		public override bool OnBeforeDeath()
		{
			DistributeGold();

			return base.OnBeforeDeath();
		}
	}
}