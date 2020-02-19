using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Client
{
    class Program
    {
        public static BinaryWriter binaryWriter;
        public static BinaryReader binaryReader;
        public static NetworkStream NetworkStream;

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        static void Main(string[] args)
        {
            IntPtr hWnd = GetConsoleWindow();
            ShowWindow(hWnd, 0);
            Thread thread = new Thread(new ThreadStart(RunClient));
            thread.Start();
        }

        private static void RunClient()
        {
            TcpClient client;

            try
            {
                client = new TcpClient();
                client.Connect("localhost", 7777);

                Console.WriteLine("[SERVIDOR]: Conexão estabelecida."); ;
               
                NetworkStream = client.GetStream();
                binaryWriter = new BinaryWriter(NetworkStream);
                binaryReader = new BinaryReader(NetworkStream);

                var msg = "";
                do
                {
                    msg = Console.ReadLine();
                    binaryWriter.Write(msg);
                    //IntPtr hWnd = GetConsoleWindow();
                    //ShowWindow(hWnd, 1);
                } while (msg.ToLower() != "fim");
                //Console.WriteLine(msg);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
