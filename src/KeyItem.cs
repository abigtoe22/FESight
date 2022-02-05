using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FESight
{
    public class KeyItem
    {
        public string Name { get; set; }
        public int MemoryIndex { get; set; }
        public bool Obtained { get; set; }
        public bool Used { get; set; }
        public PictureBox PictureBox { get; set; }
        public string IconPrefix { get; set; }
        public string IconLocation
        {
            get
            {
                string result = "../src/Icons/FFIVFE-Icons-";
                result += IconPrefix;

                if(Obtained && Used)
                {
                    result += "-Check.png";
                }
                else if(Obtained && !Used)
                {
                    result += "-Color.png";
                }
                else
                {
                    result += "-Gray.png";
                }

                return result;
            }
        }

        public KeyItem(string name, string iconPrefix, int index)
        {
            Name = name;
            Obtained = false;
            Used = false;
            IconPrefix = iconPrefix;
            MemoryIndex = index;
        }
    }
}
