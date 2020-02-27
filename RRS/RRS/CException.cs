using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RRS
{
    class CException : Exception
    {
        public CException() : base()
        {
            
        }

        public CException(String message) : base(message)
        {

        }

        public CException(String message, Exception innerException) : base(message, innerException)
        {

        }

        protected CException(SerializationInfo info, StreamingContext context)
        {

        }
    }

    class RouteException : CException
    {
        public RouteException() : base()
        {

        }

        public RouteException(string message) : base(message)
        {
            
        }
    }
}
