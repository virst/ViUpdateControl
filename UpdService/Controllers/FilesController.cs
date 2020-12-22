using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UpdService.Objects;
using File = UpdService.Objects.File;

namespace UpdService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private const string FileDataFolder = "FileDataFolder";

        [HttpGet("Add/{aid}/{nm}")]
        public File Add(int aid, string nm)
        {
            return Add(aid, nm, 0);
        }

        [HttpGet("SetRun/{fid}/{rn}")]
        public int SetRun(int fid,int rn)
        {
            var db = new EE.DbAppContext();
            var f = db.Files.FirstOrDefault(t => t.Id == fid);
            if (f == null) return 0;
            f.Run = rn > 0;
            db.SaveChanges();
            return 1;
        }

        [HttpPost("Load/{aid}/{fn}")]
        public int Load(int aid,string fn)
        {
            var f = Add(aid, fn);
            var fid = f.Id;
            var di = new DirectoryInfo("FileDataFolder");
            var d = Path.Combine(di.FullName,fid.ToString("D5"));
            Directory.CreateDirectory(d);
            var input = Request.BodyReader.AsStream();
            byte[] bb;
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                bb = ms.ToArray();
            }
            var db = new EE.DbAppContext();
            f = db.Files.Include(r=>r.Revisions).FirstOrDefault(t => t.Id == fid);
            int rev = f?.Revisions.OrderByDescending(r => r.Rev).FirstOrDefault()?.Rev ?? 0;
            rev++;
            var fna = Path.Combine(d, rev + ".bin");
            System.IO.File.WriteAllBytes(fna,bb);
            f.Revisions.Add(new Revision() { Rev = rev });
            db.SaveChanges();
            return -1;
        }

        [HttpGet("Add/{aid}/{nm}/{run}")]
        public File Add(int aid, string nm, int run)
        {
            nm = nm.ToLower();
            var db = new EE.DbAppContext();
            var a = db.Apps.Include(t => t.Files).FirstOrDefault(t => t.Id == aid);
            if (a == null) return null;
            var f = a.Files.FirstOrDefault(t => t.Name == nm);
            if (f != null) return f;
            f = new File() { Name = nm, Run = run == 1 };
            db.Files.Add(f);
            a.Files.Add(f);
            db.SaveChanges();
            f = a.Files.FirstOrDefault(t => t.Name == nm);
            return f;
        }
    }
}
