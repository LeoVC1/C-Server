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
        public static TcpClient client;
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
                ConsoleColor serverColor = (ConsoleColor)10;
                Console.ForegroundColor = serverColor;

                tcpListener = new TcpListener(7777);
                tcpListener.Start();

                while (true)
                {
                    Console.WriteLine("[SERVIDOR]: Aguardando conexão...");
                    connection = tcpListener.AcceptSocket();
                    Console.WriteLine("[SERVIDOR]: Conexão estabelecida. Remote end point: " + connection.RemoteEndPoint); ;
                    networkStream = new NetworkStream(connection);
                    binaryWriter = new BinaryWriter(networkStream);
                    binaryReader = new BinaryReader(networkStream);

                    Random rnd = new Random(DateTime.Now.Millisecond);

                    int count = 0;
                    string msg;
                    List<int> numbers = new List<int>();

                    do
                    {
                        msg = binaryReader.ReadString();

                        if(int.TryParse(msg, out int number))
                        {
                            count += number;
                            numbers.Add(number);
                        }

                        if (msg.ToLower() == "mostrar")
                        {
                            Console.Write("Os numeros digitados foram: ", Console.ForegroundColor);

                            foreach(int n in numbers)
                            {
                                Console.Write(n.ToString() + ", ", Console.ForegroundColor);
                            }

                            Console.WriteLine($"\nA somatória foi {count}.", Console.ForegroundColor);
                        }

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
