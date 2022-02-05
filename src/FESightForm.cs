using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using Newtonsoft.Json;

namespace FESight
{
	[ExternalTool("FESight")]
	public sealed class FESightForm : ToolFormBase, IExternalToolForm
	{
		public ApiContainer? _maybeAPIContainer { get; set; }

		private readonly Label _flagsLabel;
		private readonly Label _locationsLabel;
		private readonly Label _debug;

		private ApiContainer APIs => _maybeAPIContainer!;

		protected override string WindowTitleStatic => "FE Sight";

		public List<KeyItem> KeyItems;
		private List<KILocation> KILocations;

		public FESightForm()
		{
			ClientSize = new Size(480, 320);
			this.BackColor = System.Drawing.Color.FromArgb(0, 0, 99);
			SuspendLayout();
			Controls.Add(_flagsLabel = new Label { AutoSize = true });
			Controls.Add(_locationsLabel = new Label());
			_locationsLabel.AutoSize = true;
			_locationsLabel.Location = new Point(200, 0);
			_locationsLabel.ForeColor = Color.White;

			Controls.Add(_debug = new Label());
			_debug.Location = new Point(400, 0);
			_debug.AutoSize = true;
			_debug.ForeColor = Color.White;
			

			ResumeLayout();
		}

		private string ReadKeyItems()
		{
			if (KeyItems == null)
			{
				KeyItems = LoadKeyItemList();
			}

			var bytes = APIs.Memory.ReadByteRange(0x1500, 3);
			BitArray kiFlags = new BitArray(bytes.ToArray());
			UpdateKeyItems(kiFlags);

			bytes = APIs.Memory.ReadByteRange(0x1503, 3);
			BitArray usageFlags = new BitArray(bytes.ToArray());
			UpdateKeyItemUsage(usageFlags);

			string result = "";

			foreach (var ki in KeyItems)
			{
				result += ki.Name + ": " + ki.Obtained + " - " + ki.Used + "\r\n";
			}

			return result;
		}

		private void UpdateKeyItems()
		{
			if (KeyItems == null)
			{
				int ICON_SPACE = 40;

				int count = 0;
				int x = 0;
				int y = 0;

				KeyItems = LoadKeyItemList();
				foreach(var keyItem in KeyItems)
                {					
					if (count % 4 == 0 && count != 0)
                    {
						y = y + ICON_SPACE;
						x = 0;
					}
						

					keyItem.PictureBox = new PictureBox();
					keyItem.PictureBox.ImageLocation = keyItem.IconLocation;
					keyItem.PictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
					keyItem.PictureBox.Location = new Point(x, y);
					Controls.Add(keyItem.PictureBox);

					x = x + ICON_SPACE;
					count++;
				}
			}

			var bytes = APIs.Memory.ReadByteRange(0x1500, 3);
			BitArray kiFlags = new BitArray(bytes.ToArray());
			foreach(var keyItem in KeyItems)
            {
				keyItem.Obtained = kiFlags[keyItem.MemoryIndex];
            }

			bytes = APIs.Memory.ReadByteRange(0x1503, 3);
			BitArray usageFlags = new BitArray(bytes.ToArray());
			foreach (var keyItem in KeyItems)
			{
				keyItem.Used = usageFlags[keyItem.MemoryIndex];
			}

			foreach(var keyItem in KeyItems)
            {
				keyItem.PictureBox.ImageLocation = keyItem.IconLocation;
            }

		}

		private string ReadLocations()
        {
			if (KILocations == null)
            {
				KILocations = LoadKILocations();
			}				

			string result = "";

			var bytes = APIs.Memory.ReadByteRange(0x1510, 15);
			BitArray locFlags = new BitArray(bytes.ToArray());
			foreach(var location in KILocations)
            {
				bool hasBeenChecked = location.HasBeenChecked(locFlags);
				if(!hasBeenChecked)
					result += location.Name + "\r\n";
            }

			return result;
		}

