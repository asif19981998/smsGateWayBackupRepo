using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientrReader
{
    public class ReceivedSms
    {
        const int PORT_NO = 5038;
        const string SERVER_IP = "192.168.88.14";
        public static void ReceivedSmsByIp()
        {

            #region SmsReceiver Region
            using (var context = new SMSDbContext())
            {
                //---data to send to the server---
                string textToSend;

                string receivedData = "";
                TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();
                while (true)
                {

                    //textToSend = Console.ReadLine();
                    //---create a TCPClient object at the IP and port no.---

                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("Action: login\r\nUsername: apiuser\r\nSecret: apipass\r\n\r\n");

                    //---send the text---
                    //Console.WriteLine("Sending : " + textToSend);
                    //nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text-- -
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    receivedData = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    if (receivedData.Contains("Asterisk Call Manager"))
                    {
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    }
                    else if (receivedData.Contains("Authentication accepted"))
                    {
                        Console.WriteLine(receivedData);
                    }

                    else if (receivedData.Contains("Event: ReceivedSMS"))
                    {
                        string[] words = receivedData.Split("\r\n");
                        SmsInbox sms = new SmsInbox();
                        sms.ID = 0;
                        sms.GsmSpan = words[3].Substring(8);
                        sms.Sender = words[4].Substring(7);
                        sms.Recvtime =Convert.ToDateTime(words[5].Substring(10));
                        sms.SIndex = words[6].Substring(7);
                        sms.Total = words[7].Substring(7);
                        sms.Smsc = words[8].Substring(6);
                        sms.Content =Convert.ToString(words[9].Substring(9));
                        
                        Console.WriteLine(sms.Content);
                        context.smsInbox.Add(sms);
                        context.SaveChanges();
                        List<string> smsInbox = new List<string>();
                        Console.WriteLine(receivedData.GetType() + "  ghghghghg");
                        Console.WriteLine(receivedData);
                    }




                    //Console.WriteLine("Received : " + );
                    //Console.ReadLine();
                    //client.Close();

                }


            }

            #endregion


        }
    }
}
