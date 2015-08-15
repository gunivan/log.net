using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Appender
{
  interface IAppender
  {
    void Push(LogContext context);
    void Flush(LogContext context);
    void Flush(List<LogContext> contexts);
  }
}
