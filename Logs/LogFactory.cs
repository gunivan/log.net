using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Logs.Configuration;

namespace Logs
{
  public static class LogFactory
  {
    public static String configurationFile;
    private static LogConfiguration SETTING;
    private static Logger LOG;

    static LogFactory()
    {
      SETTING = new LogConfiguration(Path.Combine(Application.StartupPath, configurationFile ?? "log.conf"));
      LogWorker.Init(SETTING);
      LOG = LogWorker.GetLog("log");
    }

    public static Logger GetLog(Type type)
    {
      return LogWorker.GetLog(type.ToString());
    }

    public static void Error(Exception e)
    {
      LOG.Error(e);
    }

    public static void Error(String message, Exception e)
    {
      LOG.Error(message, e);
    }

    public static void Error(string format, params Object[] param)
    {
      LOG.Error(format, param);
    }

    public static void Warn(string sText, MethodBase method = null)
    {
      LOG.Warn(sText, method);
    }

    public static void Warn(string format, params Object[] param)
    {
      LOG.Warn(format, param);
    }

    public static void Debug(string sText, MethodBase method = null)
    {
      LOG.Debug(sText, method);
    }

    public static void Debug(string format, params Object[] param)
    {
      LOG.Debug(format, param);
    }
  }
}