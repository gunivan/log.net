using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Configuration
{
  public class FormatConfiguration
  {
    /// <summary>
    ///  %date [%thread] %level %logger - %message
    /// </summary>    
    public static readonly String DEFAULT_PATTERN = "%date [%thread] %level %logger - %message";
    private static readonly IDictionary<String, String> MAPPING = new Dictionary<String, String>() { 
    {"%date","{0:MM-dd-yy HH:mm:ss.fff}"},
    {"%thread","{1}"},{"%logger","{2}"},{"%message","{3}"}
    };
    public String Pattern { get; private set; }
    /// <summary>
    /// %date %thread %level %logger %msg
    /// </summary>
    public String PatternDebug { get; private set; }
    /// <summary>
    /// %date %thread %level %logger %msg
    /// </summary>
    public String PatternError { get; private set; }

    /// <summary>
    /// %date %thread %level %logger %msg
    /// </summary>
    public String PatternWarn { get; private set; }

    public FormatConfiguration(String pattern)
    {
      Pattern = String.IsNullOrEmpty(pattern) ? DEFAULT_PATTERN : pattern;
      var sb = new StringBuilder(Pattern);
      foreach (var item in MAPPING)
      {
        sb.Replace(item.Key, item.Value);
      }
      Pattern = sb.ToString();
      PatternDebug = new StringBuilder(Pattern).Replace("%level", "DEBUG").ToString();
      PatternError = new StringBuilder(Pattern).Replace("%level", "ERROR").ToString();
      PatternWarn = new StringBuilder(Pattern).Replace("%level", "WARN").ToString();
    }
  }
}
