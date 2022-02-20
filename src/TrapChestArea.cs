using FESight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
	// TODO: Make abstract class for KILocation and TrapChestArea to inherit commonalities
	public class TrapChestArea
	{
		public string Name { get; set; }
		public List<KILocation> TrapChests { get; set; }
		public KILocationArea AreaMap { get; set; }
		public int Total { get; set; }
		public int Current { get; set; }
		public List<KeyItem> GatingKeyItems { get; set; }
		public bool PushBToJumpSkip { get; set; }

		public TrapChestArea(string name, KILocationArea areaMap, List<KeyItem> gatingItems, bool jumpSkip, int chestTotal)
		{
			Name = name;
			AreaMap = areaMap;
			TrapChests = new List<KILocation>();
			GatingKeyItems = gatingItems;
			PushBToJumpSkip = jumpSkip;
			Total = chestTotal;
			Current = chestTotal;
		}

		public bool IsAvailable(bool hookCleared)
		{
			bool kIsRequiredObtained = true;

			if (Name.Contains("Lunar Subterrane"))
			{
				if ((Flags.Kmoon || Flags.Kunsafe || Flags.Kunsafer) == false)
					return false;
			}

			foreach (var item in GatingKeyItems)
			{
				if (hookCleared && item.Name == "Magma Key")
					continue;

				if (item.Obtained == false)
					kIsRequiredObtained = false;
			}

			if (Flags.OtherPushBToJump)
			{
				if (PushBToJumpSkip == true)
					return true;


				if (hookCleared)
				{
					if (GatingKeyItems.Count > 1 && GatingKeyItems.Where(p => p.Name == "Magma Key").Any())
					{
						foreach (var item in GatingKeyItems.Where(p => p.Name != "Magma Key").ToList())
						{
							if (item.Obtained == false)
								return false;
						}

						return true;
					}
				}
			}

			return kIsRequiredObtained;
		}
	}

	public static class TrapChestAreas
	{
		public static bool TrapChestAreasInitialized { get; set; }
		public static TrapChestArea TowerOfZot { get; set; }
		public static TrapChestArea CastleEblan { get; set; }
		public static TrapChestArea GiantOfBabil { get; set; }
		public static TrapChestArea CaveEblan { get; set; }
		public static TrapChestArea UpperBabil { get; set; }

		public static TrapChestArea Feymarch { get; set; }
		public static TrapChestArea LowerBabil { get; set; }
		public static TrapChestArea SylphCave { get; set; }

		public static TrapChestArea LunarPath { get; set; }
		public static TrapChestArea LunarSubterrane { get; set; }

		public static List<TrapChestArea> ListOfAreas { get; set; }

		public static void InitializeTrapChestAreas()
		{
			if (TrapChestAreasInitialized)
				return;

			TowerOfZot = new TrapChestArea("Tower of Zot", KILocationArea.Overworld, new List<KeyItem> { }, true, 1);
			CastleEblan = new TrapChestArea("Castle Eblan", KILocationArea.Overworld, new List<KeyItem> { }, true, 3);
			GiantOfBabil = new TrapChestArea("Giant of Babil", KILocationArea.Overworld, new List<KeyItem> { KeyItems.DarknessCrystal }, false, 1);
			UpperBabil = new TrapChestArea("Upper Babil", KILocationArea.Overworld, new List<KeyItem> { KeyItems.Hook }, true, 1);
			CaveEblan = new TrapChestArea("Cave Eblan", KILocationArea.Overworld, new List<KeyItem> { KeyItems.Hook }, true, 1);

			Feymarch = new TrapChestArea("Feymarch", KILocationArea.Underground, new List<KeyItem> { KeyItems.MagmaKey }, false, 1);
			LowerBabil = new TrapChestArea("Lower Babil", KILocationArea.Underground, new List<KeyItem> { KeyItems.MagmaKey }, false, 4);
			SylphCave = new TrapChestArea("Sylph Cave", KILocationArea.Underground, new List<KeyItem> { KeyItems.MagmaKey }, false, 7);

			LunarPath = new TrapChestArea("Lunar Path", KILocationArea.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, 1);
			LunarSubterrane = new TrapChestArea("Lunar Subterrane", KILocationArea.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, 9);


			ListOfAreas = new List<TrapChestArea>();
			ListOfAreas.Add(TowerOfZot);
			ListOfAreas.Add(CastleEblan);
			ListOfAreas.Add(GiantOfBabil);
			ListOfAreas.Add(CaveEblan);
			ListOfAreas.Add(Feymarch);
			ListOfAreas.Add(LowerBabil);
			ListOfAreas.Add(SylphCave);
			ListOfAreas.Add(LunarPath);
			ListOfAreas.Add(LunarSubterrane);

			TrapChestAreasInitialized = true;
		}

		public static void ResetChestTotals()
        {
			foreach(var area in ListOfAreas)
            {
				area.Current = area.Total;
            }
        }

		public static List<TrapChestArea> GetOverworldTrapChests()
		{
			List<TrapChestArea> oAreas = new List<TrapChestArea>();
			oAreas = TrapChestAreas.ListOfAreas.Where(p => p.AreaMap == KILocationArea.Overworld).ToList();
			return oAreas;
		}

		public static List<TrapChestArea> GetUnderworldTrapChests()
		{
			List<TrapChestArea> uAreas = new List<TrapChestArea>();
			uAreas = TrapChestAreas.ListOfAreas.Where(p => p.AreaMap == KILocationArea.Underground).ToList();
			return uAreas;
		}

		public static List<TrapChestArea> GetMoonTrapChests()
		{
			List<TrapChestArea> mAreas = new List<TrapChestArea>();
			mAreas = TrapChestAreas.ListOfAreas.Where(p => p.AreaMap == KILocationArea.Moon).ToList();
			return mAreas;
		}
	}
}

