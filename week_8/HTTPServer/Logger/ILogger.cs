using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
    public interface ILogger
    {
        public void Log(string info);

        public void ReportError(string error);
    }
}
