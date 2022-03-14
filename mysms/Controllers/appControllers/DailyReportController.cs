using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using mysms.DataBasebContext;
using mysms.DataBaseContext;
using mysms.Models;
using mysms.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Controllers.appControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportController : ControllerBase
    {
        JabloNetDbContext _context;
        MySmsDbContext _smsDbContext;
        public DailyReportController(JabloNetDbContext jabloNetContext,MySmsDbContext smsDbContext)
        {
            _context = jabloNetContext;
            _smsDbContext = smsDbContext;
        }
        [HttpGet]
        //[Route("/setDailyReportOpeningTableData")]
        public void SetDailyReportTableData()
        {
            try
            {
                var objectData = _smsDbContext.objects.Select(data => new { data.Synonym, data.Name }).ToList();
                var dailyOpeningData = _smsDbContext.dailyReportOpening.AsQueryable();
                var objectDataInQueryAbleMood = objectData.AsQueryable();
                List<DailyReportOpening> dailyReportOpenings = objectDataInQueryAbleMood.Where(o => !dailyOpeningData.Select(d => d.SystemId).Contains(o.Synonym))
                                           .Select(nopdata =>
                                                     new DailyReportOpening
                                                     {
                                                         SystemId = nopdata.Synonym,
                                                         ObjectName = nopdata.Name,



                                                     }).ToList();
                if (dailyReportOpenings.Count > 0)
                {
                    _smsDbContext.dailyReportOpening.AddRange(dailyReportOpenings);
                    _smsDbContext.SaveChanges();
                }
            }

            catch(Exception ex)
            {
                
            }
            

 }
    
       [HttpGet]
       [Route("setDailyReportOpeningData")]

       public void SetDailyReportOpeningData()
        {
            try
            {
                var objectData = _smsDbContext.objects.Select(data => new { data.Synonym, data.Name,data.PhoneNo }).ToList();
                var dailyOpeningData = _smsDbContext.dailyReportOpening.AsQueryable();
                var objectDataInQueryAbleMood = objectData.AsQueryable();

                List<DailyReportOpening> dailyReportOpenings = objectData.Where(o => !dailyOpeningData.Select(d => d.SystemId).Contains(o.Synonym))
                                           .Select(nopdata =>
                                                     new DailyReportOpening
                                                     {
                                                         SystemId = nopdata.Synonym,
                                                         ObjectName = nopdata.Name,
                                                         PhoneNo=nopdata.PhoneNo,
                                                         ARC = "No",
                                                         SMS = "No"
                                                     }).ToList();
                if (dailyReportOpenings.Count > 0)
                {
                    _smsDbContext.dailyReportOpening.AddRange(dailyReportOpenings);
                    _smsDbContext.SaveChanges();
                }

            }

            catch(Exception ex)
            {

            }
           
        }
        [HttpGet]
        [Route("setDailyReportClosingData")]
        public void SetDailyReportClosingTableData()
        {

            try
            {
                var objectData = _smsDbContext.objects.Select(data => new { data.Synonym, data.Name,data.PhoneNo }).ToList();
                var dailyClosingData = _smsDbContext.dailyReportClosing.AsQueryable();
                var objectDataInQueryAbleMood = objectData.AsQueryable();

                List<DailyReportClosing> dailyReportClosings = objectData.Where(o => !dailyClosingData.Select(d => d.SystemId).Contains(o.Synonym))
                                           .Select(nopdata =>
                                                     new DailyReportClosing
                                                     {
                                                         SystemId = nopdata.Synonym,
                                                         ObjectName = nopdata.Name,
                                                         PhoneNo = nopdata.PhoneNo,
                                                         ARC ="No",
                                                         SMS = "No"


                                                     }).ToList();
                if (dailyReportClosings.Count > 0)
                {
                    _smsDbContext.dailyReportClosing.AddRange(dailyReportClosings);
                    _smsDbContext.SaveChanges();
                }

            }

            catch (Exception ex)
            {

            }


        }


      [HttpGet]
      [Route("getClosingReport")]
      public async Task<ICollection<DailyReportClosing>> GetDailyReportClosing()
        {
            try
            {
                return  _smsDbContext.dailyReportClosing.ToList();
            }

            catch(Exception ex)
            {
                return null;
            }
           
        }


        [HttpPut]

        public string UdpateDailyReportClosing(DailyReportClosing model)
        {

            try
            {
                _smsDbContext.dailyReportClosing.Update(model);
                bool isSaved = _smsDbContext.SaveChanges() > 0;
                if (isSaved) return "Success";
                else return "Not Found";

            }
                
           
            catch(Exception ex)
            {
                return "No data Saved";
            }
            

        }

        [HttpGet]
        [Route("getUpdatedClosingReport")]

        public ICollection<DailyReportClosing> GetUpdatedClosingReport(UpdatedReportSearch model)
        {
            try
            {
                if (model.Type == "arc")
                {
                    string cs = "Server=192.168.88.35,1433;Database=JabloNET;User Id=sa;password=SQLjablonet2020;Trusted_Connection=False;MultipleActiveResultSets=true;";

                    string ReportDate = model.ReportDate.ToString("yyyy/MM/dd");
                    ICollection<DailyReportClosing> dailyClosings = _smsDbContext.dailyReportClosing.ToList();
                    List<DailyReportClosing> dailyReportClosingsTemp = new List<DailyReportClosing>();
                    SqlConnection con = new SqlConnection(cs);
                    string tableName = $"dbo.objectSignal_{model.ReportDate.Year}_{ReportDate.Split("/")[1]}_{ReportDate.Split("/")[2]}";
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM {tableName} WHERE  (Code ='1400' OR Code = '1401' OR Code = '1409')";
                    SqlDataReader sdr = cmd.ExecuteReader();


                    List<string> listOfYesId = new List<string>();
                        List<ReportingModel> listOfYesIdOrObject = new List<ReportingModel>();
                    while (sdr.Read())
                    {
                        ReportingModel reportingModel = new ReportingModel();
                        reportingModel.ObjectSynonym = sdr["ObjectSynonym"].ToString();
                        reportingModel.TimeObject = Convert.ToDateTime(sdr["TimeObject"]);

                        listOfYesId.Add(sdr["ObjectSynonym"].ToString());
                        listOfYesIdOrObject.Add(reportingModel);
                    }
                    foreach (var item in dailyClosings)
                    {
                        if (listOfYesId.Contains(item.SystemId))
                        {

                            
                            var temp = listOfYesIdOrObject.Find(lidObj => lidObj.ObjectSynonym == item.SystemId);
                            if(temp.TimeObject.Hour >= model.ReportDate.Hour)
                            {
                                item.ARC = "Yes";
                                item.ArcStopTime = temp.TimeObject;
                            }

                            else
                            {
                                item.ARC = "No";
                                item.ArcStopTime = temp.TimeObject;
                            }
                           

                        }
                        else
                        {
                            item.ARC = "No";
                            item.ArcStopTime = null;


                        }
                        dailyReportClosingsTemp.Add(item);
                           

                         }


                    con.Close();
                    _smsDbContext.dailyReportClosing.UpdateRange(dailyReportClosingsTemp);
                    _smsDbContext.SaveChanges() ;
                    return _smsDbContext.dailyReportClosing.ToList();
                }
                else
                {
                    ICollection<DailyReportClosing> dailyClosings = _smsDbContext.dailyReportClosing.ToList();
                    var synonymwiseMessages = _smsDbContext.dailyReportClosing.
                                                  Join(
                                                     _smsDbContext.objects,
                                                     dailyClosings => dailyClosings.SystemId,
                                                     contacts => contacts.Synonym,
                                                     (dailyClosings, contacts) => new { PhoneNo = contacts.PhoneNo, SystemId = dailyClosings.SystemId })
                                                  .Join(_smsDbContext.smsInbox,
                                                  dc => dc.PhoneNo,
                                                  smsInbox => smsInbox.Sender,
                                                  (dc, smsInbox) => new { SynonymId = dc.SystemId, Content = smsInbox.Content, ReceivedDate = smsInbox.Recvtime });

                   
                    var inboxData = synonymwiseMessages.ToList().Where(sms => sms.ReceivedDate.Date == model.ReportDate.Date);
                    var systemIdAndDateList = inboxData.Where(data =>
                      data.Content.ToLower().Contains("WATCH mode Closed".ToLower()) ||
                      data.Content.ToLower().Contains("Unset".ToLower()) ||
                      data.Content.ToLower().Contains("Sleep mode".ToLower()) ||
                      data.Content.ToLower().Contains("Disarmed".ToLower())


                      && (data.ReceivedDate >= model.ReportDate)).Select(system => new { system.SynonymId, system.ReceivedDate });
                    List<string> synonymIdList = new List<string>();
                    foreach (var sysIdAndDate in systemIdAndDateList)
                    {
                        synonymIdList.Add(sysIdAndDate.SynonymId);

                    }
                    foreach (var item in dailyClosings)
                    {
                        if (synonymIdList.Contains(item.SystemId))
                        {
                            var updatedData = systemIdAndDateList.FirstOrDefault(data => data.SynonymId == item.SystemId);
                            item.SMS = "Yes";
                            item.SmsStopTime = updatedData.ReceivedDate;
                        }
                        
                        _smsDbContext.dailyReportClosing.Update(item);
                    }
                    _smsDbContext.SaveChanges();
                    return _smsDbContext.dailyReportClosing.ToList();
                }

            }

            catch(Exception ex)
            {
                return null;
            }
           
        }

        [HttpPost]
        [Route("SearchByClosingARCColumn")]

        public ICollection<DailyReportClosing> GetClosingArcColumnData(string searchData)
        {
            try
            {
               return  _smsDbContext.dailyReportClosing.Where(dclosing => dclosing.ARC.ToLower() == searchData.ToLower()).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        [Route("SearchByOpeningARCColumn")]

        public ICollection<DailyReportOpening> GetOpeningArcColumnData(string searchData)
        {
            try
            {
                return _smsDbContext.dailyReportOpening.Where(dclosing => dclosing.ARC.ToLower() == searchData.ToLower()).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        [HttpGet]
        [Route("getOpeningReport")]
        public ICollection<DailyReportOpening> GetDailyReportOpening()
        {
            try
            {
                return _smsDbContext.dailyReportOpening.ToList();
            }

            catch(Exception ex)
            {

                return null;
            }
            
        }

        [HttpPost]
        [Route("getUpdatedOpeningReport")]

        public ICollection<DailyReportOpening> GetUpdatedOpeningReport(UpdatedReportSearch model)
        {
            try
            {
                if (model.Type == "arc")
                {
                    string cs = "Server=192.168.88.35,1433;Database=JabloNET;User Id=sa;password=SQLjablonet2020;Trusted_Connection=False;MultipleActiveResultSets=true;";

                    string ReportDate = model.ReportDate.ToString("yyyy/MM/dd");
                    ICollection<DailyReportOpening> dailyClosings = _smsDbContext.dailyReportOpening.ToList();
                    List<DailyReportOpening> dailyReportOpeningsTemp = new List<DailyReportOpening>();
                    SqlConnection con = new SqlConnection(cs);
                    string tableName = $"dbo.objectSignal_{model.ReportDate.Year}_{ReportDate.Split("/")[1]}_{ReportDate.Split("/")[2]}";
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM {tableName} WHERE  (Code ='3400' OR Code = '3401' OR Code = '3409')";
                    SqlDataReader sdr = cmd.ExecuteReader();


                    List<string> listOfYesId= new List<string>();
                        List<ReportingModel> listOfYesIdOrObject = new List<ReportingModel>();
                    while (sdr.Read())
                        {
                        ReportingModel reportingModel= new ReportingModel();
                        reportingModel.ObjectSynonym = sdr["ObjectSynonym"].ToString();
                        reportingModel.TimeObject =Convert.ToDateTime(sdr["TimeObject"]);

                        listOfYesId.Add(sdr["ObjectSynonym"].ToString());
                        listOfYesIdOrObject.Add(reportingModel);
                    }
                        foreach (var item in dailyClosings)
                          {
                        if (listOfYesId.Contains(item.SystemId))
                        {
                            var temp = listOfYesIdOrObject.Find(lidObj => lidObj.ObjectSynonym == item.SystemId);
                            if (temp.TimeObject.Hour >= model.ReportDate.Hour)
                            {
                                item.ARC = "Yes";
                                item.ArcStopTime = temp.TimeObject;
                            }

                            else
                            {
                                item.ARC = "No";
                                item.ArcStopTime = temp.TimeObject;
                            }
                        }
                        else
                        {
                            item.ARC = "No";
                            item.ArcStopTime = null;


                        }
                        dailyReportOpeningsTemp.Add(item);
                           

                         }

                    con.Close();
                    _smsDbContext.dailyReportOpening.UpdateRange(dailyReportOpeningsTemp);
                    _smsDbContext.SaveChanges() ;
                    return _smsDbContext.dailyReportOpening.ToList();
                }


                else
                {
                    ICollection<DailyReportOpening> dailyOpenings = _smsDbContext.dailyReportOpening.ToList();
                    var synonymwiseMessages = _smsDbContext.dailyReportOpening.
                                                  Join(
                                                     _smsDbContext.objects,
                                                     dailyOpenings => dailyOpenings.SystemId,
                                                     contacts => contacts.Synonym,
                                                     (dailyOpenings, contacts) => new { PhoneNo = contacts.PhoneNo, SystemId = dailyOpenings.SystemId })
                                                  .Join(_smsDbContext.smsInbox,
                                                  dc => dc.PhoneNo,
                                                  smsInbox => smsInbox.Sender,
                                                  (dc, smsInbox) => new { SynonymId = dc.SystemId, Content = smsInbox.Content, ReceivedDate = smsInbox.Recvtime });


                    
                    if (synonymwiseMessages.Count() > 0)
                    {
                        var d1 = model.ReportDate.Date;
                       var inboxData = synonymwiseMessages.ToList().Where(sms => sms.ReceivedDate.Date == model.ReportDate.Date);
                       var systemIdAndDateList = inboxData.Where(data=>
                       data.Content.ToLower().Contains("WATCH mode opened".ToLower()) ||
                       data.Content.ToLower().Contains("Set".ToLower()) ||
                       data.Content.ToLower().Contains("Watch mode".ToLower()) ||
                       data.Content.ToLower().Contains("armed".ToLower()) 


                       && (data.ReceivedDate >= model.ReportDate)).Select(system => new { system.SynonymId, system.ReceivedDate });


                        List<string> synonymIdList = new List<string>();
                         foreach(var sysIdAndDate in systemIdAndDateList)
                        {
                            synonymIdList.Add(sysIdAndDate.SynonymId);

                        }
                        foreach (var item in dailyOpenings)
                        {
                            if (synonymIdList.Contains(item.SystemId))
                            {
                                var updatedData = systemIdAndDateList.FirstOrDefault(data => data.SynonymId == item.SystemId);
                                item.SMS = "Yes";
                                item.SmsStopTime = updatedData.ReceivedDate;
                            }

                            _smsDbContext.dailyReportOpening.Update(item);
                        }
                    }
                    else
                    {
                        foreach (var item in dailyOpenings)
                        {
                            item.SMS = "No";

                            _smsDbContext.dailyReportOpening.Update(item);
                        }
                    }


                   
                    _smsDbContext.SaveChanges();
                    return _smsDbContext.dailyReportOpening.ToList();
                }
            }

            catch(Exception ex)
            {
                return null;
            }
           
        
        
        }



    }
}