		private List<KeyItem> LoadKeyItemList()
        {
			List<KeyItem> kis = new List<KeyItem>();
			KeyItem crystal = new KeyItem("Crystal", "1THECrystal", 16);
			kis.Add(crystal);
			KeyItem pass = new KeyItem("Pass", "2Pass", 17);
			kis.Add(pass);
			KeyItem hook = new KeyItem("Hook", "3Hook", 8);
			kis.Add(hook);
			KeyItem darknessCrystal = new KeyItem("Darkness Crystal", "4DarkCrystal", 10);
			kis.Add(darknessCrystal);
			KeyItem earthCrystal = new KeyItem("Earth Crystal", "5EarthCrystal", 5);
			kis.Add(earthCrystal);
			KeyItem twinHarp = new KeyItem("Twin Harp", "6TwinHarp", 4);
			kis.Add(twinHarp);
			KeyItem package = new KeyItem("Package", "7Package", 0);
			kis.Add(package);
			KeyItem sandruby = new KeyItem("SandRuby", "8SandRuby", 1);
			kis.Add(sandruby);
			KeyItem baronKey = new KeyItem("Baron Key", "9BaronKey", 3);
			kis.Add(baronKey);
			KeyItem magmaKey = new KeyItem("Magma Key", "10MagmaKey", 6);
			kis.Add(magmaKey);
			KeyItem towerKey = new KeyItem("Tower Key", "11Towerkey", 7);
			kis.Add(towerKey);
			KeyItem lucaKey = new KeyItem("Luca Key", "12LucaKey", 9);
			kis.Add(lucaKey);
			KeyItem adamant = new KeyItem("Adamant", "13Adamant", 12);
			kis.Add(adamant);
			KeyItem legendSword = new KeyItem("Legend Sword", "14LegendSword", 2);
			kis.Add(legendSword);
			KeyItem pan = new KeyItem("Pan", "15Pan", 13);
			kis.Add(pan);
			KeyItem spoon = new KeyItem("Spoon", "16Spoon", 14);
			kis.Add(spoon);
			KeyItem ratTail = new KeyItem("Rat Tail", "17RatTail", 11);
			kis.Add(ratTail);
			KeyItem pinkTail = new KeyItem("Pink Tail", "18PinkTail", 15);
			kis.Add(pinkTail);

			return kis;
		}

