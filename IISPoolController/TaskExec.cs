using System;
using Microsoft.Build.Utilities;

namespace IISPoolController
{
    internal class TaskExec : Task
    {
        private bool isDone;
        public string Command { get; set; }
        public string Hostname { get; set; }
        public string PoolName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public override bool Execute()
        {
            try
            {
                var ap = new AppPoolController(Hostname, UserName, PoolName, Password, 30);
                ap.AppPoolAction(Command);
                isDone = true;
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