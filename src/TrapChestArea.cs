using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    public class TrapChestArea
    {
        public string Name { get; set; }
        public List<KILocation> TrapChests { get; set; }
        public KILocationArea AreaMap { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }

        public TrapChestArea(string name, KILocationArea areaMap)
        {
            Name = name;
            AreaMap = areaMap;
            TrapChests = new List<KILocation>();
        }

        public int GetRemainingChests()
        {
            return TrapChests.Where(p => p.Checked == false).Count();
        }

        public bool IsAvailable(bool hookCleared)
        {
            return TrapChests.First().IsAvailable(hookCleared);
        }
            
    }

    public static class TrapChestAreas
    {
        public static List<TrapChestArea> ListOfAreas { get; set; }
    }
}
