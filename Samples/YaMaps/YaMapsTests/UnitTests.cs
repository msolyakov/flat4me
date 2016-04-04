using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Newtonsoft.Json;

namespace YaMapsTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void JsonParseTest()
        {
            string json;
            using (FileStream fs = new FileStream("geocoder-data.json", FileMode.Open))
            {
                StreamReader r = new StreamReader(fs);
                json = r.ReadToEnd();
                fs.Close();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Auto;

            YaMaps.GeocoderResponse georesp = JsonConvert.DeserializeObject<YaMaps.GeocoderResponse>(json, settings);
            if (georesp.Response == null)
            {
                throw new Exception("JSON parse failed");
            }
        }
    }
}
