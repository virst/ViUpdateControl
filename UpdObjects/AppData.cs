using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UpdObjects
{
    public class AppData
    {
        public class FileData
        {
            public int Id { get; set; }
            public string Nm { get; set; }
            public int Rev { get; set; }
            public string Md5 { get; set; }
            public bool Run { get; set; }
        }

        public int AppId { get; set; }
        public string AppName { get; set; }
        public List<FileData> Files { get; set; } = new List<FileData>();
    }
}
