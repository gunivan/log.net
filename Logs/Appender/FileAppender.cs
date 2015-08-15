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
  public class FileAppender : AppenderBase
  {
    private IDictionary<String, StreamWriter> writes = new Dictionary<string, StreamWriter>();

    public FileAppender(LogConfiguration setting)
      : base(setting)
    {
    }
    private StreamWriter GetWriter(String logger)
    {
      if (!writes.ContainsKey(logger))
      {
        try
        {
          var fullFile = Setting.ResolveFileName(logger);
          writes.Add(logger, new StreamWriter(fullFile, true));
        }
        catch (Exception e)
        {
          Debug.Print("Cannot init StreamWriter for " + logger + "," + e.Message);
          return null;
        }
      }
      return writes[logger];
    }

    public override void Flush(LogContext context)
    {
      var msg = Format(context.Logger, context);
      if (String.IsNullOrEmpty(msg))
        return;
      var sw = GetWriter(context.Logger);
      sw.WriteLine(msg);
      sw.Flush();
    }


    public override void Flush(List<LogContext> contexts)
    {

      if (contexts == null || contexts.Count <= 0)
        return;
      foreach (var context in contexts)
      {
        var sw = GetWriter(context.Logger);
        var msg = Format(context.Logger, context);
        if (!String.IsNullOrEmpty(msg))
          sw.WriteLine(msg);
        sw.Flush();
      }
    }
  }
}