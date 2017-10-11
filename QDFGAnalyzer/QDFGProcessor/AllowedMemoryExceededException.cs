using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDFGGraphManager.QDFGProcessor
{
    public class AllowedMemoryExceededException : Exception
    {
        public AllowedMemoryExceededException() {}

        public AllowedMemoryExceededException(String message) : base(message)
        {
        }

        public AllowedMemoryExceededException(String message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
