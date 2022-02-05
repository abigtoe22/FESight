using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    public class Metadata
    {
        public string version { get; set; }
        public string flags { get; set; }
        public string binary_flags { get; set; }
        public string seed { get; set; }
        public List<string> objectives { get; set; }

    }
}
