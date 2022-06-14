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
            Ra
            UInt16 ModRTU_CRC(byte[] buf, int len)
            {
                UInt16 crc = 0xFFFF;

                for (int pos = 0; pos < len; pos++)
                {
                    crc ^= (UInt16)buf[pos];

                    for (int i = 8; i != 0; i--)
                    {
                        if ((crc & 0x0001) != 0)
                        {
                            crc >>= 1;
                            crc ^= 0xA001;
                        }
                        else
                            crc >>= 1;
                    }
                }

                return crc;
            }

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.105"), port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {

                    Socket handler = listenSocket.Accept();

                    Console.WriteLine("Новое подключение");
                    int bytes = 0;
                    byte[] data = new byte[256];
                    int x = 0;
                    int y = 0;
                    int errs = 0;
                    byte oldB = (byte)200;
                    byte newB;

                    while (true/*handler.Available > 0*/)
                    {
                        bytes = handler.Receive(data);
                        newB = data[0];
                        if (newB == oldB)
                        {
                            break;
                        }
                        oldB = newB;


                        x++;
                        y = y + DateTime.Now.Millisecond;
                        byte OldHighByte = data[254];
                        byte OldLowByte = data[255];
                        data[254] = (byte)0;
                        data[255] = (byte)0;



                        byte HighByte = (byte)(ModRTU_CRC(data, 256) >> 8);
                        byte LowByte = (byte)(ModRTU_CRC(data, 256) & 0xFF);
                        if (HighByte != OldHighByte || LowByte != OldLowByte)
                        {
                            errs++;
                        }


                    }
                    Console.WriteLine(errs);


                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            listenSocket.Close();
        }
    }
}
