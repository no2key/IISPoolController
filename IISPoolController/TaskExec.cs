using System;
using Microsoft.Build.Utilities;
using System.Threading;

namespace IISPoolController
{
    public class TaskExec : Task
    {
        private bool isDone;
        public string Command { get; set; }
        public string Hostname { get; set; }
        public string PoolName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Interval { get; set; }

        private const int MaxTryCount = 10;

        public TaskExec()
        {
            Interval = 10; //sec
        }

        public override bool Execute()
        {
            try
            {
                var ap = new AppPoolController(Hostname, UserName, PoolName, Password);
                isDone = false;
                var tryCount = 0;

                while (isDone == false && tryCount++ <= MaxTryCount)
                {
                    isDone = ap.AppPoolAction(Command);
                
                    Thread.Sleep(1000 * Interval);
                }
            }
            catch (Exception ex)
            {
                AppPoolController.HostAccessExceptionHandler(ex);
                isDone = false;
            }
            return isDone;
        }
    }
}