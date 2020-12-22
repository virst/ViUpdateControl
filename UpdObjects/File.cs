using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdService.Objects
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Run { get; set; } = false;

        public List<Revision> Revisions { get; set; } = new List<Revision>();

        public override string ToString()
        {
            return $"[{Id}] " + Name + (Run ? "!" : "");
        }
    }
}
