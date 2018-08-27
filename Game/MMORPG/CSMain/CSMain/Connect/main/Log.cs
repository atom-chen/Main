using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMain;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using ExitGames.Logging;
public class LogManager
{
    private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();            //日志
    static LogManager()
    {

    }
    public static void Info(string format,params string[] para)
    {
        log.InfoFormat(format, para);
    }

    public static void Log(string format,params string[] para)
    {
        log.InfoFormat(format, para);
    }

    public static void Error(string format,params string[] para)
    {
        log.ErrorFormat(format, para);
    }
    
    public static void Debug(string format,params string[] para)
    {
        log.DebugFormat(format, para);
    }
}

