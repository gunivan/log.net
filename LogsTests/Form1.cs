using Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogsTests
{
  public partial class Form1 : Form
  {
    static Logger LOG = LogFactory.GetLog(typeof(Form1));
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      var tasks = new List<Task>();
      for (int j = 0; j < 20; j++)
      {
        var task = new Task(() =>
        {
          for (int i = 0; i < 100; i++)
          {
            LOG.Debug(String.Format("Thread: {0} new message {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString()));
          }
        });
        tasks.Add(task);
        task.Start();
      }
      Task.WaitAll(tasks.ToArray());
      Debug.Print("End.");
      LOG.Error("END.");
    }

    private void button1_Click(object sender, EventArgs e)
    {
      LOG.Warn(DateTime.Now.ToString());
    }
  }
}
