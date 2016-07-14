using ImageResizer.Configuration;
using ImageResizer.Plugins.Watermark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Savers
{
    internal class ImageResizerWrapper
    {
        Config _config = null;
        string _instr;
        string _wmark;

        public ImageResizerWrapper(uint res, string wmark = null, string format = "jpg")
        {
            _wmark = wmark;
            _instr = String.Format("width={0};height={0};format={1};mode=max;quality=90", res, format);

            // Инициализируем конфигурацию компонента ImageResizer
            _config = Config.Current;

            // Если требуется, подгружем WatermarkPlugin
            WatermarkPlugin wp = _config.Plugins.Get<WatermarkPlugin>();
            if (!String.IsNullOrEmpty(_wmark) && wp == null)
            {
                wp = new WatermarkPlugin();
                wp.Install(_config);
            }
        }

        public bool DoJob(Stream srcStream, Stream dstStream, out Exception jobEx)
        {
            jobEx = null;

            try
            {
                ImageResizer.Instructions instrObj = new ImageResizer.Instructions(_instr);
                if (!String.IsNullOrEmpty(_wmark))
                {
                    instrObj.Watermark = _wmark;
                }

                ImageResizer.ImageJob i = new ImageResizer.ImageJob(srcStream, dstStream, instrObj);
                _config.CurrentImageBuilder.Build(i);
            }
            catch(Exception e)
            {
                jobEx = e;
                return false;
            }

            return true;
        }
    }
}
