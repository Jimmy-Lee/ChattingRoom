using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ChattingRoom
{
    public class ChatServer
    {
        List<ChatSocket> clientList = new List<ChatSocket>();

        public static void Main(String[] args)
        {
            ChatServer chatServer = new ChatServer();
            chatServer.run();
        }

        public void run()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, ChatSetting.port);

            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);

            while (true)
            {
                Socket socket = newsock.Accept();
                Console.WriteLine("�����@�ӷs�s�u!");
                ChatSocket client = new ChatSocket(socket);
                try
                {
                    clientList.Add(client);
                    client.newListener(processMsgComeIn);
                }
                catch
                {
                }
//                clientList.Remove(client);
            }
            //	  newsock.Close();
        }

        public String processMsgComeIn(String msg)
        {
            Console.WriteLine("����T���G"+msg);
            broadCast(msg);
            return "OK";
        }

        public void broadCast(String msg)
        {
            Console.WriteLine("�s���T���� " + msg+" �u�W�ϥΪ̦@"+clientList.Count+"�ӤH!");
            foreach (ChatSocket client in clientList)
            {
				if (!client.isDead) {
					Console.WriteLine("Send to "+client.remoteEndPoint.ToString()+":"+msg);
					client.send(msg);
				}
            }
        }
    }
}
