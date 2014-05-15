using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IISPoolController
{
    class Program
    {
        private static bool bIsDone = false;
        static void Main(string[] args)
        {
            ConsoleCmdLine cl = new ConsoleCmdLine();
            CmdLineString hostCmdLineString = new CmdLineString("hostname",true,"IIS App Pool hostname");
            CmdLineString userCmdLineString = new CmdLineString("username",false,"IIS App Pool username or admin. Not required for local pool management");
            CmdLineString passwordCmdLineString = new CmdLineString("password", false, "IIS App Pool user or admin password. Not required for local pool management");
            CmdLineString appPoolNameCmdLineString = new CmdLineString("poolname",true,"IIS App Pool name");
            CmdLineInt intervalCmdLineInt = new CmdLineInt("interval",false,"Interval of attempts. 30 secs by default");
            CmdLineString actionCmdLineString = new CmdLineString("action",true,"IIS Pool action");

            cl.RegisterParameter(hostCmdLineString);
            cl.RegisterParameter(userCmdLineString);
            cl.RegisterParameter(passwordCmdLineString);
            cl.RegisterParameter(appPoolNameCmdLineString);
            cl.RegisterParameter(intervalCmdLineInt);
            cl.RegisterParameter(actionCmdLineString);
            cl.Parse(args);
            var ap = new AppPoolController(hostCmdLineString, userCmdLineString, appPoolNameCmdLineString, passwordCmdLineString, intervalCmdLineInt);
            
            while (bIsDone==false)
            {
                ap.AppPoolAction(actionCmdLineString);
                Thread.Sleep(1000*intervalCmdLineInt);
            }
            
        }
    }
}
