using System;
using System.Collections;
using Server;

namespace Server.Scripts.Commands
{
	public class GlobalCommandImplementor : BaseCommandImplementor
	{
		public GlobalCommandImplementor()
		{
			Accessors = new string[]{ "Global" };
			SupportRequirement = CommandSupport.Global;
			SupportsConditionals = true;
			AccessLevel = AccessLevel.Administrator;
			Usage = "Global <command> [condition]";
			Description = "Invokes the command on all appropriate objects in the world. Optional condition arguments can further restrict the set of objects.";
		}

		public override void Compile( Mobile from, BaseCommand command, ref string[] args, ref object obj )
		{
			try
			{
				ObjectConditional cond = ObjectConditional.Parse( from, ref args );

				bool items, mobiles;

				if ( !CheckObjectTypes( command, cond, out items, out mobiles ) )
					return;

				ArrayList list = new ArrayList();

                DateTime time;
				if ( items )
				{
                    time = DateTime.Now;
					foreach ( Item item in World.Items.Values )
					{
						if ( cond.CheckCondition( item ) )
							list.Add( item );
					}
                    from.SendMessage("Compiling items took {0:F1} seconds.", (DateTime.Now - time).TotalSeconds);
                }

                if ( mobiles )
				{
                    time = DateTime.Now;
                    foreach (Mobile mob in World.Mobiles.Values)
					{
						if ( cond.CheckCondition( mob ) )
							list.Add( mob );
					}
                    from.SendMessage("Compiling mobiles took {0:F1} seconds.", (DateTime.Now - time).TotalSeconds);
                }

				obj = list;
			}
			catch ( Exception ex )
			{
				from.SendMessage( ex.Message );
			}
		}
	}
}