using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Laba_1_seti
{
    class Program
    {
        static int port = 8005; 
        static void Main(string[] args)
        {
            
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.105"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                
                while (true)
                {
                   
                    Socket handler = listenSocket.Accept();
                    
                    Console.WriteLine(1221122);
                    // получаем сообщение
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        
                        bytes = handler.Receive(data);
                       // Console.WriteLine(DateTime.Now.ToShortTimeString() + ": ");
                       // Console.WriteLine(data[0]);
                       // Console.WriteLine(data[1]);
                        for (int i = 2; i < 254; i++)
                        {
                            Console.Write((char)data[i]);
                        }
                        // Console.WriteLine("\n" + data[254]);
                        // Console.WriteLine(data[255]);
                        Thread.Sleep(12);

                    }
                    while (handler.Available > 0);
                    

                    
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
