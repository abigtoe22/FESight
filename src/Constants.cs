﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    public static class Constants
    {
        public const int FLAGS_LENGTH_ADDRESS = 0x1FF000;
        public const int FLAGS_LENGTH_LENGTH = 4;
        public const int FLAGS_START_ADDRESS = 0x1FF004;

        public const int KI_LOCATIONS_START_ADDRESS = 0x1510;
        public const int KI_LOCATIONS_BYTES_LENGTH = 15;

        public const int KI_FOUND_START_ADDRESS = 0x1500;
        public const int KI_FOUND_BYTES_LENGTH = 3;

        public const int KI_USED_START_ADDRESS = 0x1503;
        public const int KI_USED_BYTES_LENGTH = 3;

        public const int OBJECTIVES_START_COORDS_X = 10;
        public const int OBJECTIVES_START_COORDS_Y = 210;
        public const int OBJECTIVES_HEIGHT = 12;
        public const int OBJECTIVES_LABEL_PADDING = 20;

        public const int DEBUG_OUTOUT_COORD_X = 500;
        public const int DEBUG_OUTOUT_COORD_Y = 500;

        public const int LOCATIONS_START_COORD_X = 400;
        public const int LOCATIONS_START_COORD_Y = 0;
        public const int LOCATIONS_HEIGHT = 12;
        public const int LOCATIONS_LABEL_HEADING_PADDING = 10;

        public const int TRAPS_START_COORD_X = 650;
        public const int TRAPS_START_COORD_Y = 0;
        public const int TRAPS_HEIGHT = 12;
        public const int TRAPS_LABEL_HEADING_PADDING = 10;

        public static Color OVERWORLD_LOCATIONS_COLOR = Color.FromArgb(0, 255, 153);
        public static Color UNDERGROUND_LOCATIONS_COLOR = Color.FromArgb(255, 0, 153);
        public static Color MOON_LOCATIONS_COLOR = Color.FromArgb(0, 153, 255);
        public static Color HOOK_CLEAR_LABEL_COLOR = Color.FromArgb(0, 153, 153);

        public static Font FORM_FONT = new Font(new FontFamily("Helvetica"), 10f);
        public static Color FORM_FONT_COLOR = Color.White;

        public const string CARTROM_STRING = "CARTROM";
        public const string WRAM_STRING = "WRAM";
    }
}
