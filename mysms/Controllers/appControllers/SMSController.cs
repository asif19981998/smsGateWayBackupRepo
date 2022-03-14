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

namespace mysms.Controllers.appControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : Controller
    {
        MySmsDbContext context;
        public SMSController(MySmsDbContext mySmsDb)
        { 
            context = mySmsDb;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                
                var data = context.smsInbox.
                    Join(context.objects,
                    sms => sms.Sender,
                    cont => cont.PhoneNo,
                    (sms, cont) => new { Synonym = cont.Synonym, Name = cont.Name, Port = sms.GsmSpan, Sender = sms.Sender, Content = sms.Content, RecvDate= sms.Recvtime,Id=sms.ID }

                    ).OrderByDescending(c => c.Id).ToList();
                var smsWithoutSynonym = context.smsInbox.Select(sms => new { Synonym = "", Name = "", Port = sms.GsmSpan, Sender = sms.Sender, Content = sms.Content, RecvDate = sms.Recvtime, Id = sms.ID }).AsQueryable();
                smsWithoutSynonym = smsWithoutSynonym.Where(sms => !context.objects.Select(o => o.PhoneNo).Contains(sms.Sender));
                
                data.AddRange(smsWithoutSynonym.ToList());
                var inboxData = data.AsQueryable().OrderByDescending(sms => sms.Id).Where(sms => sms.RecvDate >= DateTime.Now.Date).ToList();
                return Ok(inboxData);
            }
            catch(Exception ex)
            {
                return null;
            }
                
           

        }
        [HttpPost]
        public string SendSms(SendSms sendSms)
        {
            try
            {
                List<string> portList = GetAvailablePort().Select(p => p.Number.ToString()).ToList();

                //if (!portList.Contains(sendSms.Port))
                //{
                //    return "Send Failed";
                //}
                sendSms.Content = sendSms.Content;
                const int PORT_NO = 5038;
                const string SERVER_IP = "192.168.88.14";

                foreach (var phoneNo in sendSms.PhoneNo)
                {

                    string fileTime = Convert.ToString(DateTime.Now.ToFileTime());
                    Random random = new Random();
                    int rnd = random.Next(1, 1000);
                    string randomTime = Convert.ToString(rnd);
                    string uniqueCode = fileTime + randomTime;

                    string content = '"' + sendSms.Content + '"';

                    string sms = string.Format("Action: smscommand\r\ncommand: gsm send sms {0} {1} {2} {3}\r\n\r\n", Convert.ToInt32(sendSms.Port) + 1, phoneNo, content, uniqueCode);



                    string receivedData = "";
                    TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                    NetworkStream nwStream = client.GetStream();
                    var i = 0;
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
                            byte[] bytesToSendSMS = ASCIIEncoding.ASCII.GetBytes($"Action: smscommand\r\nCommand: gsm show span {Convert.ToInt32(sendSms.Port) + 1}\r\n\r\n");
                            nwStream.Write(bytesToSendSMS, 0, bytesToSendSMS.Length);
                        }

                        else if (receivedData.Contains("Manufacturer: SIMCOM_Ltd"))
                        {
                            //if (receivedData.Contains("State: READY") && !receivedData.Contains("Network Status: Not registered"))
                            //{

                                byte[] smsContent = ASCIIEncoding.ASCII.GetBytes(sms);
                                nwStream.Write(smsContent, 0, smsContent.Length);
                                AddSendItem(new SendItem { Id = sendSms.Id, PhoneNo = phoneNo, Port = Convert.ToInt32(sendSms.Port), Message = sendSms.Content }, phoneNo, uniqueCode);
                                AddSysLog();

                                break;
                        //}
                        //else
                        //{
                        //    return "Sending Failed";

                        //}

                    }
                        i++;
                        if (i > 20)
                        {
                            return "Device Not Ready";
                            break;
                        }


                    }

                    Thread.Sleep(5000);

                }

            }

            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Successfully Send";
        }


        [HttpPost]
        [Route("GroupWiseSms")]
        public string SendSms(List<GroupWiseSmsSend> sendSms)
        {
            int sendItemCount = 0;
            try
            {
                List<string> portList = GetAvailablePort().Select(p => p.Number.ToString()).ToList();

                
                const int PORT_NO = 5038;
                const string SERVER_IP = "192.168.88.14";
               
                foreach (var smsItem in sendSms)
                {
                if (portList.Contains(smsItem.Port))
                    {
                    
                    string fileTime = Convert.ToString(DateTime.Now.ToFileTime());
                    Random random = new Random();
                    int rnd = random.Next(1, 1000);
                    string randomTime = Convert.ToString(rnd);
                    string uniqueCode = fileTime + randomTime;

                    string content = '"' + smsItem.Content + '"';

                    string sms = string.Format("Action: smscommand\r\ncommand: gsm send sms {0} {1} {2} {3}\r\n\r\n", Convert.ToInt32(smsItem.Port) + 1, smsItem.PhoneNo, content, uniqueCode);



                    string receivedData = "";
                    TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                    NetworkStream nwStream = client.GetStream();
                    var i = 0;
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
                            byte[] bytesToSendSMS = ASCIIEncoding.ASCII.GetBytes($"Action: smscommand\r\nCommand: gsm show span {Convert.ToInt32(smsItem.Port) + 1}\r\n\r\n");
                            nwStream.Write(bytesToSendSMS, 0, bytesToSendSMS.Length);
                        }

                        else if (receivedData.Contains("Manufacturer: SIMCOM_Ltd"))
                        {
                            if (receivedData.Contains("State: READY") && !receivedData.Contains("Network Status: Not registered"))
                            {

                                byte[] smsContent = ASCIIEncoding.ASCII.GetBytes(sms);
                                nwStream.Write(smsContent, 0, smsContent.Length);
                                sendItemCount += 1;
                                AddSendItem(new SendItem { Id = smsItem.Id, PhoneNo = smsItem.PhoneNo, Port = Convert.ToInt32(smsItem.Port), Message = smsItem.Content }, smsItem.PhoneNo, uniqueCode);
                                AddSysLog();

                                break;
                            }
                            else
                            {
                                return "Sending Failed";

                            }

                        }
                        i++;
                        if (i > 20)
                        {
                            return "Device Not Ready";
                            break;
                        }


                    }

                    Thread.Sleep(5000);

                }

            }
            
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

            return sendItemCount.ToString();
        }





        public bool AddSendItem(SendItem sendSms,string phoneNo,string uniqueCode)
        {
            try
            {
                SendItem sendItem = new SendItem();
                sendItem.Port = Convert.ToInt32(sendSms.Port) + 1;
                sendItem.PhoneNo = phoneNo;
                sendItem.Message = sendSms.Message;
                sendItem.MessageId = uniqueCode;
                sendItem.SendingTime = DateTime.Now;
                sendItem.StatusId = 2;

                context.sendItems.Add(sendItem);
                return context.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                return false;
            }
            

        }

        public bool AddSysLog()
        {

            try
            {
                SysLog sysLog = new SysLog();
                sysLog.LogType = "Send Sms";
                sysLog.Time = DateTime.Now;
                sysLog.IpAddress = "";
                sysLog.User = "User";

                context.sysLogs.Add(sysLog);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }


           

        }

        [HttpPost]
        [Route("searchForInboxSms")]
        public IActionResult SearchInboxSms(ReceivedSmsSearch searchValue)
        {
            try
            {

                var smsData = context.smsInbox.
                        Join(context.objects,
                        sms => sms.Sender,
                        cont => cont.PhoneNo,
                        (sms, cont) => new { Synonym = cont.Synonym, Name = cont.Name, Port = sms.GsmSpan, Sender = sms.Sender, Content = sms.Content, RecvDate = sms.Recvtime }

                        ).ToList();

                var result= smsData.AsQueryable();
               
                if (!string.IsNullOrEmpty(searchValue.Synonym) ||
                   !string.IsNullOrEmpty(searchValue.StartDate)||
                   !string.IsNullOrEmpty(searchValue.EndDate)||
                   searchValue.Port != 0 ||
                   !string.IsNullOrEmpty(searchValue.Content) ||
                   !string.IsNullOrEmpty(searchValue.Sender)

                   )
                {
                    //IEnumerable<SmsInbox> smsData = context.smsInbox;
                    
                    if (!string.IsNullOrEmpty(searchValue.StartDate))
                    {
                        if (string.IsNullOrEmpty(searchValue.EndDate))
                        {
                            searchValue.EndDate = searchValue.StartDate;
                        }

                        //smsData = smsData.Where(sms => (Convert.ToDateTime(sms.RecvDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(searchValue.StartDate)) && ((Convert.ToDateTime(sms.RecvDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(searchValue.EndDate))));

                        result = smsData.AsQueryable().Where(sms => (Convert.ToDateTime(sms.RecvDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(searchValue.StartDate)));


                    }

                    if (!string.IsNullOrEmpty(searchValue.EndDate))
                    {
                        if (string.IsNullOrEmpty(searchValue.StartDate))
                        {
                            searchValue.StartDate = searchValue.EndDate;
                        }

                        result = smsData.AsQueryable().Where(sms => (Convert.ToDateTime(sms.RecvDate.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(searchValue.StartDate)) && ((Convert.ToDateTime(sms.RecvDate.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(searchValue.EndDate))));
                    }
                    if (!string.IsNullOrEmpty(searchValue.Synonym))
                    {
                        Objects objects = new Objects();
                        objects = context.objects.FirstOrDefault(Objects => Objects.Synonym.ToLower() == searchValue.Synonym.ToLower());

                        if(objects != null)
                        {
                            result = smsData.AsQueryable().Where(sms => sms.Sender == objects.PhoneNo);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (searchValue.Port != 0)
                    {
                        result = smsData.AsQueryable().Where(sms => sms.Port == (Convert.ToInt64(searchValue.Port) + 1).ToString());
                    }
                    if (!string.IsNullOrEmpty(searchValue.Content))
                    {
                        result = smsData.AsQueryable().Where(sms => sms.Content.Contains(searchValue.Content));
                    }
                    if (!string.IsNullOrEmpty(searchValue.Sender))
                    {
                        if (!searchValue.Sender.Contains("+88"))
                        {
                            searchValue.Sender = "+88" + searchValue.Sender;
                        }
                        
                        result = smsData.AsQueryable().Where(sms => sms.Sender == searchValue.Sender);
                    }
                    
                        return Ok(result.ToList());
                    
                }

                else
                       {
                        
                        //var data = context.smsInbox.
                        //Join(context.objects,
                        //sms => sms.Sender,
                        //cont => cont.PhoneNo,
                        //(sms, cont) => new { Synonym = cont.Synonym, Name = cont.Name, Port = sms.GsmSpan, Sender = sms.Sender, Content = sms.Content, RecvDate = sms.Recvtime }

                        //).ToList();

                        return Ok(smsData.Where(sms => sms.RecvDate >= DateTime.Now.Date).ToList());
                       }

        }

            catch(Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("getSendItem")]
        public ICollection<SendItem> GetSendItems()
        {
            try
            {
                List<SendItem> sendItems = context.sendItems.OrderByDescending(data=>data.Id).ToList();
                return sendItems;
            }
          
            catch(Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("searchForSendingSms")]
        public ICollection<SendItem> SearchSendingSms(ReceivedSmsSearch searchValue)
        {
            try
           {
               
                    IEnumerable<SendItem> smsData = context.sendItems;
                    if (!string.IsNullOrEmpty(searchValue.Synonym))
                    {
                        Objects Objects = context.objects.FirstOrDefault(Objects => Objects.Synonym == searchValue.Synonym);

                        smsData = smsData.Where(sms => sms.PhoneNo == Objects.PhoneNo);
                    }
                    if (!string.IsNullOrEmpty(searchValue.StartDate))
                    {
                        if (string.IsNullOrEmpty(searchValue.EndDate))
                        {
                            searchValue.EndDate = searchValue.StartDate;
                        }

                        smsData = smsData.Where(sms => (Convert.ToDateTime(sms.SendingTime.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(searchValue.StartDate)) && ((Convert.ToDateTime(sms.SendingTime.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(searchValue.EndDate))));
                    }
                if (!string.IsNullOrEmpty(searchValue.EndDate))
                {
                    if (string.IsNullOrEmpty(searchValue.StartDate))
                    {
                        searchValue.StartDate = searchValue.EndDate;
                    }

                    smsData = smsData.Where(sms => (Convert.ToDateTime(sms.SendingTime.ToString("yyyy/MM/dd")) >= Convert.ToDateTime(searchValue.StartDate)) && ((Convert.ToDateTime(sms.SendingTime.ToString("yyyy/MM/dd")) <= Convert.ToDateTime(searchValue.EndDate))));
                }

                if (searchValue.Port != 0)
                    {
                        smsData = smsData.Where(sms => sms.Port == searchValue.Port);
                    }
                if (searchValue.StatusCode >= 0)
                {
                    smsData = smsData.Where(sms => sms.StatusId == searchValue.StatusCode);
                }
                if (!string.IsNullOrEmpty(searchValue.Content))
                    {
                        smsData = smsData.Where(sms => sms.Message.Contains(searchValue.Content));
                    }
                if (!string.IsNullOrEmpty(searchValue.Sender))
                    {
                        searchValue.Sender = "+88" + searchValue.Sender;
                        smsData = smsData.Where(sms => sms.PhoneNo == searchValue.Sender);
                    }

                    return smsData.ToList();
                }

            

            catch(Exception ex)
            {
                return null;
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

                                if (i == 1) availablePort.Label = portList[0].Port_1;
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
