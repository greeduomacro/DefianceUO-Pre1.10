//al@2007-11-08
using System;
using System.Collections;
using Server;

namespace Server.Misc
{
    public delegate void TaskSchedulerAction();

    class TaskScheduler
    {
        public const bool ENABLED = true;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ArrayList ms_tasks = new ArrayList();

        public static void Initialize()
        {
            log.Info(String.Format("initializing, scheduler {0}.", ENABLED ? "enabled" : "disabled"));
            if (ENABLED)
            {
                EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
                Server.Commands.Register("TaskScheduler", AccessLevel.Administrator, new CommandEventHandler(TaskScheduler_OnCommand));
            }
        }

        [Usage("TaskScheduler")]
        [Description("Displays information about scheduled tasks.")]
        private static void TaskScheduler_OnCommand(CommandEventArgs e)
        {
            if (ms_tasks == null) return;
            e.Mobile.SendMessage("Scheduled tasks:\n");
            foreach (TaskSchedulerTask t in ms_tasks)
                e.Mobile.SendMessage(t.ToString()+"\n");
        }

        public static void EventSink_WorldSave(WorldSaveEventArgs args)
        {
            TaskSchedulerTask executedTask = null;
            foreach (TaskSchedulerTask task in ms_tasks)
            {
                /*
                 * Stop after one overdue task.
                 * This way, only one task per worldsave is executed.
                 * However, in some situations, tasks may encounter starvation.
                 */
                log.Debug("executing overdue tasks.");
                if (task.TryExecute())
                {
                    executedTask = task;
                    break;
                }
            }
            //Move executed task to the end of the ArrayList
            if (executedTask != null)
            {
                ms_tasks.Remove(executedTask);
                ms_tasks.Add(executedTask);
            }
        }

        public static void RegisterTask(String name, TimeSpan frequency, TaskSchedulerAction action)
        {
            ms_tasks.Add(new TaskSchedulerTask(name, frequency, action));
            log.Info(String.Format("new task '{0}' registered. Frequency: {1}", name, frequency));
        }
    }

    class TaskSchedulerTask : IComparable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        String m_name;
        TimeSpan m_frequency;
        DateTime m_lastExecution;
        TimeSpan m_lastExecutionTime;
        TaskSchedulerAction m_action;

        public DateTime LastExecution
        {
            get { return m_lastExecution; }
        }

        public TimeSpan Frequency
        {
            get { return m_frequency; }
        }

        public bool TryExecute()
        {
            if (IsOverdue())
            {
                log.Info(String.Format("task {0} is overdue, executing.", m_name));

                DateTime startTime = DateTime.Now;
                m_action();
                m_lastExecutionTime = DateTime.Now-startTime;

                m_lastExecution = DateTime.Now;
                log.Info(String.Format("done. Execution took {0}ms.", m_lastExecutionTime.TotalMilliseconds));
                return true;
            }
            log.Debug(String.Format("task {0} is not overdue, skipping.", m_name));
            return false;
        }

        public bool IsOverdue()
        {
            return ((m_lastExecution + m_frequency) < DateTime.Now);
        }

        public override string ToString()
        {
            return (String.Format("[{0}] {1}\n   frequency: {2}\n   last exec: {3}",
                IsOverdue() ? "o" : " ",
                m_name,
                m_frequency,
                m_lastExecution == DateTime.MinValue ? "Never" : String.Format("{0}\n   last exec time: {1}ms", m_lastExecution.ToString(), m_lastExecutionTime.TotalMilliseconds)
                ));
        }

        public int CompareTo(object o)
        {
            if (o is TaskSchedulerTask)
                return m_lastExecutionTime.CompareTo((TaskSchedulerTask)o);
            throw new ArgumentException("object is not a TaskSchedulerTask");
        }

        public TaskSchedulerTask(string name, TimeSpan frequency, TaskSchedulerAction action)
        {
            m_name = name;
            m_frequency = frequency;
            m_action = action;
            m_lastExecution = DateTime.MinValue;
            m_lastExecutionTime = TimeSpan.MinValue;
        }
    }
}