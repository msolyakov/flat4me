using Flat4Me.Core.Rabbit;
using Flat4Me.Data.Repository.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.TestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StorageLogRepository logger = new StorageLogRepository();
                logger.Init();
                logger.LogInfo("Test program has started").Wait();
                logger.LogWarning("Some warning...").Wait();
                logger.LogWarning("And another one...", new Exception("Test Warning")).Wait();
                logger.LogException("... and Error", new Exception("Test Exception")).Wait();
                logger.LogInfo("Test program has stopped").Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Exception: {0}", e.Message));
            }

            Console.WriteLine("Press key to exit...");
            Console.Read();
        }
    }
}
