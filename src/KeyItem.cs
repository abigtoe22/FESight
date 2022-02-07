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

    public static class KeyItems
    {
        public static bool KeyItemsInitialized { get; set; }
        public static List<KeyItem> KeyItemList { get; set; }
        public static KeyItem Crystal { get; set; }
        public static KeyItem Pass { get; set; }
        public static KeyItem Hook { get; set; }
        public static KeyItem DarknessCrystal { get; set; }
        public static KeyItem EarthCrystal { get; set; }
        public static KeyItem TwinHarp { get; set; }
        public static KeyItem Package { get; set; }
        public static KeyItem SandRuby { get; set; }
        public static KeyItem BaronKey { get; set; }
        public static KeyItem MagmaKey { get; set; }
        public static KeyItem TowerKey { get; set; }
        public static KeyItem LucaKey { get; set; }
        public static KeyItem Adamant { get; set; }
        public static KeyItem LegendSword { get; set; }
        public static KeyItem Pan { get; set; }
        public static KeyItem Spoon { get; set; }
        public static KeyItem RatTail { get; set; }
        public static KeyItem PinkTail { get; set; }

        public static void InitializeKeyItems()
        {
            Crystal = new KeyItem("Crystal", "1THECrystal", 16);
            Pass = new KeyItem("Pass", "2Pass", 17);
            Hook = new KeyItem("Hook", "3Hook", 8);
            DarknessCrystal = new KeyItem("Darkness Crystal", "4DarkCrystal", 10);
            EarthCrystal = new KeyItem("Earth Crystal", "5EarthCrystal", 5);
            TwinHarp = new KeyItem("Twin Harp", "6TwinHarp", 4);
            Package = new KeyItem("Package", "7Package", 0);
            SandRuby = new KeyItem("SandRuby", "8SandRuby", 1);
            BaronKey = new KeyItem("Baron Key", "9BaronKey", 3);
            MagmaKey = new KeyItem("Magma Key", "10MagmaKey", 6);
            TowerKey = new KeyItem("Tower Key", "11Towerkey", 7);
            LucaKey = new KeyItem("Luca Key", "12LucaKey", 9);
            Adamant = new KeyItem("Adamant", "13Adamant", 12);
            LegendSword = new KeyItem("Legend Sword", "14LegendSword", 2);
            Pan = new KeyItem("Pan", "15Pan", 13);
            Spoon = new KeyItem("Spoon", "16Spoon", 14);
            RatTail = new KeyItem("Rat Tail", "17RatTail", 11);
            PinkTail = new KeyItem("Pink Tail", "18PinkTail", 15);

            KeyItemList = new List<KeyItem>();
            KeyItemList.Add(Crystal);
            KeyItemList.Add(Pass);
            KeyItemList.Add(Hook);
            KeyItemList.Add(DarknessCrystal);
            KeyItemList.Add(EarthCrystal);
            KeyItemList.Add(TwinHarp);
            KeyItemList.Add(Package);
            KeyItemList.Add(SandRuby);
            KeyItemList.Add(BaronKey);
            KeyItemList.Add(MagmaKey);
            KeyItemList.Add(TowerKey);
            KeyItemList.Add(LucaKey);
            KeyItemList.Add(Adamant);
            KeyItemList.Add(LegendSword);
            KeyItemList.Add(Pan);
            KeyItemList.Add(Spoon);
            KeyItemList.Add(RatTail);
            KeyItemList.Add(PinkTail);

            KeyItemsInitialized = true;
        }

        public static int KeyItemObtainedCount()
        {
            return KeyItemList.Where(p => p.Name != "Pass" && p.Obtained).Count();
        }
    }
}
