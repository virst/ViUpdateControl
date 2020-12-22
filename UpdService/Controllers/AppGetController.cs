using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpdObjects;

namespace UpdService.Controllers
{
    [ApiController]
    [CustomJson]
    [Route("[controller]")]
    public class AppGetController : ControllerBase
    {
        [HttpGet("get/{fid}/{rev}")]
        public ActionResult Generate(int fid,int rev)
        {
            var di = new DirectoryInfo("FileDataFolder");
            var d = Path.Combine(di.FullName, fid.ToString("D5"));
            var fna = Path.Combine(d, rev + ".bin");
            return File(System.IO.File.OpenRead(fna), "application/octet-stream");
        }


        [HttpGet("info/{id}")]
        public AppData Get(int id)
        {
            var db = new EE.DbAppContext();
            AppData a = new AppData();

            var ap = db.Apps.Include(t => t.Files).ThenInclude(r=>r.Revisions).FirstOrDefault(t => t.Id == id);
            a.AppId = ap.Id;
            a.AppName = ap.AppName;
            foreach (var apFile in ap.Files)
            {
                var fd = new AppData.FileData { Id = apFile.Id, Nm = apFile.Name, Run = apFile.Run };
                var di = new DirectoryInfo("FileDataFolder");
                var d = Path.Combine(di.FullName, apFile.Id.ToString("D5"));
                var rr = db.Revisions.ToList();
                int rev = apFile.Revisions.OrderByDescending(r => r.Rev).FirstOrDefault()?.Rev ?? 0;
                if (rev == 0) continue;
                fd.Rev = rev;
                var fna = Path.Combine(d, rev + ".bin");
                fd.Md5 = Utils.Utils.ComputeMD5Checksum(System.IO.File.OpenRead(fna));
                a.Files.Add(fd);
            }

            return a;
        }
    }
}
