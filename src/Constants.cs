using System;
using System.Collections.Generic;
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

        public const string CARTROM_STRING = "CARTROM";
        public const string WRAM_STRING = "WRAM";
    }
}
