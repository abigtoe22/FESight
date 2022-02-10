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
				
		private readonly Label _objectivesLabel;
		private List<CheckBox> _objectivesCheckboxes;
		private readonly Label _locationsLabel;
		private readonly Label _oLocationsLabel;
		private readonly Label _uLocationsLabel;
		private readonly Label _mLocationsLabel;
		private readonly Label _oLocationsLabelChecked;
		private readonly Label _uLocationsLabelChecked;
		private readonly Label _mLocationsLabelChecked;
		private readonly Label _trapsLabel;
		private readonly Label _oTrapsLabel;
		private readonly Label _uTrapsLabel;
		private readonly Label _mTrapsLabel;
		private readonly Label _oTrapsLabelChecked;
		private readonly Label _uTrapsLabelChecked;
		private readonly Label _mTrapsLabelChecked;
		private readonly Label _hookClearedLabel;

		private string romHash;
		private string romName;
		private int KICount;
		private bool _hookCleared;

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
			_objectivesLabel.ForeColor = Constants.FORM_FONT_COLOR;
			_objectivesLabel.Font = Constants.FORM_FONT;
			_objectivesLabel.Text = "Objectives:";
			_objectivesCheckboxes = new List<CheckBox>();

			Controls.Add(_locationsLabel = new Label());
			_locationsLabel.AutoSize = true;
			_locationsLabel.Location = new Point(Constants.LOCATIONS_START_COORD_X, Constants.LOCATIONS_START_COORD_Y);
			_locationsLabel.ForeColor = Constants.FORM_FONT_COLOR;
			_locationsLabel.Font = Constants.FORM_FONT;
			_locationsLabel.Text = "Locations";

			Controls.Add(_oLocationsLabel = new Label());
			_oLocationsLabel.AutoSize = true;
			_oLocationsLabel.ForeColor = Constants.OVERWORLD_LOCATIONS_COLOR;
			_oLocationsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_uLocationsLabel = new Label());
			_uLocationsLabel.AutoSize = true;
			_uLocationsLabel.ForeColor = Constants.UNDERGROUND_LOCATIONS_COLOR;
			_uLocationsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_mLocationsLabel = new Label());
			_mLocationsLabel.AutoSize = true;
			_mLocationsLabel.ForeColor = Constants.MOON_LOCATIONS_COLOR;
			_mLocationsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_oLocationsLabelChecked = new Label());
			_oLocationsLabelChecked.AutoSize = true;
			_oLocationsLabelChecked.ForeColor = Constants.OVERWORLD_LOCATIONS_COLOR;

			Controls.Add(_uLocationsLabelChecked = new Label());
			_uLocationsLabelChecked.AutoSize = true;
			_uLocationsLabelChecked.ForeColor = Constants.UNDERGROUND_LOCATIONS_COLOR;

			Controls.Add(_mLocationsLabelChecked = new Label());
			_mLocationsLabelChecked.AutoSize = true;
			_mLocationsLabelChecked.ForeColor = Constants.MOON_LOCATIONS_COLOR;

			Controls.Add(_trapsLabel = new Label());
			_trapsLabel.AutoSize = true;
			_trapsLabel.ForeColor = Constants.FORM_FONT_COLOR;
			_trapsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_oTrapsLabel = new Label());
			_oTrapsLabel.AutoSize = true;
			_oTrapsLabel.ForeColor = Constants.OVERWORLD_LOCATIONS_COLOR;
			_oTrapsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_uTrapsLabel = new Label());
			_uTrapsLabel.AutoSize = true;
			_uTrapsLabel.ForeColor = Constants.UNDERGROUND_LOCATIONS_COLOR;
			_uTrapsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_mTrapsLabel = new Label());
			_mTrapsLabel.AutoSize = true;
			_mTrapsLabel.ForeColor = Constants.MOON_LOCATIONS_COLOR;
			_mTrapsLabel.Font = Constants.FORM_FONT;

			Controls.Add(_oTrapsLabelChecked = new Label());
			_oTrapsLabelChecked.AutoSize = true;
			_oTrapsLabelChecked.ForeColor = Constants.OVERWORLD_LOCATIONS_COLOR;

			Controls.Add(_uTrapsLabelChecked = new Label());
			_uTrapsLabelChecked.AutoSize = true;
			_uTrapsLabelChecked.ForeColor = Constants.UNDERGROUND_LOCATIONS_COLOR;

			Controls.Add(_mTrapsLabelChecked = new Label());
			_mTrapsLabelChecked.AutoSize = true;
			_mTrapsLabelChecked.ForeColor = Constants.MOON_LOCATIONS_COLOR;

			Controls.Add(_hookClearedLabel = new Label());
			_hookClearedLabel.AutoSize = true;
			_hookClearedLabel.ForeColor = Constants.HOOK_CLEAR_LABEL_COLOR;
			_hookClearedLabel.Font = Constants.FORM_FONT;
			_hookClearedLabel.Click += hookButtonClick;
			_hookClearedLabel.Visible = false;

            // TODO: Proper DEBUG compiler stuff
            Controls.Add(_debug = new Label());
            _debug.Location = new Point(400, 0);
            _debug.AutoSize = true;
            _debug.ForeColor = Constants.FORM_FONT_COLOR;


            ResumeLayout();
		}

		private void hookButtonClick(object sender, EventArgs e)
        {
			if (_hookClearedLabel != null)
            {
				_hookCleared = true;
            }
        }

		private void SetObjectives(Metadata metadata)
		{
			if(romHash == metadata.binary_flags + metadata.seed)
				return;

			romHash = metadata.binary_flags + metadata.seed;

			foreach(var objectiveCheckbox in _objectivesCheckboxes)
            {
				Controls.Remove(objectiveCheckbox);
            }
			_objectivesCheckboxes.Clear();


			int currentY = Constants.OBJECTIVES_START_COORDS_Y;

			foreach (var objective in metadata.objectives)
            {
				currentY += 20;
				CheckBox objectiveCheckbox = new CheckBox();
				objectiveCheckbox.Location = new Point(Constants.OBJECTIVES_START_COORDS_X, currentY);
				objectiveCheckbox.ForeColor = Constants.FORM_FONT_COLOR;
				objectiveCheckbox.Font = Constants.FORM_FONT;
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

			int currentX = Constants.LOCATIONS_START_COORD_X;
			int currentY = Constants.LOCATIONS_START_COORD_Y + Constants.LOCATIONS_LABEL_HEADING_PADDING;

			List<KILocation> overworldLocations = KILocations.ListOfKILocations.Where
				(
					p => p.LocationArea == KILocationArea.Overworld &&
					p.LocationFlagType != KILocationFlagType.Trap &&
					p.Checked == false &&
					p.IsAvailable(_hookCleared)
				)
				.ToList();

			List<KILocation> underworldLocations = KILocations.ListOfKILocations.Where
				(
					p => p.LocationArea == KILocationArea.Underground &&
					p.LocationFlagType != KILocationFlagType.Trap &&
					p.Checked == false &&
					p.IsAvailable(_hookCleared)
				)
				.ToList();

			List<KILocation> moonLocations = KILocations.ListOfKILocations.Where
				(
					p => p.LocationArea == KILocationArea.Moon &&
					p.LocationFlagType != KILocationFlagType.Trap &&
					p.Checked == false &&
					p.IsAvailable(_hookCleared)
				)
				.ToList();

			_oLocationsLabel.Location = new Point(currentX, currentY);
			string tempLocationsText = "";
			foreach (var location in overworldLocations)
            {
				tempLocationsText += "\n" + location.Name;
            }
			_oLocationsLabel.Text = tempLocationsText;

			bool displayHookLabel = _hookCleared == false && (KeyItems.Hook.Obtained || Flags.OtherPushBToJump);

			if (displayHookLabel)
			{
				_hookClearedLabel.Location = new Point(currentX, currentY + _oLocationsLabel.Size.Height);
				_hookClearedLabel.Text = "[Clear Hook Route]";
				_hookClearedLabel.Visible = true;				
			}
			else
			{
				_hookClearedLabel.Visible = false;
			}

			_uLocationsLabel.Location = new Point(currentX, currentY + _oLocationsLabel.Size.Height);

			tempLocationsText = "";
			foreach (var location in underworldLocations)
			{
				tempLocationsText += "\n" + location.Name;
			}
			_uLocationsLabel.Text = tempLocationsText;

			_mLocationsLabel.Location = new Point(currentX, currentY + _oLocationsLabel.Size.Height + _uLocationsLabel.Size.Height);
			tempLocationsText = "";
			foreach (var location in moonLocations)
			{
				tempLocationsText += "\n" + location.Name;
			}
			_mLocationsLabel.Text = tempLocationsText;

			// TODO: Checked Locations
		}

		private void UpdateTraps()
        {
			int currentX = Constants.TRAPS_START_COORD_X;
			int currentY = Constants.TRAPS_START_COORD_Y + Constants.TRAPS_LABEL_HEADING_PADDING;

			bool showOverworldTraps = false;
			bool showUnderworldTraps = false;
			bool showMoonTraps = false;

			List<TrapChestArea> overworldTrapChestAreas = TrapChestAreas.ListOfAreas.Where
				(
					p => p.AreaMap == KILocationArea.Overworld &&
					p.IsAvailable(_hookCleared) &&
					p.TrapChests.Any(p => p.Checked == false)
				)
				.ToList();
			if (overworldTrapChestAreas.Count > 0)
				showOverworldTraps = true;

			List<TrapChestArea> underworldTrapChestAreas = TrapChestAreas.ListOfAreas.Where
				(
					p => p.AreaMap == KILocationArea.Underground &&
					p.IsAvailable(_hookCleared) &&
					p.TrapChests.Any(p => p.Checked == false)
				)
				.ToList();
			if (underworldTrapChestAreas.Count > 0)
				showUnderworldTraps = true;

			List<TrapChestArea> moonTrapChestAreas = TrapChestAreas.ListOfAreas.Where
				(
					p => p.AreaMap == KILocationArea.Moon &&
					p.IsAvailable(_hookCleared) &&
					p.TrapChests.Any(p => p.Checked == false)
				)
				.ToList();
			if(moonTrapChestAreas.Count > 0)
				showMoonTraps = true;


			_oTrapsLabel.Location = new Point(currentX, currentY);
			string tempTrapText = "";

			if(showOverworldTraps)
            {
				_oTrapsLabel.Visible = true;
				foreach (var area in overworldTrapChestAreas)
				{
					tempTrapText += "\n" + area.Name + " [" + area.TrapChests.Where(p => p.Checked == false).Count() + "]";
				}
			}
			else
            {
				_oTrapsLabel.Visible = false;
            }

			_oTrapsLabel.Text = tempTrapText;

			_uTrapsLabel.Location = new Point(currentX, currentY + _oTrapsLabel.Size.Height);
			tempTrapText = "";

			if (showUnderworldTraps)
			{
				_uTrapsLabel.Visible = true;
				foreach (var area in underworldTrapChestAreas)
				{
					tempTrapText += "\n" + area.Name + " [" + area.TrapChests.Where(p => p.Checked == false).Count() + "]";
				}

			}
			else
			{
				_uTrapsLabel.Visible = false;
			}

			_uTrapsLabel.Text = tempTrapText;

			_mTrapsLabel.Location = new Point(currentX, currentY + _oTrapsLabel.Size.Height + _uTrapsLabel.Size.Height);
			tempTrapText = "";

			if (showMoonTraps)
			{
				_mTrapsLabel.Visible = true;
				foreach (var area in moonTrapChestAreas)
				{
					tempTrapText += "\n" + area.Name + " [" + area.TrapChests.Where(p => p.Checked == false).Count() + "]";
				}

			}
			else
			{
				_mTrapsLabel.Visible = false;
			}

			_mTrapsLabel.Text = tempTrapText;

		}

		// Called on ROM start or restart
		public override void Restart()
		{
			Metadata metadata = GetMetadata();
			Flags.SetFlags(metadata);
			SetObjectives(metadata);

			_trapsLabel.Text = String.Empty;
			_trapsLabel.Location = new Point();

			if (Flags.Ktrap)
            {
				_trapsLabel.Location = new Point(Constants.TRAPS_START_COORD_X, Constants.TRAPS_START_COORD_Y);
				_trapsLabel.Text = "Trapped Chests";
			}

			_hookCleared = false;
		}

		// Called after every frame
		protected override void UpdateAfter()
		{
			if (APIs.Emulation.FrameCount() % 60 == 0)
				UpdateDisplay();
		}

		private void UpdateDisplay()
        {
			//_debug.Text = Debug();
			UpdateKeyItems();
			UpdateLocations();

			if(Flags.Ktrap)
				UpdateTraps();
		}

		private string Debug()
        {
			string output = "Debug: \n";

			// Debug stuff!

			// End debug stuff.

			return output;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FESightForm
            // 
            this.ClientSize = new System.Drawing.Size(420, 261);
            this.Name = "FESightForm";
            this.ResumeLayout(false);

        }
    }


}