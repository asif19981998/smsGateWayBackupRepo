using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientrReader
{
    public class SMSSend
    {
        public static void  SmsSend()
        {
            const int PORT_NO = 5038;
            const string SERVER_IP = "192.168.88.14";

            using (var context = new SMSDbContext())
            {
                string receivedData = "";
                TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();
                while (true)
                {
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("Action: login\r\nUsername: apiuser\r\nSecret: apipass\r\n\r\n");
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                   
                    receivedData = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                   
                    if (receivedData.Contains("Asterisk Call Manager"))
                    {
                        Console.WriteLine(receivedData);
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                      }
                    else if (receivedData.Contains("Authentication accepted"))
                    {
                        byte[] bytesToSendSMS = ASCIIEncoding.ASCII.GetBytes("Action: smscommand\r\nCommand: gsm show span 2\r\n\r\n");
                        nwStream.Write(bytesToSendSMS, 0, bytesToSendSMS.Length);
                     }
                    else
                    {
                        Console.WriteLine(receivedData);
                    }
                       }
 }

 
        
        
        }
 }
}
