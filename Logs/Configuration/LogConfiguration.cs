using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logs.Configuration
{
  public class LogConfiguration
  {
    public static readonly String EXTENSION = ".log";
    public String LogFolder = "Logs";
    public FormatConfiguration Formatter;
    private IDictionary<String, String> Properies;

    public int MaxHistory
    {
      get
      {
        var maxHistory = AsInt("maxHistory");
        return Math.Max(maxHistory, 10);
      }
    }

    public LogConfiguration(String fileName)
    {
      Properies = LoadProperties(fileName);
      LogFolder = GetProperty("log.folder") ?? LogFolder;
      ResolveLogFolder();
      Formatter = new FormatConfiguration(GetProperty("pattern"));
    }

    public String GetProperty(String name)
    {
      if (Properies.ContainsKey(name))
        return Properies[name];
      return null;
    }

    private void ResolveLogFolder()
    {
      if (!Directory.Exists(LogFolder))
      {
        try
        {
          Directory.CreateDirectory(LogFolder);
        }
        catch
        {
          LogFolder = Path.Combine(Application.StartupPath, LogFolder);
          try
          {
            Directory.CreateDirectory(LogFolder);
          }
          catch (Exception ex)
          {
            throw new ArgumentException("log folder is invalid." + LogFolder + ", " + ex.Message);
          }
        }
      }
    }

    /// <summary>
    /// Resolve file name with extension and current datetime
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public String ResolveFileName(String fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        return null;
      var fullFileName = String.Format("{0}_{1:yyyy-MM-dd}{2}", fileName, DateTime.Now, EXTENSION);
      return Path.Combine(LogFolder, fullFileName);
    }

    /// <summary>
    /// Load properties file to Dictionary
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public IDictionary<String, String> LoadProperties(String fileName)
    {
      if (!File.Exists(fileName))
      {
        return new Dictionary<String, String>();
      }
      var lines = File.ReadAllLines(fileName);
      var dic = new Dictionary<String, String>();
      foreach (var s in lines)
      {
        if (s.StartsWith("#"))
          continue;
        var configured = s.Split('=', ':');
        if (configured.Length >= 2)
        {
          var key = configured[0];
          if (dic.ContainsKey(key))
            dic.Remove(key);
          dic.Add(key, configured[1]);
        }
      }
      return dic;
    }

    public int AsInt(String name)
    {
      try
      {
        var value = GetProperty(name);
        if (String.IsNullOrEmpty(value))
          return Int32.MinValue;
        return Int32.Parse(value);
      }
      catch
      {
        return Int32.MinValue;
      }
    }
  }
}