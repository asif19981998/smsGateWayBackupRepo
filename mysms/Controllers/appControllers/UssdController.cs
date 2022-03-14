using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mysms.DataBasebContext;
using mysms.Models;
using mysms.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mysms.Controllers.appControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UssdController : ControllerBase
    {
        MySmsDbContext context;
        public UssdController(MySmsDbContext mySmsDb)
        {
            context = mySmsDb;
        }
        [HttpPost]
        public string SendSms(SendUSSD sendUsd)
        {
             Thread.Sleep(30000);
            try
            {
                sendUsd.Content = sendUsd.Content;
                const int PORT_NO = 5038;
                const string SERVER_IP = "192.168.88.14";



                string fileTime = Convert.ToString(DateTime.Now.ToFileTime());
                Random random = new Random();
                int rnd = random.Next(1, 1000);
                string randomTime = Convert.ToString(rnd);
                string uniqueCode = fileTime + randomTime;

                string content = '"' + sendUsd.Content + '"';

                string sms = string.Format("Action: smscommand\r\ncommand: gsm send ussd {0} {1} \r\n\r\n", Convert.ToInt32(sendUsd.Port) + 1, content);



                string receivedData = "";
                TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream nwStream = client.GetStream();

                while (true)
                {
                    
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                    receivedData = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);


                    if (receivedData.Contains("Asterisk Call Manager"))
                    {
                        Console.WriteLine(receivedData);
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("Action: login\r\nUsername: apiuser\r\nSecret: apipass\r\n\r\n");
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    else if (receivedData.Contains("Authentication accepted"))
                    {
                        byte[] bytesToSendSMS = ASCIIEncoding.ASCII.GetBytes(string.Format("Action: smscommand\r\ncommand: gsm send ussd {0} {1} \r\n\r\n", Convert.ToInt32(sendUsd.Port) + 1, content));
                        nwStream.Write(bytesToSendSMS, 0, bytesToSendSMS.Length);
                    }

                    else if (receivedData.Contains("Received USSD success on span"))
                    {

                        string first = "Balance";
                        int Pos1 = receivedData.IndexOf(first);
                        string FinalString = receivedData.Substring(Pos1, receivedData.Length - Pos1);

                        return receivedData;
                        break;
                    }
                    else if(receivedData.Contains("Send USSD failed on span"))
                    {
                       
                        return "Send Failed!";
                        break;
                    }


                }



            }


            catch (Exception ex)
            {
                return "Something is wrong try again";
            }


        }


        [HttpGet]
        [Route("getAvailablePort")]

        public ICollection<AvailablePort> GetAvailablePort()
        {
            List<AvailablePort> ports = new List<AvailablePort>();


            try
            {
                const int PORT_NO = 5038;
                const string SERVER_IP = "192.168.88.14";

                string receivedData = "";

                for (int i = 1; i < 5; i++)
                {
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

                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        else if (receivedData.Contains("Authentication accepted"))
                        {
                            byte[] bytesToSendSMS = ASCIIEncoding.ASCII.GetBytes(string.Format("Action: smscommand\r\ncommand: gsm show span {0}  \r\n\r\n", i + 1));
                            nwStream.Write(bytesToSendSMS, 0, bytesToSendSMS.Length);
                        }

                        else if (receivedData.Contains("Manufacturer: SIMCOM_Ltd"))
                        {
                            if (receivedData.Contains("State: READY") && !receivedData.Contains("Network Status: Not registered"))
                               
                                {
                                AvailablePort availablePort = new AvailablePort();
                                List<PortSetting> portList = context.PortSettings.ToList();
                                availablePort.Number = i;
                                
                                if(i==1) availablePort.Label = portList[0].Port_1;
                                if (i == 2) availablePort.Label = portList[0].Port_2;
                                if (i == 3) availablePort.Label = portList[0].Port_3;
                                if (i == 4) availablePort.Label = portList[0].Port_4;

                                ports.Add(availablePort);



                                break;
                            }
                            else
                            {
                                break;
                            }
                        }


                    }

                }

                return ports;


            }


            catch (Exception ex)
            {

                return null;
            }


        }



    }
}
