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
				
		private readonly Label _locationsLabel;
		private readonly Label _objectivesLabel;
		private List<CheckBox> _objectivesCheckboxes;
		private readonly Label _oLocationsLabel;
		private readonly Label _uLocationsLabel;
		private readonly Label _mLocationsLabel;
		private readonly Label _oLocationsLabelChecked;
		private readonly Label _uLocationsLabelChecked;
		private readonly Label _mLocationsLabelChecked;
		private int KICount;

		private readonly Label _debug;

		private ApiContainer APIs => _maybeAPIContainer!;

		protected override string WindowTitleStatic => "FE Sight";

		public FESightForm()
		{
			ClientSize = new Size(1000, 450);
			this.BackColor = System.Drawing.Color.FromArgb(0, 0, 99);
			SuspendLayout();

			Controls.Add(_objectivesLabel = new Label());
			_objectivesLabel.AutoSize = true;
			_objectivesLabel.Location = new Point(Constants.OBJECTIVES_START_COORDS_X, Constants.OBJECTIVES_START_COORDS_Y);
			_objectivesLabel.ForeColor = Color.White;
			_objectivesLabel.Text = "Objectives:";
			_objectivesCheckboxes = new List<CheckBox>();

			Controls.Add(_oLocationsLabel = new Label());
			_oLocationsLabel.AutoSize = true;
			_oLocationsLabel.Location = new Point(200, 0);
			_oLocationsLabel.ForeColor = Color.White;
			_oLocationsLabel.Text = "oLocations";

			Controls.Add(_uLocationsLabel = new Label());
			_uLocationsLabel.AutoSize = true;
			_uLocationsLabel.Location = new Point(200, 20);
			_uLocationsLabel.ForeColor = Color.White;
			_uLocationsLabel.Text = "uLocations";

			Controls.Add(_mLocationsLabel = new Label());
			_mLocationsLabel.AutoSize = true;
			_mLocationsLabel.Location = new Point(200, 30);
			_mLocationsLabel.ForeColor = Color.White;
			_mLocationsLabel.Text = "mLocations";

			Controls.Add(_oLocationsLabelChecked = new Label());
			_oLocationsLabelChecked.AutoSize = true;
			_oLocationsLabelChecked.Location = new Point(200, 40);
			_oLocationsLabelChecked.ForeColor = Color.White;
			_oLocationsLabelChecked.Text = "oLocationsChecked";

			Controls.Add(_uLocationsLabelChecked = new Label());
			_uLocationsLabelChecked.AutoSize = true;
			_uLocationsLabelChecked.Location = new Point(200, 50);
			_uLocationsLabelChecked.ForeColor = Color.White;
			_uLocationsLabelChecked.Text = "uLocationsChecked";

			Controls.Add(_mLocationsLabelChecked = new Label());
			_mLocationsLabelChecked.AutoSize = true;
			_mLocationsLabelChecked.Location = new Point(200, 60);
			_mLocationsLabelChecked.ForeColor = Color.White;
			_mLocationsLabelChecked.Text = "mLocationsChecked";

			Controls.Add(_debug = new Label());
			_debug.Location = new Point(400, 0);
			_debug.AutoSize = true;
			_debug.ForeColor = Color.White;
			

			ResumeLayout();
		}

		private void SetObjectives(Metadata metadata)
		{
			if (_objectivesCheckboxes.Count > 0)
				return;

			int currentY = Constants.OBJECTIVES_START_COORDS_Y;
			int labelX = Constants.OBJECTIVES_START_COORDS_X + Constants.OBJECTIVES_LABEL_PADDING;

			foreach (var objective in metadata.objectives)
            {
				currentY += 20;
				CheckBox objectiveCheckbox = new CheckBox();
				objectiveCheckbox.Location = new Point(Constants.OBJECTIVES_START_COORDS_X, currentY);
				objectiveCheckbox.ForeColor = Color.White;
				objectiveCheckbox.AutoSize = true;
				objectiveCheckbox.Text = objective;

				_objectivesCheckboxes.Add(objectiveCheckbox);
				Controls.Add(objectiveCheckbox);
            }
        }

		private void UpdateKeyItems()
		{
			if (KeyItems.KeyItemsInitialized == false)
			{
				int ICON_SPACE = 40;

				int count = 0;
				int x = 0;
				int y = 0;

				KeyItems.InitializeKeyItems();

				foreach(var keyItem in KeyItems.KeyItemList)
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

			APIs.Memory.UseMemoryDomain(Constants.WRAM_STRING);

			var bytes = APIs.Memory.ReadByteRange(0x1500, 3);
			BitArray kiFlags = new BitArray(bytes.ToArray());
			foreach(var keyItem in KeyItems.KeyItemList)
            {
				keyItem.Obtained = kiFlags[keyItem.MemoryIndex];
            }


            bytes = APIs.Memory.ReadByteRange(0x1503, 3);
            BitArray usageFlags = new BitArray(bytes.ToArray());
            foreach (var keyItem in KeyItems.KeyItemList)
            {
                keyItem.Used = usageFlags[keyItem.MemoryIndex];
            }

            foreach (var keyItem in KeyItems.KeyItemList)
            {
				keyItem.PictureBox.ImageLocation = keyItem.IconLocation;
            }

		}

		private void UpdateLocations()
        {
			if (KILocations.KILocationsInitialized == false)
            {
				KILocations.InitializeKILocations(false, Flags.Kmain, Flags.Ksummon, Flags.Kmoon, Flags.Ktrap, Flags.Kunsafe || Flags.Kunsafer, Flags.Knofree);
			}				

			var bytes = APIs.Memory.ReadByteRange(0x1510, 15);
			BitArray locFlags = new BitArray(bytes.ToArray());
			foreach(var location in KILocations.ListOfKILocations)
            {
				bool hasBeenChecked = location.HasBeenChecked(locFlags);
            }
		}

		public override void Restart()
		{
			Metadata metadata = GetMetadata();
			Flags.SetFlags(metadata);
			SetObjectives(metadata);
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
			UpdateLocations();
			UpdateKeyItems();
        }

        private string Debug()
        {
			string output = "Debug: \n";

			//Debug stuff!
			output += "Flags:";
			output += "\nKmain: " + Flags.Kmain;
			output += "\nKsummon: " + Flags.Ksummon;
			output += "\nKmoon: " + Flags.Kmoon;
			output += "\nKtrap: " + Flags.Ktrap;
			output += "\nKunsafe: " + Flags.Kunsafe;
			output += "\nKunsafer: " + Flags.Kunsafer;
			output += "\n-pushbtojump: " + Flags.OtherPushBToJump;
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
			bool worked = APIs.Memory.UseMemoryDomain(Constants.CARTROM_STRING);
			string jsonString = "";

			if (worked)
			{
                try
                {
					var bytes = APIs.Memory.ReadByteRange(Constants.FLAGS_LENGTH_ADDRESS, Constants.FLAGS_LENGTH_LENGTH);
					bytes.Reverse();

					int jsonLength = GetIntFromBytes(bytes.ToArray());
					var jsonBytes = APIs.Memory.ReadByteRange(Constants.FLAGS_START_ADDRESS, jsonLength);
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