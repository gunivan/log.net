using Logs.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logs.Appender
{
  public abstract class AppenderBase : IAppender
  {
    public LogConfiguration Setting { get; set; }
    protected ConcurrentQueue<LogContext> Queue;
    protected Thread worker;

    protected AppenderBase(LogConfiguration setting)
    {
      this.Setting = setting;
      Queue = new ConcurrentQueue<LogContext>();
      worker = new Thread(FlushLog);
      worker.IsBackground = true;
      worker.Start();
    }

    public void Push(LogContext context)
    {
      Queue.Enqueue(context);
    }

    protected String Format(String logger, LogContext context)
    {
      var msg = "";
      switch (context.Level)
      {
        case LogLevel.DEBUG:
          msg = String.Format(Setting.Formatter.PatternDebug, context.LoggedTime, context.ThreadName, logger, context.message);
          break;
        case LogLevel.WARN:
          msg = String.Format(Setting.Formatter.PatternWarn, context.LoggedTime, context.ThreadName, logger, context.message);
          break;
        case LogLevel.ERROR:
          msg = String.Format(Setting.Formatter.PatternError, context.LoggedTime, context.ThreadName, logger, context.message);
          break;
        default:
          break;
      }
      return msg;
    }

    protected void FlushLog()
    {
      while (true)
      {
        var context = default(LogContext);
        Queue.TryDequeue(out context);
        if (null != context)
        {
          Flush(context);
        }
      }
    }

    public virtual void Flush(LogContext context)
    {
      throw new NotImplementedException();
    }

    public virtual void Flush(List<LogContext> contexts)
    {
      throw new NotImplementedException();
    }
  }
}
