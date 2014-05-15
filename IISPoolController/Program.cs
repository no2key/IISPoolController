using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISPoolController
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleCmdLine cl = new ConsoleCmdLine();
            CmdLineString hostCmdLineString = new CmdLineString("hostname",true,"IIS App Pool hostname");
            CmdLineString userCmdLineString = new CmdLineString("username",true,"IIS App Pool username or admin");
            CmdLineString passwordCmdLineString = new CmdLineString("password",true,"IIS App Pool user or admin password");
            CmdLineString appPoolNameCmdLineString = new CmdLineString("poolname",true,"IIS App Pool name");
            CmdLineInt intervalCmdLineInt = new CmdLineInt("interval",true,"Interval of attempts");
            CmdLineString actionCmdLineString = new CmdLineString("action",true,"IIS Pool action");

            cl.RegisterParameter(hostCmdLineString);
            cl.RegisterParameter(userCmdLineString);
            cl.RegisterParameter(passwordCmdLineString);
            cl.RegisterParameter(appPoolNameCmdLineString);
            cl.RegisterParameter(intervalCmdLineInt);
            cl.RegisterParameter(actionCmdLineString);
            cl.Parse(args);

            var ap = new AppPoolController(hostCmdLineString,userCmdLineString,appPoolNameCmdLineString,passwordCmdLineString,intervalCmdLineInt);
            ap.AppPoolAction(actionCmdLineString);
        }
    }
}
