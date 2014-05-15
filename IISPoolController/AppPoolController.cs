using System;
using System.Diagnostics;
using System.Management;

namespace IISPoolController
{
    internal class AppPoolController
    {
        private readonly ConnectionOptions _options;
        private readonly int _iInterval;
        private readonly string _sHost;
        private readonly string _sPassword;
        private readonly string _sPoolName;
        private readonly string _sUser;

        public AppPoolController(string sHost, string sUser, string sPoolName, string sPassword, int iInterval=30)
        {
            _iInterval = iInterval;
            _sHost = sHost;
            _sPassword = sPassword;
            _sPoolName = sPoolName;
            _sUser = sUser;
//            sHost = ISPoolController.Default.host;
//            sUser = ISPoolController.Default.user;
//            sPassword = ISPoolController.Default.password;
//            sPoolName = ISPoolController.Default.pool;
//            iInterval = ISPoolController.Default.interval;
//            ISPoolController.Default.Save();
            _options = new ConnectionOptions {Username = sUser, Password = sPassword};
        }

        public bool AppPoolAction(string action)
        {
            bool bSuccess = false;
            _options.Authentication = AuthenticationLevel.PacketPrivacy;
            var scope = new ManagementScope(@"\\" + _sHost + "\\root\\MicrosoftIISv2", _options);

            try
            {
                scope.Connect();
                var path = new ManagementPath("IIsApplicationPool='W3SVC/AppPools/" + _sPoolName + "'");
                var pool = new ManagementObject(scope, path, null);
                switch (action.ToLower())
                {
                    case "stop":
                        Console.Write("Stopping \"{0}\" on \"{1}\"...\n", _sPoolName, _sHost);
                        LogWriter.WriteLog(DateTime.Now, "Stopping " + _sPoolName + " on " + _sHost);
                        pool.InvokeMethod("Stop", new object[0]);
                        bSuccess = true;
                        Console.WriteLine("Stopping is done \n");
                        LogWriter.WriteLog(DateTime.Now, "Stopping is done on " + _sHost);
                        break;
                    case "start":
                        Console.WriteLine("Starting \"{0}\" on \"{1}\"...\n", _sPoolName, _sHost);
                        LogWriter.WriteLog(DateTime.Now, "Starting " + _sPoolName + " on " + _sHost);
                        pool.InvokeMethod("Start", new object[0]);
                        bSuccess = true;
                        Console.WriteLine("Starting is done \n");
                        LogWriter.WriteLog(DateTime.Now, "Starting is done on" + _sHost);
                        break;
                    case "recycle":
                        Console.WriteLine("Recycling \"{0}\" on \"{1}\"...\n", _sPoolName, _sHost);
                        LogWriter.WriteLog(DateTime.Now, "Recycling " + _sPoolName + " on " + _sHost);
                        pool.InvokeMethod("Recycle", new object[0]);
                        bSuccess = true;
                        Console.WriteLine("Recycling is done \n");
                        LogWriter.WriteLog(DateTime.Now, "Recycling is done on " + _sHost);
                        break;
                    default:
                        Console.WriteLine("Incorrect operation. Operations can be start,stop,recyle");
                        LogWriter.WriteLog(DateTime.Now, "Incorrect operation. Operations can be is start,stop,recycle");
                        break;
                }
            }
            catch (Exception ex)
            {
                HostAccessExceptionHandler(ex);
            }
            return bSuccess;
        }

        private static void HostAccessExceptionHandler(Exception exception)
        {
            switch (exception.HResult)
            {
                case -2147463168:
                    LogWriter.WriteLog(DateTime.Now,
                        "Please enable component IIS Metabase and IIS 6 configuration compatibility");
                    Console.WriteLine("Please enable component IIS Metabase and IIS 6 configuration compatibility");
                    Process.Start(Environment.SystemDirectory + @"\OptionalFeatures.exe");
                    break;
                case -2147024893:
                    LogWriter.WriteLog(DateTime.Now,
                        "The system can't find the path specified. Check the application pool name");
                    Console.WriteLine("The system can't find the path specified. Check the application pool name");
                    break;
                case -2147023174:
                    LogWriter.WriteLog(DateTime.Now, "Host unreachable. Check the hostname");
                    Console.WriteLine("Host unreachable. Check the hostname");
                    break;
                case -2147024891:
                    LogWriter.WriteLog(DateTime.Now, "Access denied. Check the credentials");
                    Console.WriteLine("Access denied. Check the credentials");
                    break;
                default:
                    LogWriter.WriteLog(DateTime.Now, exception.Message + ". Undocumented error " + exception.HResult);
                    Console.WriteLine(exception.Message);
                    break;
            }
        }
    }
}