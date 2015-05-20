using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;

namespace RestMocker.Core.Spec
{
    class NullLogger : ILogger
    {
        public void Debug(string message) { }

        public void Debug(string format, params object[] args) { }

        public void Debug(Exception exception, string format, params object[] args) { }

        public void DebugException(string message, Exception exception) { }

        public void Info(string message) { }

        public void Info(string format, params object[] args) { }

        public void Info(Exception exception, string format, params object[] args) { }

        public void InfoException(string message, Exception exception) { }

        public void Trace(string message) { }

        public void Trace(string format, params object[] args) { }

        public void Trace(Exception exception, string format, params object[] args) { }

        public void TraceException(string message, Exception exception) { }

        public void Warn(string message) { }

        public void Warn(string format, params object[] args) { }

        public void Warn(Exception exception, string format, params object[] args) { }

        public void WarnException(string message, Exception exception) { }

        public void Error(string message) { }

        public void Error(string format, params object[] args) { }

        public void Error(Exception exception, string format, params object[] args) { }

        public void ErrorException(string message, Exception exception) { }

        public void Fatal(string message) { }

        public void Fatal(string format, params object[] args) { }

        public void Fatal(Exception exception, string format, params object[] args) { }

        public void FatalException(string message, Exception exception) { }

        public Type Type
        {
            get { return null; }
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsTraceEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }
    }
}
