using System.Threading;

namespace IISPoolController
{
    internal class Program
    {
        private static bool _bIsDone;

        private static void Main(string[] args)
        {
            var cl = new ConsoleCmdLine();
            var hostCmdLineString = new CmdLineString("hostname", true, "IIS App Pool hostname");
            var userCmdLineString = new CmdLineString("username", true, "IIS App Pool username or admin");
            var passwordCmdLineString = new CmdLineString("password", true, "IIS App Pool user or admin password");
            var appPoolNameCmdLineString = new CmdLineString("poolname", true, "IIS App Pool name");
            var intervalCmdLineInt = new CmdLineInt("interval", true, "Interval of attempts");
            var actionCmdLineString = new CmdLineString("action", true, "IIS Pool action");

            cl.RegisterParameter(hostCmdLineString);
            cl.RegisterParameter(userCmdLineString);
            cl.RegisterParameter(passwordCmdLineString);
            cl.RegisterParameter(appPoolNameCmdLineString);
            cl.RegisterParameter(intervalCmdLineInt);
            cl.RegisterParameter(actionCmdLineString);
            cl.Parse(args);

            var ap = new AppPoolController(hostCmdLineString, userCmdLineString, appPoolNameCmdLineString,
                passwordCmdLineString, intervalCmdLineInt);

            while (_bIsDone == false)
            {
                _bIsDone = ap.AppPoolAction(actionCmdLineString);
                Thread.Sleep(1000 * intervalCmdLineInt);
            }
        }
    }
}