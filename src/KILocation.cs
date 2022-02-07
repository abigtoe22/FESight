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
		public KILocationType LocationType { get; set; }
		public List<KeyItem> GatingKeyItems { get; set; }
		public bool PushBToJumpSkip { get; set; }
		public bool Available { get; set; }

		public KILocation(string name, string hexIndex, KILocationType type, List<KeyItem> gates, bool jumpSkip)
		{
			Name = name;
			Checked = false;
			Index = Convert.ToInt32(hexIndex, 16);
			MemoryLocation = new BitArray(120);
			MemoryLocation[Index] = true;
			GatingKeyItems = gates;
			LocationType = type;
			PushBToJumpSkip = jumpSkip;
			Available = false;
		}

		public bool HasBeenChecked(BitArray memoryArray)
		{
			if (memoryArray[Index])
				Checked = true;

			return Checked;
		}
	}

	public enum KILocationType
	{
		Main,
		Summon,
		Moon,
		Trap,
		Objective,
		Golbez
	}

	public static class KILocations
	{
		public static bool KILocationsInitialized { get; set; }
		public static List<KILocation> ListOfKILocations { get; set; }

		//Main
		public static KILocation ObjectiveCompletion { get; set; }
		public static KILocation DMist { get; set; }
		public static KILocation EdwardToroia { get; set; }
		public static KILocation StartingItem { get; set; }
		public static KILocation AntlionNest { get; set; }
		public static KILocation DefendingFabul { get; set; }
		public static KILocation MtOrdeals { get; set; }
		public static KILocation BaronInn { get; set; }
		public static KILocation BaronCastleKing { get; set; }
		public static KILocation CaveMagnes { get; set; }
		public static KILocation TowerOfZot { get; set; }
		public static KILocation LowerBabilBoss { get; set; }
		public static KILocation SuperCannon { get; set; }
		public static KILocation DwarfCastle { get; set; }
		public static KILocation SealedCave { get; set; }
		public static KILocation FeymarchChest { get; set; }
		public static KILocation AdamantCave { get; set; }
		public static KILocation Sheila1 { get; set; }
		public static KILocation Sheila2 { get; set; }

		//Summon
		public static KILocation FeymarchQueen { get; set; }
		public static KILocation FeymarchKing { get; set; }
		public static KILocation OdinThrone { get; set; }
		public static KILocation SylphTurnIn { get; set; }
		public static KILocation CaveBahamut { get; set; }

		//Moon
		public static KILocation MurasameAltar { get; set; }
		public static KILocation CrystalSwordAltar { get; set; }
		public static KILocation WhiteSpearAltar { get; set; }
		public static KILocation RibbonChest1 { get; set; }
		public static KILocation RibbonChest2 { get; set; }
		public static KILocation MasamuneAltar { get; set; }

		//Trap
		public static KILocation TrapZot { get; set; }
		public static KILocation TrapEblan1 { get; set; }
		public static KILocation TrapEblan2 { get; set; }
		public static KILocation TrapEblan3 { get; set; }
		public static KILocation TrapLowerBabil1 { get; set; }
		public static KILocation TrapLowerBabil2 { get; set; }
		public static KILocation TrapLowerBabil3 { get; set; }
		public static KILocation TrapLowerBabil4 { get; set; }
		public static KILocation TrapCaveEblan { get; set; }
		public static KILocation TrapUpperBabil { get; set; }
		public static KILocation TrapFeymarch { get; set; }
		public static KILocation TrapSylph1 { get; set; }
		public static KILocation TrapSylph2 { get; set; }
		public static KILocation TrapSylph3 { get; set; }
		public static KILocation TrapSylph4 { get; set; }
		public static KILocation TrapSylph5 { get; set; }
		public static KILocation TrapSylph6 { get; set; }
		public static KILocation TrapSylph7 { get; set; }
		public static KILocation TrapGiant { get; set; }
		public static KILocation TrapLunarPath { get; set; }

		//Lunar Core Trap
		public static KILocation TrapLunarCore1 { get; set; }
		public static KILocation TrapLunarCore2 { get; set; }
		public static KILocation TrapLunarCore3 { get; set; }
		public static KILocation TrapLunarCore4 { get; set; }
		public static KILocation TrapLunarCore5 { get; set; }
		public static KILocation TrapLunarCore6 { get; set; }
		public static KILocation TrapLunarCore7 { get; set; }
		public static KILocation TrapLunarCore8 { get; set; }
		public static KILocation TrapLunarCore9 { get; set; }

		//LolBez
		public static KILocation Golbez { get; set; }


		public static void InitializeKILocations(bool objective, bool main, bool summon, bool moon, bool trap, bool unSafe, bool noFree)
        {
			ListOfKILocations = new List<KILocation>();
			
			StartingItem = new KILocation("Starting Item", "0x0020", KILocationType.Main, new List<KeyItem>(), true);
			AntlionNest = new KILocation("Antlion Nest", "0x0021", KILocationType.Main, new List<KeyItem>(), true);
			DefendingFabul = new KILocation("Fabul Defense", "0x0022", KILocationType.Main, new List<KeyItem>(), true);
			MtOrdeals = new KILocation("Mt. Ordeals", "0x0023", KILocationType.Main, new List<KeyItem>(), true);
			BaronInn = new KILocation("Baron Inn", "0x0024", KILocationType.Main, new List<KeyItem>(), true);
			BaronCastleKing = new KILocation("Baron Castle King", "0x0025", KILocationType.Main, new List<KeyItem> { KeyItems.BaronKey }, true);
			EdwardToroia = new KILocation("Troia Castle", "0x0026", KILocationType.Main, new List<KeyItem>(), true);
			CaveMagnes = new KILocation("Cave Magnes", "0x0027", KILocationType.Main, new List<KeyItem> { KeyItems.TwinHarp }, true);
			TowerOfZot = new KILocation("Tower of Zot", "0x0028", KILocationType.Main, new List<KeyItem> { KeyItems.EarthCrystal }, false);
			LowerBabilBoss = new KILocation("Lower Babil Boss", "0x0029", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey }, true);
			SuperCannon = new KILocation("Lower Babil Super Cannon", "0x002A", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.TowerKey }, false);
			DwarfCastle = new KILocation("Dwarf Castle", "0x002B", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey }, true);
			SealedCave = new KILocation("Sealed Cave", "0x002C", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.LucaKey }, false);
			FeymarchChest = new KILocation("Feymarch Chest", "0x002D", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey }, true);
			AdamantCave = new KILocation("Adamant Cave", "0x002E", KILocationType.Main, new List<KeyItem> { KeyItems.Hook, KeyItems.RatTail }, false);
			Sheila1 = new KILocation("Sheila 1/Sylph", "0x002F", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey }, true);
			Sheila2 = new KILocation("Sheila 2/Pan", "0x0030", KILocationType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.Pan }, false);

			FeymarchQueen = new KILocation("Asura Spot", "0x0031", KILocationType.Summon, new List<KeyItem> { KeyItems.MagmaKey }, true);
			FeymarchKing = new KILocation("Leviathan Spot", "0x0032", KILocationType.Summon, new List<KeyItem> { KeyItems.MagmaKey }, true);
			OdinThrone = new KILocation("Baron Castle Odin", "0x0033", KILocationType.Summon, new List<KeyItem> { KeyItems.BaronKey }, true);
			SylphTurnIn = new KILocation("Sylph Cave", "0x0034", KILocationType.Summon, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.Pan }, false);
			CaveBahamut = new KILocation("Cave Bahamut", "0x0035", KILocationType.Summon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);

			MurasameAltar = new KILocation("Murasame Altar", "0x0036", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			CrystalSwordAltar = new KILocation("Crystal Sword Altar", "0x0037", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			WhiteSpearAltar = new KILocation("White Spear Altar", "0x0038", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			RibbonChest1 = new KILocation("Ribbon Room Chest 1", "0x0039", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			RibbonChest2 = new KILocation("Ribbon Room Chest 2", "0x003A", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			MasamuneAltar = new KILocation("Masamune Altar", "0x003B", KILocationType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false);

			TrapZot = new KILocation("Zot Trap Chest", "0x003C", KILocationType.Trap, new List<KeyItem>(), true);
			TrapEblan1 = new KILocation("Eblan Trap Chest 1", "0x003D", KILocationType.Trap, new List<KeyItem>(), true);
			TrapEblan2 = new KILocation("Eblan Trap Chest 2", "0x003E", KILocationType.Trap, new List<KeyItem>(), true);
			TrapEblan3 = new KILocation("Eblan Trap Chest 3", "0x003F", KILocationType.Trap, new List<KeyItem>(), true);
			TrapLowerBabil1 = new KILocation("Lower Babil Trap Chest 1", "0x0040", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapLowerBabil2 = new KILocation("Lower Babil Trap Chest 2", "0x0041", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapLowerBabil3 = new KILocation("Lower Babil Trap Chest 3", "0x0042", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapLowerBabil4 = new KILocation("Lower Babil Trap Chest 4", "0x0043", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapCaveEblan = new KILocation("Cave Eblan Trap Chest", "0x0044", KILocationType.Trap, new List<KeyItem> { KeyItems.Hook }, true);
			TrapUpperBabil = new KILocation("Upper Babil Trap Chest", "0x0045", KILocationType.Trap, new List<KeyItem> { KeyItems.Hook }, true);
			TrapFeymarch = new KILocation("Feymarch Trap Chest", "0x0046", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph1 = new KILocation("Sylph Cave Trap Chest 1", "0x0047", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph2 = new KILocation("Sylph Cave Trap Chest 2", "0x0048", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph3 = new KILocation("Sylph Cave Trap Chest 3", "0x0049", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph4 = new KILocation("Sylph Cave Trap Chest 4", "0x004A", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph5 = new KILocation("Sylph Cave Trap Chest 5", "0x004B", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph6 = new KILocation("Sylph Cave Trap Chest 6", "0x004C", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapSylph7 = new KILocation("Sylph Cave Trap Chest 7", "0x004D", KILocationType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, true);
			TrapGiant = new KILocation("Giant of Babil Trap Chest", "0x004E", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarPath = new KILocation("Lunar Path Trap Chest", "0x004F", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);

			TrapLunarCore1 = new KILocation("Lunar Subterrane Trap Chest 1", "0x0050", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore2 = new KILocation("Lunar Subterrane Trap Chest 2", "0x0051", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore3 = new KILocation("Lunar Subterrane Trap Chest 3", "0x0052", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore4 = new KILocation("Lunar Subterrane Trap Chest 4", "0x0053", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore5 = new KILocation("Lunar Subterrane Trap Chest 5", "0x0054", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore6 = new KILocation("Lunar Subterrane Trap Chest 6", "0x0055", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore7 = new KILocation("Lunar Subterrane Trap Chest 7", "0x0056", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore8 = new KILocation("Lunar Subterrane Trap Chest 8", "0x0057", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);
			TrapLunarCore9 = new KILocation("Lunar Subterrane Trap Chest 9", "0x0058", KILocationType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false);

			DMist = new KILocation("Mist Village", "0x0059", KILocationType.Main, new List<KeyItem>(), false);

			Golbez = new KILocation("Fallen Golbez", "0x005A", KILocationType.Golbez, new List<KeyItem>(), false);

			ObjectiveCompletion = new KILocation("Objective Completion", "0x005D", KILocationType.Objective, new List<KeyItem>(), true);

			if (main)
            {
				ListOfKILocations.Add(StartingItem);
				ListOfKILocations.Add(AntlionNest);
				ListOfKILocations.Add(DefendingFabul);
				ListOfKILocations.Add(MtOrdeals);
				ListOfKILocations.Add(BaronInn);
				ListOfKILocations.Add(BaronCastleKing);
				ListOfKILocations.Add(CaveMagnes);
				ListOfKILocations.Add(TowerOfZot);
				ListOfKILocations.Add(LowerBabilBoss);
				ListOfKILocations.Add(SuperCannon);
				ListOfKILocations.Add(DwarfCastle);
				ListOfKILocations.Add(SealedCave);
				ListOfKILocations.Add(FeymarchChest);
				ListOfKILocations.Add(AdamantCave);
				ListOfKILocations.Add(Sheila1);
				ListOfKILocations.Add(Sheila2);

				if (noFree)
				{
					ListOfKILocations.Add(DMist);
				}
				else
                {
					ListOfKILocations.Add(EdwardToroia);
				}
			}

			if(summon)
            {
				ListOfKILocations.Add(FeymarchQueen);
				ListOfKILocations.Add(FeymarchKing);
				ListOfKILocations.Add(OdinThrone);
				ListOfKILocations.Add(SylphTurnIn);
				ListOfKILocations.Add(CaveBahamut);
			}

			if(moon)
            {
				ListOfKILocations.Add(MurasameAltar);
				ListOfKILocations.Add(CrystalSwordAltar);
				ListOfKILocations.Add(WhiteSpearAltar);
				ListOfKILocations.Add(RibbonChest1);
				ListOfKILocations.Add(RibbonChest2);
				ListOfKILocations.Add(MasamuneAltar);
			}

			if(trap)
            {
				ListOfKILocations.Add(TrapZot);
				ListOfKILocations.Add(TrapEblan1);
				ListOfKILocations.Add(TrapEblan2);
				ListOfKILocations.Add(TrapEblan3);
				ListOfKILocations.Add(TrapLowerBabil1);
				ListOfKILocations.Add(TrapLowerBabil2);
				ListOfKILocations.Add(TrapLowerBabil3);
				ListOfKILocations.Add(TrapLowerBabil4);
				ListOfKILocations.Add(TrapCaveEblan);
				ListOfKILocations.Add(TrapUpperBabil);
				ListOfKILocations.Add(TrapFeymarch);
				ListOfKILocations.Add(TrapSylph1);
				ListOfKILocations.Add(TrapSylph2);
				ListOfKILocations.Add(TrapSylph3);
				ListOfKILocations.Add(TrapSylph4);
				ListOfKILocations.Add(TrapSylph5);
				ListOfKILocations.Add(TrapSylph6);
				ListOfKILocations.Add(TrapSylph7);
				ListOfKILocations.Add(TrapGiant);
				ListOfKILocations.Add(TrapLunarPath);

				if(unSafe)
                {
					ListOfKILocations.Add(TrapLunarCore1);
					ListOfKILocations.Add(TrapLunarCore2);
					ListOfKILocations.Add(TrapLunarCore3);
					ListOfKILocations.Add(TrapLunarCore4);
					ListOfKILocations.Add(TrapLunarCore5);
					ListOfKILocations.Add(TrapLunarCore6);
					ListOfKILocations.Add(TrapLunarCore7);
					ListOfKILocations.Add(TrapLunarCore8);
					ListOfKILocations.Add(TrapLunarCore9);
				}
			}

			if(objective)
            {
				ListOfKILocations.Add(ObjectiveCompletion);
            }


        }
	}
}
