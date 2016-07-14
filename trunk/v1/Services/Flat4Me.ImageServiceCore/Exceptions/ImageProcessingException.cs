using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Exceptions
{
    public class ImageProcessingException : Exception
    {
        public ImageProcessingException() : base()
        {
        }

        public ImageProcessingException(string message) : 
            base(message)
        {
        }

        public ImageProcessingException(string message, Exception inner) :
            base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected ImageProcessingException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
