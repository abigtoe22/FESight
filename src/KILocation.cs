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
		public KILocationFlagType LocationFlagType { get; set; }
		public KILocationArea LocationArea { get; set; }
		public List<KeyItem> GatingKeyItems { get; set; }
		public bool PushBToJumpSkip { get; set; }

		public KILocation(string name, string hexIndex, KILocationFlagType type, List<KeyItem> gates, bool jumpSkip, KILocationArea kILocationArea)
		{
			Name = name;
			Checked = false;
			Index = Convert.ToInt32(hexIndex, 16);
			MemoryLocation = new BitArray(120);
			MemoryLocation[Index] = true;
			GatingKeyItems = gates;
			LocationFlagType = type;
			PushBToJumpSkip = jumpSkip;
			LocationArea = kILocationArea;
		}

		public bool HasBeenChecked(BitArray memoryArray)
		{
			if (memoryArray[Index])
				Checked = true;

			return Checked;
		}

		public bool IsAvailable(bool hookCleared)
		{
            try
            {
				bool kIsRequiredObtained = true;

				if (Name == "Troia Castle" && Flags.Knofree)
					return false;

				if (Name == "Mist Village")
				{
					if (Flags.Knofree == false)
						return false;

					if (KILocations.DMistChecked)
						return true;

					return false;
				}

				if (Name.Contains("Lunar Subterrane Trap"))
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
			catch (Exception)
            {

                throw new Exception(this.Name);
            }
		}


	}

	public enum KILocationFlagType
	{
		Main,
		Summon,
		Moon,
		Trap,
		Objective,
		Golbez
	}

	public enum KILocationArea
    {
		Overworld,
		Underground,
		Moon
    }

	public static class KILocations
	{
		public static bool KILocationsInitialized { get; set; }
		public static bool DMistChecked { get; set; }
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


		public static void InitializeKILocations(bool objectiveFlag, bool mainFlag, bool summonFlag, bool moonFlag, bool trapFlag, bool unSafeFlag, bool noFreeFlag)
        {
			if (KILocationsInitialized)
				return;

			ListOfKILocations = new List<KILocation>();
			
			StartingItem = new KILocation("Starting Item", "0x0020", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			AntlionNest = new KILocation("Antlion Nest", "0x0021", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			DefendingFabul = new KILocation("Fabul Defense", "0x0022", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			MtOrdeals = new KILocation("Mt. Ordeals", "0x0023", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			BaronInn = new KILocation("Baron Inn", "0x0024", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			BaronCastleKing = new KILocation("Baron Castle King", "0x0025", KILocationFlagType.Main, new List<KeyItem> { KeyItems.BaronKey }, true, KILocationArea.Overworld);
			EdwardToroia = new KILocation("Troia Castle", "0x0026", KILocationFlagType.Main, new List<KeyItem>(), true, KILocationArea.Overworld);
			CaveMagnes = new KILocation("Cave Magnes", "0x0027", KILocationFlagType.Main, new List<KeyItem> { KeyItems.TwinHarp }, true, KILocationArea.Overworld);
			TowerOfZot = new KILocation("Tower of Zot", "0x0028", KILocationFlagType.Main, new List<KeyItem> { KeyItems.EarthCrystal }, false, KILocationArea.Overworld);
			LowerBabilBoss = new KILocation("Lower Babil Boss", "0x0029", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			SuperCannon = new KILocation("Lower Babil Super Cannon", "0x002A", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.TowerKey }, false, KILocationArea.Underground);
			
			if(Flags.Gwarp == false)
            {
				DwarfCastle = new KILocation("Dwarf Castle", "0x002B", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			}
			else
            {
				DwarfCastle = new KILocation("Dwarf/Warp", "0x002B", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			}
			SealedCave = new KILocation("Sealed Cave", "0x002C", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.LucaKey }, false, KILocationArea.Underground);
			FeymarchChest = new KILocation("Feymarch Chest", "0x002D", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			AdamantCave = new KILocation("Adamant Cave", "0x002E", KILocationFlagType.Main, new List<KeyItem> { KeyItems.Hook, KeyItems.RatTail }, false, KILocationArea.Overworld);
			Sheila1 = new KILocation("Fabul [Sheila 1/Sylph]", "0x002F", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			Sheila2 = new KILocation("Fabul [Sheila 2/Pan]", "0x0030", KILocationFlagType.Main, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.Pan }, false, KILocationArea.Underground);

			FeymarchQueen = new KILocation("Asura Spot", "0x0031", KILocationFlagType.Summon, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			FeymarchKing = new KILocation("Leviathan Spot", "0x0032", KILocationFlagType.Summon, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			OdinThrone = new KILocation("Baron Castle Odin", "0x0033", KILocationFlagType.Summon, new List<KeyItem> { KeyItems.BaronKey }, true, KILocationArea.Overworld);
			SylphTurnIn = new KILocation("Sylph Cave", "0x0034", KILocationFlagType.Summon, new List<KeyItem> { KeyItems.MagmaKey, KeyItems.Pan }, false, KILocationArea.Underground);
			CaveBahamut = new KILocation("Cave Bahamut", "0x0035", KILocationFlagType.Summon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);

			MurasameAltar = new KILocation("Murasame Altar", "0x0036", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			CrystalSwordAltar = new KILocation("Crystal Sword Altar", "0x0037", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			WhiteSpearAltar = new KILocation("White Spear Altar", "0x0038", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			RibbonChest1 = new KILocation("Ribbon Room Chest 1", "0x0039", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			RibbonChest2 = new KILocation("Ribbon Room Chest 2", "0x003A", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			MasamuneAltar = new KILocation("Masamune Altar", "0x003B", KILocationFlagType.Moon, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);

			TrapZot = new KILocation("Zot Trap Chest", "0x003C", KILocationFlagType.Trap, new List<KeyItem>(), true, KILocationArea.Overworld);
			TrapEblan1 = new KILocation("Eblan Trap Chest 1", "0x003D", KILocationFlagType.Trap, new List<KeyItem>(), true, KILocationArea.Overworld);
			TrapEblan2 = new KILocation("Eblan Trap Chest 2", "0x003E", KILocationFlagType.Trap, new List<KeyItem>(), true, KILocationArea.Overworld);
			TrapEblan3 = new KILocation("Eblan Trap Chest 3", "0x003F", KILocationFlagType.Trap, new List<KeyItem>(), true, KILocationArea.Overworld);
			TrapLowerBabil1 = new KILocation("Lower Babil Trap Chest 1", "0x0040", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapLowerBabil2 = new KILocation("Lower Babil Trap Chest 2", "0x0041", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapLowerBabil3 = new KILocation("Lower Babil Trap Chest 3", "0x0042", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapLowerBabil4 = new KILocation("Lower Babil Trap Chest 4", "0x0043", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapCaveEblan = new KILocation("Cave Eblan Trap Chest", "0x0044", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.Hook }, true, KILocationArea.Overworld);
			TrapUpperBabil = new KILocation("Upper Babil Trap Chest", "0x0045", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.Hook }, true, KILocationArea.Overworld);
			TrapFeymarch = new KILocation("Feymarch Trap Chest", "0x0046", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph1 = new KILocation("Sylph Cave Trap Chest 1", "0x0047", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph2 = new KILocation("Sylph Cave Trap Chest 2", "0x0048", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph3 = new KILocation("Sylph Cave Trap Chest 3", "0x0049", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph4 = new KILocation("Sylph Cave Trap Chest 4", "0x004A", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph5 = new KILocation("Sylph Cave Trap Chest 5", "0x004B", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph6 = new KILocation("Sylph Cave Trap Chest 6", "0x004C", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapSylph7 = new KILocation("Sylph Cave Trap Chest 7", "0x004D", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.MagmaKey }, false, KILocationArea.Underground);
			TrapGiant = new KILocation("Giant of Babil Trap Chest", "0x004E", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Overworld);
			TrapLunarPath = new KILocation("Lunar Path Trap Chest", "0x004F", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);

			TrapLunarCore1 = new KILocation("Lunar Subterrane Trap Chest 1", "0x0050", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore2 = new KILocation("Lunar Subterrane Trap Chest 2", "0x0051", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore3 = new KILocation("Lunar Subterrane Trap Chest 3", "0x0052", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore4 = new KILocation("Lunar Subterrane Trap Chest 4", "0x0053", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore5 = new KILocation("Lunar Subterrane Trap Chest 5", "0x0054", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore6 = new KILocation("Lunar Subterrane Trap Chest 6", "0x0055", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore7 = new KILocation("Lunar Subterrane Trap Chest 7", "0x0056", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore8 = new KILocation("Lunar Subterrane Trap Chest 8", "0x0057", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);
			TrapLunarCore9 = new KILocation("Lunar Subterrane Trap Chest 9", "0x0058", KILocationFlagType.Trap, new List<KeyItem> { KeyItems.DarknessCrystal }, false, KILocationArea.Moon);

			DMist = new KILocation("Mist Village", "0x0059", KILocationFlagType.Main, new List<KeyItem>(), false, KILocationArea.Overworld);

			// Techically a location. Maybe someday?
			Golbez = new KILocation("Fallen Golbez", "0x005A", KILocationFlagType.Golbez, new List<KeyItem>(), false, KILocationArea.Moon);

			// Technically a location but we're not going to display it
			ObjectiveCompletion = new KILocation("Objective Completion", "0x005D", KILocationFlagType.Objective, new List<KeyItem>(), true, KILocationArea.Overworld);

			if (mainFlag)
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

				if (noFreeFlag)
				{
					ListOfKILocations.Add(DMist);
				}
				else
                {
					ListOfKILocations.Add(EdwardToroia);
				}
			}

			if(summonFlag)
            {
				ListOfKILocations.Add(FeymarchQueen);
				ListOfKILocations.Add(FeymarchKing);
				ListOfKILocations.Add(OdinThrone);
				ListOfKILocations.Add(SylphTurnIn);
				ListOfKILocations.Add(CaveBahamut);
			}

			if(moonFlag)
            {
				ListOfKILocations.Add(MurasameAltar);
				ListOfKILocations.Add(CrystalSwordAltar);
				ListOfKILocations.Add(WhiteSpearAltar);
				ListOfKILocations.Add(RibbonChest1);
				ListOfKILocations.Add(RibbonChest2);
				ListOfKILocations.Add(MasamuneAltar);
			}

			if(objectiveFlag)
            {
				ListOfKILocations.Add(ObjectiveCompletion);
            }

			KILocationsInitialized = true;
        }
	}
}
