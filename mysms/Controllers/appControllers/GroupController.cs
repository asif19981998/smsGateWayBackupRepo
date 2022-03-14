using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mysms.DataBasebContext;
using mysms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace mysms.Controllers.appControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        MySmsDbContext context;
        public GroupController(MySmsDbContext mySmsDb)
        {
            context = mySmsDb;
        }
        [HttpPost]
        public IActionResult AddGroup(Group model)
        {
            try
            {
                context.groups.Add(model);
                bool isAdded = context.SaveChanges() > 0;

                if (isAdded)
                {
                   return RedirectToAction("GetGroup");
                }
                else
                {
                    return Ok(new ResponseResult { Result = model, IsSuccess = false, Message = "Failed ! ", });
                }
            }

            catch(Exception ex)
            {
                 return Ok(new ResponseResult { Result = model, IsSuccess = false, Message = ex.Message });
            }
            


        }

        [HttpPut]
        public IActionResult UpdateGroup(Group group)
        {
            try
            {
                context.groups.Update(group);
                bool isUpdated = context.SaveChanges() > 0;

                if (isUpdated)
                {
                    return RedirectToAction("GetGroup");
                }
                else
                {
                    return Ok(new ResponseResult { Result = group, IsSuccess = false, Message = "Failed ! ", });
                }

            }
            catch (Exception ex)
            {
                return Ok(new ResponseResult { Result = group, IsSuccess = false, Message = "Something is Wrong" });
            }
        }


        [HttpGet]

        public IActionResult GetGroup()
        {
            try
            { 
                List<Group> groups = context.groups.ToList();

                return Ok(new ResponseResult { Result = groups, IsSuccess = true, Message = "Successfully Saved"});
            }
            
            catch(Exception ex)
            { 
            
                return Ok(new ResponseResult { Result = null, IsSuccess = false, Message = ex.Message });
            }
        }

        

        }
    }


