using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using Newtonsoft.Json;

namespace FESight
{
    internal static class FESight
    {
        public static string CurrentRomHash { get; set; }
        public static Metadata CurrentMetaData { get; set; }
        public static bool InitializedBeforeFrames { get; set; }
        internal static void InitBeforeRomLoad()
        {
            KeyItems.InitializeKeyItems();
            TrapChestAreas.InitializeTrapChestAreas();
        }

        internal static void InitBeforeAnyFrame()
        {
            if (InitializedBeforeFrames)
                return;

            KILocations.InitializeKILocations(false, Flags.Kmain, Flags.Ksummon, Flags.Kmoon, Flags.Ktrap, Flags.Kunsafe || Flags.Kunsafer, Flags.Knofree);            
        }

        internal static void InitOnRestartNewRom(ApiContainer api)
        {
            Metadata metadata = GetMetadata(api);
            CurrentMetaData = metadata;
            CurrentRomHash = metadata.binary_flags + metadata.seed;

            Flags.SetFlags(metadata);
        }

        internal static Metadata GetMetadata(ApiContainer api)
        {
            bool worked;
            try
            {
                worked = api.Memory.UseMemoryDomain(Constants.CARTROM_STRING);

            }
            catch (Exception)
            {
                throw new Exception("Trouble access APIs");
            }

            string jsonString = "";

            if (worked)
            {
                try
                {
                    var bytes = api.Memory.ReadByteRange(Constants.FLAGS_LENGTH_ADDRESS, Constants.FLAGS_LENGTH_LENGTH);
                    bytes.Reverse();

                    int jsonLength = GetIntFromBytes(bytes.ToArray());
                    var jsonBytes = api.Memory.ReadByteRange(Constants.FLAGS_START_ADDRESS, jsonLength);
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

        // Assuming little endian. Anything this runs on is going to be little endian.
        public static int GetIntFromBytes(byte[] data)
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
