using Logs.Appender;
using Logs.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logs
{
  static class LogWorker
  {
    private static List<Logger> LOGGERS;
    private static List<IAppender> APPENDERS;
    private static LogConfiguration SETTING;

    public static void Init(LogConfiguration setting)
    {
      LOGGERS = new List<Logger>();
      SETTING = setting;
      APPENDERS = new List<IAppender>() { 
      new FileAppender(SETTING),
      new ConsoleAppender(SETTING)
      };
      Task.Factory.StartNew(() =>
      {
        CleanLog();
      });
    }

    public static Logger GetLog(String name)
    {
      var logger = new Logger(name);
      if (LOGGERS.Contains(logger))
        return LOGGERS.FirstOrDefault(lg => lg.Name == name);
      logger.PushLog += PushLog;
      LOGGERS.Add(logger);
      return logger;
    }

    static void PushLog(object sender, LogContext e)
    {
      foreach (var appender in APPENDERS)
      {
        appender.Push(e);
      }
    }

    public static void CleanLog()
    {
      Task.Factory.StartNew(() =>
      {
        var date = DateTime.Now.AddDays(-SETTING.MaxHistory);
        try
        {
          var files = Directory.GetFiles(SETTING.LogFolder, "*" + LogConfiguration.EXTENSION);
          foreach (var item in files)
          {
            var f = new FileInfo(item);
            if (f.CreationTime < date)
              File.Delete(item);
          }
        }
        catch (Exception e)
        {
          Debug.Print("LogWorker: Cannot clean old logs" + e.Message);
        }
      });
    }
  }
}
