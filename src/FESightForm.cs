using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly Label _hookClearedLabel;

		private readonly Stopwatch _stopWatch;
		private readonly Label _stopWatchLabel;
		private readonly Button _stopWatchStartButton;
		private readonly Button _stopWatchPauseButton;
		private readonly Button _stopWatchRestartButton;

		private List<Label> _oTrapLabels;
		private List<Label> _uTrapLabels;
		private List<Label> _mTrapLabels;

		private Label _kiTotalLabel;
		private PictureBox _dMistPictureBox;

		private bool _hookCleared;

		private readonly Label _debug;

		internal ApiContainer APIs => _maybeAPIContainer!;

		protected override string WindowTitleStatic => "FE Sight";
        public override bool BlocksInputWhenFocused { get { return false; } }

        public FESightForm()
		{
			FESight.InitBeforeRomLoad();
			InitKeyItemIcons();

			ClientSize = new Size(Constants.CLIENT_SIZE_X, Constants.CLIENT_SIZE_Y);
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

			_oTrapLabels = new List<Label>();
			_uTrapLabels = new List<Label>();
			_mTrapLabels = new List<Label>();

			Controls.Add(_hookClearedLabel = new Label());
			_hookClearedLabel.AutoSize = true;
			_hookClearedLabel.ForeColor = Constants.HOOK_CLEAR_LABEL_COLOR;
			_hookClearedLabel.Font = Constants.FORM_FONT;
			_hookClearedLabel.Click += hookLabelClick;
			_hookClearedLabel.Visible = false;

			_stopWatch = new Stopwatch();
			Controls.Add(_stopWatchLabel = new Label());
			_stopWatchLabel.AutoSize = true;
			_stopWatchLabel.Location = new Point(10, ClientSize.Height - 110);
			_stopWatchLabel.ForeColor = Constants.FORM_FONT_COLOR;
			_stopWatchLabel.Font = new Font(new FontFamily("Helvetica"), 40f);
			_stopWatchLabel.Text = "0:00:00";

			Controls.Add(_stopWatchStartButton = new Button());
			_stopWatchStartButton.Location = new Point(_stopWatchLabel.Location.X, _stopWatchLabel.Location.Y + 70);
			_stopWatchStartButton.Text = "Start";
			_stopWatchStartButton.BackColor = Color.White;
			_stopWatchStartButton.Width = 60;
			_stopWatchStartButton.Click += StartStopWatch;

			Controls.Add(_stopWatchPauseButton = new Button());
			_stopWatchPauseButton.Location = new Point(_stopWatchStartButton.Location.X + _stopWatchStartButton.Size.Width + 10, _stopWatchStartButton.Location.Y);
			_stopWatchPauseButton.Text = "Pause";
			_stopWatchPauseButton.BackColor = Color.White;
			_stopWatchPauseButton.Width = _stopWatchStartButton.Width;
			_stopWatchPauseButton.Click += PauseStopWatch;

			Controls.Add(_stopWatchRestartButton = new Button());
			_stopWatchRestartButton.Location = new Point(_stopWatchPauseButton.Location.X + _stopWatchPauseButton.Size.Width + 10, _stopWatchPauseButton.Location.Y);
			_stopWatchRestartButton.Text = "Restart";
			_stopWatchRestartButton.BackColor = Color.White;
			_stopWatchRestartButton.Width = _stopWatchStartButton.Width;
			_stopWatchRestartButton.Click += RestartStopWatch;

			Controls.Add(_dMistPictureBox = new PictureBox());
			Controls.Add(_kiTotalLabel = new Label());
			_kiTotalLabel.Location = new Point(Constants.KI_TOTAL_LABEL_X, Constants.KI_TOTAL_LABEL_Y);
			_kiTotalLabel.Font = Constants.FORM_FONT;
			_kiTotalLabel.ForeColor = Constants.FORM_FONT_COLOR;

			// TODO: Proper DEBUG compiler stuff
			Controls.Add(_debug = new Label());
            _debug.Location = new Point(400, 400);
            _debug.AutoSize = true;
            _debug.ForeColor = Constants.FORM_FONT_COLOR;


            ResumeLayout();
		}

        private void InitKeyItemIcons()
        {
			int ICON_SPACE = 40;

			int count = 0;
			int x = 0;
			int y = 0;

			foreach (var keyItem in KeyItems.KeyItemList)
			{
				if (count % 4 == 0 && count != 0)
				{
					y = y + ICON_SPACE;
					x = 0;
				}

				if (keyItem.Name == "Rat Tail")
				{
					x = x + ICON_SPACE;
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

		private void hookLabelClick(object sender, EventArgs e)
        {
			if (_hookClearedLabel != null)
            {
				_hookCleared = true;
            }
        }

		private void SetObjectives(Metadata metadata)
		{
			foreach (var objectiveCheckbox in _objectivesCheckboxes)
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

			if (Flags.Knofree)
			{
				Controls.Remove(_dMistPictureBox);
				_dMistPictureBox = new PictureBox();
				_dMistPictureBox.ImageLocation = "../src/Icons/FFIVFE-Bosses-1MistD-Gray.png";
				_dMistPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
				_dMistPictureBox.Location = new Point(Constants.DMIST_LABEL_X, Constants.DMIST_LABEL_Y);
				_dMistPictureBox.Click += ClickDMist;
				Controls.Add(_dMistPictureBox);
			}
			else
            {
				Controls.Remove(_dMistPictureBox);
            }

			if (Flags.Ktrap)
            {
				TrapChestAreas.ResetChestTotals();
				Controls.Add(_trapsLabel);

				int currentTrapsX = Constants.TRAPS_START_COORD_X;
				int currentTrapsY = Constants.TRAPS_START_COORD_Y + Constants.TRAPS_LABEL_HEADING_PADDING;

				foreach (var area in TrapChestAreas.GetOverworldTrapChests())
				{
					currentTrapsY += Constants.TRAPS_HEIGHT;
					Label newLabel = new Label();
					newLabel.Text = area.Name + String.Format(" [{0}/{1}]", area.Current, area.Total);
					newLabel.Location = new Point(currentTrapsX, currentTrapsY);
					newLabel.Font = Constants.FORM_FONT;
					newLabel.ForeColor = Constants.OVERWORLD_LOCATIONS_COLOR;
					newLabel.AutoSize = true;
					newLabel.Click += (sender, e) => ClickTrapsLabel(area, sender, e);
					_oTrapLabels.Add(newLabel);
					Controls.Add(newLabel);
				}

				currentTrapsY += Constants.TRAPS_LABEL_HEADING_PADDING;

				foreach (var area in TrapChestAreas.GetUnderworldTrapChests())
                {
					currentTrapsY += Constants.TRAPS_HEIGHT;
					Label newLabel = new Label();
                    newLabel.Text = area.Name + String.Format(" [{0}/{1}]", area.Current, area.Total);
                    newLabel.Location = new Point(currentTrapsX, currentTrapsY);
                    newLabel.Font = Constants.FORM_FONT;
                    newLabel.ForeColor = Constants.UNDERGROUND_LOCATIONS_COLOR;
                    newLabel.AutoSize = true;
                    newLabel.Click += (sender, e) => ClickTrapsLabel(area, sender, e);
                    _uTrapLabels.Add(newLabel);
                    Controls.Add(newLabel);
                }

				currentTrapsY += Constants.TRAPS_LABEL_HEADING_PADDING;

				foreach (var area in TrapChestAreas.GetMoonTrapChests())
                {
					currentTrapsY += Constants.TRAPS_HEIGHT;
					Label newLabel = new Label();
                    newLabel.Text = area.Name + String.Format(" [{0}/{1}]", area.Current, area.Total);
                    newLabel.Location = new Point(currentTrapsX, currentTrapsY);
                    newLabel.Font = Constants.FORM_FONT;
                    newLabel.ForeColor = Constants.MOON_LOCATIONS_COLOR;
                    newLabel.AutoSize = true;
                    newLabel.Click += (sender, e) => ClickTrapsLabel(area, sender, e);
                    _mTrapLabels.Add(newLabel);
                    Controls.Add(newLabel);
                }
            }
			else
			{
				RemoveTrapLabels();
			}
		}

		private void RemoveTrapLabels()
        {
			Controls.Remove(_trapsLabel);

			foreach (var trap in _oTrapLabels)
			{
				Controls.Remove(trap);
			}

			foreach (var trap in _uTrapLabels)
			{
				Controls.Remove(trap);
			}

			foreach (var trap in _mTrapLabels)
			{
				Controls.Remove(trap);
			}
		}

		private void ClickTrapsLabel(TrapChestArea area, object sender, EventArgs e)
        {
			Label label = (Label)sender;
			area.Current--;
			if (area.Current < 0)
				area.Current = area.Total;
			
			label.Text = area.Name + String.Format(" [{0}/{1}]", area.Current, area.Total);
		}

        private void UpdateKeyItems()
		{
			APIs.Memory.UseMemoryDomain(Constants.WRAM_STRING);

			List<byte> bytes;

			bytes = APIs.Memory.ReadByteRange(0x1500, 3);
			BitArray kiFlags = new BitArray(bytes.ToArray());
			foreach (var keyItem in KeyItems.KeyItemList)
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

			_kiTotalLabel.Text = KeyItems.KeyItemList.Where(p => p.Name != "<Warp Glitch>" && p.Obtained).Count() + " / " + 17;
		}

		private void UpdateLocations()
        {
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

		// Called on ROM start or restart
		public override void Restart()
		{
			//FESight.InitBeforeAnyFrame();
			Metadata metadata = FESight.GetMetadata(APIs);

			FESight.CurrentRomHash = metadata.binary_flags + metadata.seed;

			if(FESight.CurrentRomHash == metadata.binary_flags + metadata.seed)
            {
				InitializeOnRestartNewROM();
            }
			
			InitializeOnRestart();
		}

        private void InitializeOnRestart()
        {
			

			_trapsLabel.Text = String.Empty;
			_trapsLabel.Location = new Point();

			if (Flags.Ktrap)
			{
				_trapsLabel.Location = new Point(Constants.TRAPS_START_COORD_X, Constants.TRAPS_START_COORD_Y);
				_trapsLabel.Text = "Trapped Chests";
			}

			_hookCleared = false;
		}

		private void InitializeOnRestartNewROM()
        {
			FESight.InitOnRestartNewRom(APIs);
			SetObjectives(FESight.CurrentMetaData);
		}

		// Called after every frame
		protected override void UpdateAfter()
		{
			if (APIs.Emulation.FrameCount() % 60 == 0)
            {
				FESight.InitBeforeAnyFrame();
				UpdateDisplay();
			}	
		}

		private void UpdateDisplay()
        {
			//_debug.Text = Debug();
			UpdateStopWatch();
			UpdateKeyItems();
			UpdateLocations();
			if (Flags.Ktrap)
				UpdateTrapLabels();
		}

        private void UpdateTrapLabels()
        {
            
        }

        private string Debug()
        {
			string output = "Debug: \n";

			// Debug stuff!

			// End debug stuff.

			return output;
		}

		private void StartStopWatch(object sender, EventArgs e)
        {
			_stopWatch.Start();
        }

		private void PauseStopWatch(object sender, EventArgs e)
        {
			_stopWatch.Stop();
        }

		private void RestartStopWatch(object sender, EventArgs e)
        {
			_stopWatch.Restart();
			_stopWatch.Stop();
        }

		private void UpdateStopWatch()
        {
			_stopWatchLabel.Text = _stopWatch.Elapsed.Hours.ToString().PadLeft(1, '0')
				+ ":" + _stopWatch.Elapsed.Minutes.ToString().PadLeft(2, '0')
				+ ":" + _stopWatch.Elapsed.Seconds.ToString().PadLeft(2, '0');
		}

		private void ClickDMist(object sender, EventArgs e)
        {
			if(KILocations.DMistChecked)
            {
				_dMistPictureBox.ImageLocation = "../src/Icons/FFIVFE-Bosses-1MistD-Gray.png";
				KILocations.DMistChecked = false;

			}
			else
            {
				_dMistPictureBox.ImageLocation = "../src/Icons/FFIVFE-Bosses-1MistD-Color.png";
				KILocations.DMistChecked = true;
			}
		}

		// Not used unless I decide to use the forms designer
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