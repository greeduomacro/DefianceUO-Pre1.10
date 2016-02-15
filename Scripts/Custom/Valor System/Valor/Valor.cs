using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Engines.CannedEvil;

namespace Server
{
	public class ValorVirtue
	{
		private static TimeSpan LossDelay = TimeSpan.FromDays( 7.0 ); // delay for valor loss
		private static int sacrifice_activate = 20; // number of points sacrificed for spawn's activation
		private static double chance_valorous_challenge = 0.03; // chance for Valorous Challenge
		private static int sacrifice_valorous_challenge = 10; // number of points sacrificed for Valorous Challenge
		private static double m_dValorAwardPerCreature = 0.06;

		public static void Initialize()
		{
			VirtueGump.Register( 112, new OnVirtueUsed( OnVirtueUsed ) );
		}

		public static void OnVirtueUsed( Mobile from )
		{
			if ( from.Alive )
			{
				from.SendLocalizedMessage( 1054034 ); // Target the Champion Idol of the Champion you wish to challenge!

				from.Target = new InternalTarget();
			}
		}

		public static void CheckAtrophy( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			try
			{
				if ( (pm.LastValorLoss + LossDelay) < DateTime.Now )
				{
					if ( VirtueHelper.Atrophy( from, VirtueName.Valor ) )
						from.SendLocalizedMessage( 1054040 ); // You have lost some Valor.

					VirtueLevel level = VirtueHelper.GetLevel( from, VirtueName.Valor );

					pm.LastValorLoss = DateTime.Now;
				}
			}
			catch
			{
			}
		}

		public static bool CheckValor( ChampionSpawn spawn, Mobile from )
		{
			VirtueLevel level = VirtueHelper.GetLevel( from, VirtueName.Valor );

			if ( spawn != null && VirtueHelper.HasAny( from, VirtueName.Valor ) )
			{
				if ( level >= VirtueLevel.Seeker && spawn.Level < 5 )
					return true;
				else if ( level >= VirtueLevel.Follower && spawn.Level < 10 )
					return true;
				else if ( level >= VirtueLevel.Knight )
					return true;
			}

			return false;
		}

		private static void LowerValor( Mobile from, int value, int current )
		{
			if ( value > current )
				value = current;

			from.Virtues.SetValue( (int)VirtueName.Valor, current - value );
			from.SendLocalizedMessage( 1054040 ); // You have lost some Valor.
		}

		public static void Valor( Mobile from, object targeted )
		{
			if ( !from.CheckAlive() )
				return;

			ChampionIdol targ = targeted as ChampionIdol;

			if ( targ == null )
				return;

			VirtueLevel level = VirtueHelper.GetLevel( from, VirtueName.Valor );

			int current = from.Virtues.GetValue( (int)VirtueName.Valor );

			if ( from.Hidden )
			{
				from.SendLocalizedMessage( 1052015 ); // You cannot do that while hidden.
			}
			else
			{
				if ( targ.Spawn.IsValorUsed )
				{
					from.SendMessage("This spawn has already used with Valor virtue. Wait next session.");
				}
				else
				{
					if ( !targ.Spawn.Active )
					{
						// Try to activate the spawn
						if ( level >= VirtueLevel.Knight )
						{
							from.SendMessage("You successfully activated this spawn!");
							targ.Spawn.Active = true;

							LowerValor( from, sacrifice_activate, current );

							//targ.Spawn.IsValorUsed = true;
						}
						else
						{
							from.SendLocalizedMessage( 1054036 ); // You must be a Knight of Valor to summon the champion's spawn in this manner!
						}
					}
					else
					{
						if ( targ.Spawn.Champion == null )
						{
							// Attempt Valorous Challange
							if ( level >= VirtueLevel.Knight && ( (chance_valorous_challenge >= Utility.RandomDouble() && targ.Spawn.Level >= 15) || targ.Spawn.Level >= 16) )
							{
								from.SendLocalizedMessage( 1054037 ); // Your challenge is heard by the Champion of this region! Beware its wrath!
								targ.Spawn.SpawnChampion();

								LowerValor( from, sacrifice_valorous_challenge, current );

								targ.Spawn.IsValorUsed = true;
							}
							// Attempt to advance the spawn
							else if ( CheckValor( targ.Spawn, from ) )
							{
								from.SendMessage("You successfully advanced this spawn one level closer to the summoning of its champion of evil");
								targ.Spawn.Level += 1;

								LowerValor( from, targ.Spawn.Level, current );

								targ.Spawn.IsValorUsed = true;
							}
							else
							{
								from.SendLocalizedMessage( 1054039 ); // The Champion of this region ignores your challenge. You must further prove your valor.
							}
						}
						else
						{
							from.SendLocalizedMessage( 1054038 ); // The Champion of this region has already been challenged!
						}
					}
				}
			}
		}

		private static bool TryToGiveValor( PlayerMobile pm, int value )
		{
			if ( VirtueHelper.IsHighestPath( pm, VirtueName.Valor ) )
			{
				pm.SendLocalizedMessage( 1054031 ); // You have achieved the highest path in Valor and can no longer gain any further.
			}
			else
			{
				bool gainedPath = false;

				if ( VirtueHelper.Award( pm, VirtueName.Valor, value, ref gainedPath ) )
				{
					if ( gainedPath )
					{
						pm.SendLocalizedMessage( 1054032 ); // You have gained a path in Valor!
					}
					else
					{
						pm.SendLocalizedMessage( 1054030 ); // You have gained in Valor!
					}
					return true;
				}
			}
			return false;
		}

		public static void GiveValor( BaseCreature creature, PlayerMobile pm )
		{
			if ( creature.IsChampionMonster )
			{
				if ( creature.SpawnLevel < 1 )
					creature.SpawnLevel = 1;

				if ( creature.SpawnLevel > 4 )
					creature.SpawnLevel = 4;

				double ValorPoints = (double) ( creature.SpawnLevel * m_dValorAwardPerCreature );

				if ( ValorPoints > 0 )
				{
					pm.ValorGain += ValorPoints;

					if( pm.ValorGain >= 1 )
					{
						TryToGiveValor( pm, 1 );
						pm.ValorGain -= 1;
					}
				}
			}
		}

		public static void GiveValor( BaseChampion champ )
		{
			ArrayList toGive = new ArrayList();

			ArrayList list = champ.Aggressors;
			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if ( info.Attacker.Player && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( info.Attacker ) )
					toGive.Add( info.Attacker );
			}

			list = champ.Aggressed;
			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if ( info.Defender.Player && info.Defender.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( info.Defender ) )
					toGive.Add( info.Defender );
			}

			if ( toGive.Count == 0 )
				return;

			for ( int i = 0; i < toGive.Count; ++i )
			{
				int rand = Utility.Random( toGive.Count );
				object hold = toGive[i];
				toGive[i] = toGive[rand];
				toGive[rand] = hold;
			}

			if ( champ.ChanceToGiveValorPoints >= Utility.RandomDouble() )
			{
				for ( int i = 0; i < toGive.Count; ++i )
					TryToGiveValor( (PlayerMobile)toGive[i % toGive.Count], champ.ValorPoints );
			}
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( 8, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is ChampionIdol )
				{
					Valor( from, targeted );
				}
				else
				{
					from.SendLocalizedMessage( 1054035 ); // You must target a Champion Idol to challenge the Champion's spawn!
				}
			}
		}
	}
}