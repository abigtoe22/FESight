using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    public class KILocation
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
        public BitArray MemoryLocation { get; set; }
        public int Index { get; set; }

        public KILocation(string name, string hexIndex)
        {
            Name = name;
            Checked = false;
            Index = Convert.ToInt32(hexIndex, 16);
            MemoryLocation = new BitArray(120);
            MemoryLocation[Index] = true;
        }

        public bool HasBeenChecked(BitArray memoryArray)
        {
            if (memoryArray[Index])
                Checked = true;

            return Checked;
        }
    }
}
