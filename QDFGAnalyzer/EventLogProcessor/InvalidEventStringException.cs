using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDFGGraphManager.EventLogProcessor
{
    internal sealed class InvalidEventStringException : Exception
    {
        public InvalidEventStringException()
        {
        }

        public InvalidEventStringException(String message) : base(message)
        {
        }

        public InvalidEventStringException(String message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
