using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Logs
{
  public class Logger : IDisposable
  {
    public String Name;
    public event EventHandler<LogContext> PushLog;
    public Logger(String name)
    {
      this.Name = name;
    }
    public override bool Equals(object obj)
    {
      return obj == null ? false : Name.Equals(((Logger)obj).Name);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    void Flush(LogLevel level, String msg)
    {
      PushLog(this, new LogContext()
      {
        LoggedTime = DateTime.Now,
        message = msg,
        ThreadName = Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString(),
        Level = level,
        Logger = this.Name
      });
    }

    public void Debug(string sText, MethodBase method = null)
    {
      Flush(LogLevel.DEBUG, method == null ? sText : method.ToString() + ":" + sText);
    }

    public void Debug(string sText, params Object[] param)
    {
      Flush(LogLevel.DEBUG, String.Format(sText, param));
    }
    public void Error(Exception e)
    {
      Flush(LogLevel.ERROR, e.ToString());
    }

    public void Error(String message, Exception e)
    {
      Flush(LogLevel.ERROR, String.Format("{0} : {1}", message, e.ToString()));
    }

    public void Error(string format, params Object[] param)
    {
      Flush(LogLevel.ERROR, String.Format(format, param));
    }

    public void Warn(string sText, MethodBase method = null)
    {
      Flush(LogLevel.WARN, method == null ? sText : method.ToString() + ":" + sText);
    }

    public void Warn(string sText, params Object[] param)
    {
      Flush(LogLevel.WARN, String.Format(sText, param));
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}