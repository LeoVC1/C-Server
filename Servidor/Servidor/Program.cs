using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Servidor
{
    class Program
    {
        public static Socket connection;
        public static BinaryWriter binaryWriter;
        public static BinaryReader binaryReader;
        public static NetworkStream networkStream;

        static void Main(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(RunServer));
            thread.Start();
        }

        private static void RunServer()
        {
            TcpListener tcpListener;

            try
            {
                tcpListener = new TcpListener(7777);
                tcpListener.Start();

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[SERVIDOR]: Aguardando conexão...");
                    connection = tcpListener.AcceptSocket();
                    Console.WriteLine("[SERVIDOR]: Conexão estabelecida. Remote end point: " + connection.RemoteEndPoint); ;
                    networkStream = new NetworkStream(connection);
                    binaryWriter = new BinaryWriter(networkStream);
                    binaryReader = new BinaryReader(networkStream);

                    Random rnd = new Random(DateTime.Now.Millisecond);

                    ConsoleColor connectionColor = (ConsoleColor) rnd.Next(0, 10);

                    string msg;

                    Console.ForegroundColor = connectionColor;

                    do
                    {
                        msg = binaryReader.ReadString();
                        Console.WriteLine(msg, Console.ForegroundColor);
                    } while (msg.ToLower() != "fim");

                    binaryWriter.Close();
                    binaryReader.Close();
                    networkStream.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
