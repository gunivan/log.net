
using System;
using System.Collections.Generic;
namespace Logs
{
  public class LogContext
  {
    public String Logger { get; set; }
    public DateTime LoggedTime { get; set; }
    public String message { get; set; }
    public String ThreadName { get; set; }
    public LogLevel Level { get; set; }
  }

  public enum LogLevel
  {
    DEBUG = 0,
    ERROR,
    WARN
  }
}