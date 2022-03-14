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
    public class PortSettingController : ControllerBase
    {
        MySmsDbContext context;
        public PortSettingController(MySmsDbContext mySmsDb)
        {
            context = mySmsDb;
        }
        // GET: api/<PortSettingController>
        [HttpGet]
        public ICollection<PortSetting> Get()
        {
            try
            {
                var portList =  context.PortSettings.ToList();
                return portList;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        // GET api/<PortSettingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PortSettingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PortSettingController>/5
        [HttpPut]
        public ICollection<PortSetting> Put(PortSetting portSetting)
        {
            try
            {
                context.PortSettings.Update(portSetting);
                bool isUpdated = context.SaveChanges() > 0;

                if (isUpdated) return context.PortSettings.ToList();
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        // DELETE api/<PortSettingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
