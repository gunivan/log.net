using Logs.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace Logs.Appender
{
  public class ConsoleAppender : AppenderBase
  {
    public ConsoleAppender(LogConfiguration setting)
      : base(setting)
    {
    }           

    public override void Flush(LogContext context)
    {
      var msg = Format(context.Logger, context);
      if (!String.IsNullOrEmpty(msg))
      {
        Debug.Print(msg);
      }
    }


    public override void Flush(List<LogContext> contexts)
    {

      if (contexts == null || contexts.Count <= 0)
        return;
      foreach (var context in contexts)
      {
        var msg = Format(context.Logger, context);
        if (!String.IsNullOrEmpty(msg))
        {
          Debug.Print(msg);
        }
      }
    }
  }
}