		private List<KILocation> LoadKILocations(bool main = true, bool summon = false, bool moon = false, bool trap = false, bool objective = false, bool noFreeKIs = false)
		{
			bool golbez = false;
			summon = true;
			moon = true;
			noFreeKIs = true;

			List<KILocation> locations = new List<KILocation>();

			if(objective)
            {
				KILocation objectiveCompletion = new KILocation("Objective completion", "0x005D");
				locations.Add(objectiveCompletion);
            }

			if(noFreeKIs)
            {
				KILocation dmist = new KILocation("D. Mist", "0x0059");
				locations.Add(dmist);
			}
			else
            {
				KILocation edwardToroia = new KILocation("Edward in Toroia", "0x0026");
				locations.Add(edwardToroia);
			}

			if (main)
			{
				KILocation startingItem = new KILocation("Starting item", "0x0020");
				locations.Add(startingItem);
				KILocation antlionNest = new KILocation("Antlion nest", "0x0021");
				locations.Add(antlionNest);
				KILocation defendingFabul = new KILocation("Defending Fabul", "0x0022");
				locations.Add(defendingFabul);
				KILocation mtOrdeals = new KILocation("Mt. Ordeals", "0x0023");
				locations.Add(mtOrdeals);
				KILocation baronInn = new KILocation("Baron Inn", "0x0024");
				locations.Add(baronInn);
				KILocation baronCastle = new KILocation("Baron Castle", "0x0025");
				locations.Add(baronCastle);
				KILocation caveMagnes = new KILocation("Cave Magnes", "0x0027");
				locations.Add(caveMagnes);
				KILocation towerOfZot = new KILocation("Tower of Zot", "0x0028");
				locations.Add(towerOfZot);
				KILocation lowerBabil = new KILocation("Lower Bab-il boss", "0x0029");
				locations.Add(lowerBabil);
				KILocation superCannon = new KILocation("Super Cannon", "0x002A");
				locations.Add(superCannon);
				KILocation luca = new KILocation("Luca", "0x002B");
				locations.Add(luca);
				KILocation sealedCave = new KILocation("Sealed Cave", "0x002C");
				locations.Add(sealedCave);
				KILocation feymarchChest = new KILocation("Feymarch Chest", "0x002D");
				locations.Add(feymarchChest);
				KILocation ratTail = new KILocation("Rat Tail trade", "0x002E");
				locations.Add(ratTail);
				KILocation sheila1 = new KILocation("Sheila 1", "0x002F");
				locations.Add(sheila1);
				KILocation sheila2 = new KILocation("Sheila 2", "0x0030");
				locations.Add(sheila2);
			}

			if (summon)
            {
				KILocation feymarchQueen = new KILocation("Feymarch queen", "0x0031");
				locations.Add(feymarchQueen);
				KILocation feymarchKing = new KILocation("Feymarch King", "0x0032");
				locations.Add(feymarchKing);
				KILocation odinThrone = new KILocation("Odin throne", "0x0033");
				locations.Add(odinThrone);
				KILocation sylph = new KILocation("From the sylphs", "0x0034");
				locations.Add(sylph);
				KILocation bahamut = new KILocation("Cave Bahamut", "0x0035");
				locations.Add(bahamut);
			}

			if(moon)
            {
				KILocation murasameAltar = new KILocation("Murasame altar", "0x0036");
				locations.Add(murasameAltar);
				KILocation crystalSword = new KILocation("Crystal Sword altar", "0x0037");
				locations.Add(crystalSword);
				KILocation whiteSpear = new KILocation("White Spear altar", "0x0038");
				locations.Add(whiteSpear);
				KILocation ribbon1 = new KILocation("Ribbon chest 1", "0x0039");
				locations.Add(ribbon1);
				KILocation ribbon2 = new KILocation("Ribbon chest 2", "0x003A");
				locations.Add(ribbon2);
				KILocation masamuneAltar = new KILocation("Masamune altar", "0x003B");
				locations.Add(masamuneAltar);
			}

			if(trap)
            {
				KILocation zotTrap = new KILocation("Zot Chest", "0x003C");
				locations.Add(zotTrap);
				KILocation eblan1 = new KILocation("Eblan Chest 1", "0x003D");
				locations.Add(eblan1);
				KILocation eblan2 = new KILocation("Eblan Chest 2", "0x003E");
				locations.Add(eblan2);
				KILocation eblan3 = new KILocation("Eblan Chest 3", "0x003F");
				locations.Add(eblan3);
				KILocation lowerBabilChest1 = new KILocation("Lower Bab-il Chest 1", "0x0040");
				locations.Add(lowerBabilChest1);
				KILocation lowerBabilChest2 = new KILocation("Lower Bab-il Chest 2", "0x0041");
				locations.Add(lowerBabilChest2);
				KILocation lowerBabilChest3 = new KILocation("Lower Bab-il Chest 3", "0x0042");
				locations.Add(lowerBabilChest3);
				KILocation lowerBabilChest4 = new KILocation("Lower Bab-il Chest 4", "0x0043");
				locations.Add(lowerBabilChest4);
				KILocation caveEblanChest = new KILocation("Cave Eblan Chest", "0x0044");
				locations.Add(caveEblanChest);
				KILocation upperBabilChest = new KILocation("Upper Bab-il Chest", "0x0045");
				locations.Add(upperBabilChest);
				KILocation caveOfSummonsChest = new KILocation("Cave of Summons Chest", "0x0046");
				locations.Add(caveOfSummonsChest);
				KILocation sylph1 = new KILocation("Sylph Chest 1", "0x0047");
				locations.Add(sylph1);
				KILocation sylph2 = new KILocation("Sylph Chest 2", "0x0048");
				locations.Add(sylph2);
				KILocation sylph3 = new KILocation("Sylph Chest 3", "0x0049");
				locations.Add(sylph3);
				KILocation sylph4 = new KILocation("Sylph Chest 4", "0x004A");
				locations.Add(sylph4);
				KILocation sylph5 = new KILocation("Sylph Chest 5", "0x004B");
				locations.Add(sylph5);
				KILocation sylph6 = new KILocation("Sylph Chest 6", "0x004C");
				locations.Add(sylph6);
				KILocation sylph7 = new KILocation("Sylph Chest 7", "0x004D");
				locations.Add(sylph7);
				KILocation giantOfBabilChest = new KILocation("Giant of Babil Chest", "0x004E");
				locations.Add(giantOfBabilChest);
				KILocation lunarPathChest = new KILocation("Lunar Path Chest", "0x004F");
				locations.Add(lunarPathChest);
				KILocation lunar1 = new KILocation("Lunar Core Chest 1", "0x0050");
				locations.Add(lunar1);
				KILocation lunar2 = new KILocation("Lunar Core Chest 2", "0x0051");
				locations.Add(lunar2);
				KILocation lunar3 = new KILocation("Lunar Core Chest 3", "0x0052");
				locations.Add(lunar3);
				KILocation lunar4 = new KILocation("Lunar Core Chest 4", "0x0053");
				locations.Add(lunar4);
				KILocation lunar5 = new KILocation("Lunar Core Chest 5", "0x0054");
				locations.Add(lunar5);
				KILocation lunar6 = new KILocation("Lunar Core Chest 6", "0x0055");
				locations.Add(lunar6);
				KILocation lunar7 = new KILocation("Lunar Core Chest 7", "0x0056");
				locations.Add(lunar7);
				KILocation lunar8 = new KILocation("Lunar Core Chest 8", "0x0057");
				locations.Add(lunar8);
				KILocation lunar9 = new KILocation("Lunar Core Chest 9", "0x0058");
				locations.Add(lunar9);
			}

			if(golbez)
            {
				KILocation fallenGolbez = new KILocation("Fallen Golbez", "0x005A");
				locations.Add(fallenGolbez);
            }

			return locations;
		}



