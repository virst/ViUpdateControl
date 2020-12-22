using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using UpdService.Objects;

namespace UpdService.Controllers
{
    [ApiController]
    [CustomJson]
    [Route("[controller]")]
    public class AppInfoController : ControllerBase
    {
       

        [HttpGet]
        public IEnumerable<App> Get()
        {
            var db = new EE.DbAppContext();
            var t = db.Apps.Include(x => x.Files);
            return t;
        }


        [HttpGet("{id}")]
        public App Get(int id)
        {
            var db = new EE.DbAppContext();
            var a = db.Apps.Include(t=>t.Files).FirstOrDefault(t => t.Id == id);
            return a;
        }

        [HttpGet("Add/{nm}")]
        public int Add(string nm)
        {
            nm = nm.ToLower();
            var db = new EE.DbAppContext();
            var o = db.Apps.FirstOrDefault(t => t.AppName == nm);
            if (o != null)
                return o.Id;
            db.Apps.Add(new App() {AppName = nm});
            db.SaveChanges();
            o = db.Apps.FirstOrDefault(t => t.AppName == nm);
            if (o != null)
                return o.Id;
            return -1;
        }
    }
}
