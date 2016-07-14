using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Savers
{
    internal interface IImageSaver
    {
        bool SaveImage(int imageId, byte[] imageData, out string errorMessage);
    }
}
