using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdService.Objects
{
    public class App
    {
        public int Id { get; set; }

        public string AppName { get; set; }
        public List<File> Files { get; set; } = new List<File>();

        public override string ToString()
        {
            return $"[{Id}] " + AppName;
        }
    }
}
