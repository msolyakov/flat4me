using ImageResizer.Configuration;
using ImageResizer.Plugins.Watermark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitFacade;
using System.Net.Mime;

namespace TestApp
{
    class Program
    {
        public static string TEST_QUEUE = "TestImageQueue";
        
        static void Main(string[] args)
        {
            Config c = Config.Current;

            //Get a reference to the instance we added previously
            WatermarkPlugin wp = c.Plugins.Get<WatermarkPlugin>();
            if (wp == null)
            { 
                //Install it if it's missing
                wp = new WatermarkPlugin();
                wp.Install(c);
            }

            // string wmrk_file_name = "E:\\_f4me\\source\\watermark.png";
            string src_file_name = "E:\\_f4me\\source\\1.jpg";
            string dst_file_key = Guid.NewGuid().ToString();

            // Send file to Rabbit
            using (FileStream src_file = new FileStream(src_file_name, FileMode.Open))
            {
                SendMessage(src_file);
                Console.WriteLine(String.Format("{0} bytes were sent.", src_file.Length));
            }

            // Receive file from Rabbit 
            MemoryStream received_file = null;
            try 
            {
                while (received_file == null)
                {
                    received_file = ReceiveMessage();
                }

                Console.WriteLine(String.Format("{0} bytes were received.", received_file.Length));
                // Save files to the disk
                Write1280px(received_file, dst_file_key, c);
                // Write1024px(received_file, dst_file_key, c);
                // Write800px(received_file, dst_file_key, c);
                // Write640px(received_file, dst_file_key, c);
            }
            finally
            {
                if (received_file != null)
                {
                    received_file.Close();
                    received_file.Dispose();
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        // Sends image to queue
        static void SendMessage( FileStream fileData )
        {
            byte[] fileBytes = new byte[fileData.Length];

            int numBytesToRead = (int)fileData.Length;
            int chunkSize = 4096;
            int readOffset = 0;
            while (numBytesToRead > 0)
            {
                if (chunkSize > numBytesToRead)
                    chunkSize = numBytesToRead;

                int n = fileData.Read(fileBytes, readOffset, chunkSize);

                if (n == 0)
                {
                    // The end of the file is reached.
                    break;
                }

                readOffset += n;
                numBytesToRead -= n;
            }

            using (RabbitFacade.RabbitFacade rmq = new RabbitFacade.RabbitFacade(Program.TEST_QUEUE))
            {
                try
                {
                    rmq.BeginTran();
                    rmq.Publish(MediaTypeNames.Image.Jpeg, fileBytes);
                    rmq.Commit();
                }
                catch (Exception e)
                {
                    rmq.Rollback();
                    Console.WriteLine("Exception:" + e.Message);
                }
            }
        }

        // Recive message from the queue
        static MemoryStream ReceiveMessage()
        {
            MemoryStream result = null;
            byte[] messageBody = null;

            // Получаем сообшение
            using (RabbitFacade.RabbitFacade rmq = new RabbitFacade.RabbitFacade(Program.TEST_QUEUE))
            {
                try
                {
                    rmq.InitConsumer();

                    Tuple<ContentType, byte[], bool, ulong> message;
                    while(rmq.Next(2000, out message))
                    {
                        if (message.Item1 == null || message.Item1.MediaType != MediaTypeNames.Image.Jpeg)
                        {
                            // Не задан или неверный ContentType. Игнорируем сообщение.
                            rmq.SetDelivered(message.Item4, false);
                            Console.WriteLine("Error: wrong content type");
                            continue;
                        }

                        messageBody = message.Item2;
                        rmq.SetDelivered(message.Item4, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception:" + e.Message);
                }
            }

            // Сохраняем полученный результат в поток
            if( messageBody != null )
            {
                result = new MemoryStream(messageBody);
            }

            return result;
        }

        static void Write1280px(MemoryStream src_file, string dst_file_key, Config c)
        {
            Console.Write("1280px..");
            src_file.Seek(0, SeekOrigin.Begin);

            string dst_file_name = "E:\\_f4me\\destination\\" + dst_file_key + ".1280px.jpg";
            using (FileStream dst_file = new FileStream(dst_file_name, FileMode.CreateNew))
            {
                ImageResizer.Instructions instr = new ImageResizer.Instructions("width=1280;height=1280;format=jpg;mode=max;quality=90");
                instr.Watermark = "TextTest";
                ImageResizer.ImageJob i = new ImageResizer.ImageJob(src_file, dst_file, instr);

                c.CurrentImageBuilder.Build(i);
                dst_file.Close();
            }

            Console.WriteLine(" Done.");
        }

        static void Write1024px(MemoryStream src_file, string dst_file_key, Config c)
        {
            Console.Write("1024px..");
            src_file.Seek(0, SeekOrigin.Begin);

            string dst_file_name = "E:\\_f4me\\destination\\" + dst_file_key + ".1024px.jpg";
            using (FileStream dst_file = new FileStream(dst_file_name, FileMode.CreateNew))
            {
                ImageResizer.Instructions instr = new ImageResizer.Instructions("width=1024;height=1024;format=jpg;mode=max;quality=90");
                instr.Watermark = "TextTest";

                ImageResizer.ImageJob i = new ImageResizer.ImageJob(src_file, dst_file, instr);
                i.ResetSourceStream = true;
                c.CurrentImageBuilder.Build(i);
                dst_file.Close();
            }

            Console.WriteLine(" Done.");
        }

        static void Write800px(MemoryStream src_file, string dst_file_key, Config c)
        {
            Console.Write("800px..");
            src_file.Seek(0, SeekOrigin.Begin);

            string dst_file_name = "E:\\_f4me\\destination\\" + dst_file_key + ".800px.jpg";
            using (FileStream dst_file = new FileStream(dst_file_name, FileMode.CreateNew))
            {
                ImageResizer.ImageJob i = new ImageResizer.ImageJob(src_file, dst_file,
                    new ImageResizer.Instructions("width=800;height=800;format=jpg;mode=max;quality=90"));
                i.ResetSourceStream = true;
                c.CurrentImageBuilder.Build(i);
                dst_file.Close();
            }

            Console.WriteLine(" Done.");
        }

        static void Write640px(MemoryStream src_file, string dst_file_key, Config c)
        {
            Console.Write("640px..");
            src_file.Seek(0, SeekOrigin.Begin);

            string dst_file_name = "E:\\_f4me\\destination\\" + dst_file_key + ".640px.jpg";
            using (FileStream dst_file = new FileStream(dst_file_name, FileMode.CreateNew))
            {
                ImageResizer.ImageJob i = new ImageResizer.ImageJob(src_file, dst_file,
                    new ImageResizer.Instructions("width=640;height=640;format=jpg;mode=max;quality=80"));
                i.ResetSourceStream = true;
                i.Build();
                dst_file.Close();
            }

            Console.WriteLine(" Done.");
        }
    
    }
}