		private void UpdateKeyItems(BitArray flags)
		{
			for (int i = 0; i < KeyItems.Count; i++)
			{
				KeyItems[i].Obtained = flags[i];
			}
		}

		private void UpdateKeyItemUsage(BitArray flags)
		{
			for (int i = 0; i < KeyItems.Count; i++)
			{
				KeyItems[i].Used = flags[i];
			}
		}

		public override void Restart()
		{
			DisplayOutput();
		}

		protected override void UpdateAfter()
		{
			if (APIs.Emulation.FrameCount() % 60 == 0)
				DisplayOutput();
		}

		private void DisplayOutput()
        {
			//_flagsLabel.Text = ReadKeyItems();
			_debug.Text = Debug();
			_locationsLabel.Text = ReadLocations();
			UpdateKeyItems();
        }

        private string Debug()
        {
			string output = "Debug: \n";

			//Debug stuff!
			var metadata = GetMetadata();

			output += "\nVersion: " + metadata.version;
			output += "\nFlags: " + metadata.flags;			
			output += "\nBinary Flags: " + metadata.binary_flags;
			output += "\nSeed: " + metadata.seed;
			if(metadata.objectives != null)
			{
				foreach(var objective in metadata.objectives)
                {
					output += "\nObjective: " + objective;
                }
            }
			//End debug stuff.

			return output;
		}

        public static string PrintValues(IEnumerable myList, int myWidth)
		{
			string result = "";
			int i = myWidth;
			foreach (Object obj in myList)
			{
				if (i <= 0)
				{
					i = myWidth;
					result += "\r\n";
				}
				i--;
				result += string.Format("{0,8}", obj);
			}
			result += "\r\n";

			return result;
		}

		private Metadata GetMetadata()
        {
			bool worked = APIs.Memory.UseMemoryDomain("CARTROM");
			string jsonString = "";

			if (worked)
			{
                try
                {
					var bytes = APIs.Memory.ReadByteRange(Constants.FLAGS_LENGTH_LOCATION, Constants.FLAGS_LENGTH_LENGTH);
					bytes.Reverse();

					int jsonLength = GetIntFromBytes(bytes.ToArray());
					var jsonBytes = APIs.Memory.ReadByteRange(Constants.FLAGS_START_LOCATION, jsonLength);
					jsonString = System.Text.Encoding.ASCII.GetString(jsonBytes.ToArray());
					Metadata metadata = JsonConvert.DeserializeObject<Metadata>(jsonString);

					return metadata;

				}
				catch (Exception ex)
                {

                    throw new Exception("Error reading metadata. JSON String: " + jsonString + " -- Inner Exception: " + ex.Message);
                }
			}
			else
			{
				throw new Exception("Can't read CARTROM memory domain.");
			}
		}

		//Assuming little endian. Anything this runs on is going to be little endian.
		private int GetIntFromBytes(byte[] data)
		{
			if (data.Length > 4)
				throw new Exception("Can't get int from a byte array with more than 4 bytes.");

			if (data.Length < 4)
			{
				int empties = 4 - data.Length;
				for (int i = 0; i < empties; i++)
				{
					data = data.Prepend(new byte()).ToArray();
				}
			}

			Array.Reverse(data);
			int result = BitConverter.ToInt32(data, 0);
			return result;
		}
	}


}