using System;
using System.Collections.Generic;
using System.IO;
//using System.Threading.Tasks;
using System.IO.Ports;

namespace AlegriaCom
{
    class Program
    {
        public const byte end_msg = 0x07;
        public static SerialPort myport = new SerialPort();
        static void Main(string[] args)
        {
            myport.ReadTimeout = 1500;
            myport.BaudRate = 9600;
            myport.Handshake = System.IO.Ports.Handshake.None;

            
             if (!SelectComPort())
             {
                 Console.Write("No Com Port :( .");
                 using (StreamWriter w = System.IO.File.AppendText("log.txt")) { log("No Com Port :( .", w); }
                 Console.Read();
                 Environment.Exit(0);
             }
             else
             {
                 using (StreamWriter w = System.IO.File.AppendText("log.txt")) { log(myport.PortName, w); }
             }
           
              Console.ReadKey();
        }

        public static bool SelectComPort()
        {
            List<string> PortName = new List<string>();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                PortName.Add(s);
            }
            if (PortName.Count == 0)
            {
              return false;
            }
            else
            {
                byte number = 1;
                foreach (string s in PortName)
                {
                    Console.WriteLine(number++ + "." + s);
                }
                Console.Write("Select your com port:");
                int comport = 0;
                bool key_in = false;
                do
                {
                    try
                    {
                        comport = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                    }
                    catch (Exception)
                    {
                        Console.Write("\r\nEnter Valid number 1 to {0}:", PortName.Count);
                        continue;
                    }
                    if ((comport > PortName.Count) || (comport <= 0))
                    {
                        Console.Write("\r\nEnter Valid number 1 to {0}:", PortName.Count);
                        continue;
                    }
                    key_in = true;
                } while (!key_in);
                myport.PortName = PortName[comport - 1];
                Console.WriteLine("\r\n" + PortName[comport - 1]);
            }

            return true;
        }
        public static bool SendData(byte[] data)
        {
            if(myport.PortName =="")
            {
                Console.Write("\r\n No port select.");
                return false;
            }
            try
            {
                if (myport.IsOpen) myport.Close();
                myport.Open();
            }
            catch
            {
                Console.Write("\r\n Port is busy {0}.", myport.PortName);
                return false;
            }
            myport.Write(data, 0, data.Length);
            return true;
        }
        public static void log(string log, TextWriter w)
        {
            DateTime time = DateTime.Now;
            string timelog = time.ToString("dd.MM.yyyy hh:mm ");
            w.Write(timelog + log);           
        }


       
    }
}
