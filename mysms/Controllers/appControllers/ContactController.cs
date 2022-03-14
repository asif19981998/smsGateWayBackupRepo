using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mysms.DataBasebContext;
using mysms.Models;
using mysms.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Controllers.appControllers
{

    public class ViewDropDown
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public string PhoneNo { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        MySmsDbContext context;
        public ContactController(MySmsDbContext mySmsDb)
        {
            context = mySmsDb;
        }
        [EnableCors]
        [HttpGet]
        public ICollection<Objects> GetAll()
        {
            try
            {

                List<Objects> data = context.objects.OrderByDescending(c => c.Id).ToList();

                    return data;
               
            }
            catch(Exception ex)
            {
                return null;
            }
            

        }
        [HttpGet]
        [Route("GetContactForDD")]

        public ICollection<ViewDropDown> GetContactForDD()
        {
            try
            {
               
                    List<Objects> data = context.objects.ToList();
                    List<ViewDropDown> dpData = new List<ViewDropDown>();
                    foreach (var item in data)
                    {
                        ViewDropDown view = new ViewDropDown();
                        view.Value = item.Synonym;
                        view.Label = item.Name;
                        view.PhoneNo = item.PhoneNo;
                        dpData.Add(view);
                    }
                    return dpData;
               

            }

            catch(Exception ex)
            {
                return null;
            }


        }
        [HttpPost]
        [Route("postContact")]
        public ActionResult<Objects> Save(Objects models)
        {
            try
            {
               
                    context.objects.Add(models);
                    bool isSaved = context.SaveChanges() > 0;
                    if (isSaved)
                    {
                        return Ok("Successfully Updated");
                    }
                    //Asif Do
                    return Ok("Update Failed");

           
            
            }

            catch(Exception ex)
            {
                return null;
            }
            

        }

        [HttpPost]
        [Route("searchByPhoneNo")]
        public string SearchByPhoneNo(Models.ViewModel.SearchAbleData searchValue)
        {
            try
            {
               
                    var data = context.objects.FirstOrDefault(c => c.PhoneNo == searchValue.PhoneNo);
                    if (data is null)
                    {
                        return "";
                    }
                    else return data.Name;
                

            }

            catch(Exception ex){
                return null;
            }


        }

        [HttpPut]

        public string UdpateObject(Objects model)
        {

            try
            {
                context.objects.Update(model);
                bool isSaved = context.SaveChanges() > 0;
                if (isSaved) return "Success";
                else return "Not Found";

            }


            catch (Exception ex)
            {
                return "No data Saved";
            }


        }

        [HttpPost]
        [Route("searchByGroupName")]
        public IActionResult GetContactByGroupName(ObjectsSearch objectsSearch)
        {
            try
            {
                List<Objects> objects = context.objects.Where(ob => ob.Name.ToLower().Contains(objectsSearch.SearchValue.ToLower())).ToList();
                if(objects is not null)
                {
                    return Ok(new ResponseResult { Result = objects, IsSuccess = true, Message = "Successfully Get" });
                }
                else
                {
                    return Ok(new ResponseResult { Result = objects, IsSuccess = false, Message = "No such Contact is Found " });
                }
            }
            catch(Exception ex)
            {
                return Ok(new ResponseResult { Result = null, IsSuccess = false, Message = "Something is Wrong" });
            }
          

        }

        [HttpPost]
        [Route("searchBySynonym")]
        public IActionResult GetContactByName(ObjectsSearch objectsSearch)
        {
            try
            {
                Object obj = context.objects.FirstOrDefault(obj => obj.Synonym.Trim() == objectsSearch.SearchValue.Trim());
                if (obj is not null)
                {
                    return Ok(new ResponseResult { Result = obj, IsSuccess = true, Message = "Successfully Get" });
                }
                else
                {
                    return Ok(new ResponseResult { Result = obj, IsSuccess = false, Message = "No such Contact is Found " });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseResult { Result = null, IsSuccess = false, Message = "Something is Wrong" });
            }


        }
    }
}
