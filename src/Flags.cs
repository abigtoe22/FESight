using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    public static class Flags
    {
        //Objective flags
        public static bool OwinCyrstal { get; set; }
        public static string ORequiredNumber { get; set; }

        //Key Item flags
        public static bool Kmain { get; set; }
        public static bool Ksummon { get; set; }
        public static bool Kmoon { get; set; }
        public static bool Ktrap { get; set; }
        public static bool Knofree { get; set; }
        public static bool Kforce { get; set; }
        public static string KforceSetting { get; set; }
        public static bool Kunsafe { get; set; }
        public static bool Kunsafer { get; set; }

        //Pass flags
        public static bool Pshop { get; set; }
        public static bool Pkey { get; set; }
        public static bool Pchests { get; set; }

        //Character flags
        public static bool Cvanilla { get; set; }
        public static bool Cnofree { get; set; }
        public static bool Cnoearned { get; set; }
        public static List<string> StartingCharacters { get; set; }
        public static List<string> CharacterPool { get; set; }
        public static bool Cjspells { get; set; }
        public static bool Cjabilities { get; set; }
        public static bool Cnekkie { get; set; }
        public static bool Cnodupes { get; set; }
        public static bool Cbye { get; set; }
        public static int Cmaxparty { get; set; }
        public static bool Cpermajoin { get; set; }
        public static bool Cpermadeath { get; set; }
        public static bool Cpermadeader { get; set; }
        public static bool Chero { get; set; }
        public static string CheroName { get; set; }

        //Treasure flags
        public static bool Tvanilla { get; set; }
        public static bool Tshuffle { get; set; }
        public static bool Tstandard { get; set; }
        public static bool Tpro { get; set; }
        public static bool Twild { get; set; }
        public static bool Twildish { get; set; }
        public static bool Tempty { get; set; }
        public static bool Tsparse { get; set; }
        public static int TsparsePercent { get; set; }
        public static bool Tnoj { get; set; }
        public static bool Tjunk { get; set; }
        public static bool Tmoney { get; set; }

        //Shop flags
        public static bool Svanilla { get; set; }
        public static bool Sshuffle { get; set; }
        public static bool Sstandard { get; set; }
        public static bool Spro { get; set; }
        public static bool Swild { get; set; }
        public static bool Scabins { get; set; }
        public static bool Sempty { get; set; }
        public static bool Sfree { get; set; }
        public static bool SsellQuarter { get; set; }
        public static bool Ssell0 { get; set; }
        public static bool Snoj { get; set; }
        public static bool SnoApples { get; set; }
        public static bool SnoSirens { get; set; }
        public static bool SnoLife { get; set; }

        //Boss flags
        public static bool Bvanilla { get; set; }
        public static bool Bstandard { get; set; }
        public static bool BnoFree { get; set; }
        public static bool BaltGauntlet  { get; set; }
        public static bool BwhyBurn { get; set; }
        public static bool BwhichBurn { get; set; }

        //Encounter flags
        public static bool Evanilla { get; set; }
        public static bool Etoggle { get; set; }
        public static bool Ereduce { get; set; }
        public static bool EnoEncounters { get; set; }
        public static bool EnoSirens { get; set; }
        public static bool EnoJDrops { get; set; }
        public static bool EcantRun { get; set; }
        public static bool EnoExp { get; set; }

        //Glitch flags
        public static bool Gdupe { get; set; }
        public static bool Gmp { get; set; }
        public static bool Gwarp { get; set; }
        public static bool Glife { get; set; }
        public static bool Gsylph { get; set; }
        public static bool Gbackrow { get; set; }
        public static bool G64 { get; set; }

        //Other flags
        public static string OtherStarterKit1 { get; set; }
        public static string OtherStarterKit2 { get; set; }
        public static string OtherStarterKit3 { get; set; }
        public static bool OtherNoAdamants { get; set; }
        public static bool OtherNoCursed { get; set; }
        public static bool OtherSpoon { get; set; }
        public static bool OthersmithAlt { get; set; }
        public static bool OthersmithSuper { get; set; }
        public static bool OtherexpSplit { get; set; }
        public static bool OtherexpNoBoost { get; set; }
        public static bool OthernoKeyBonus { get; set; }
        public static bool OtherVanillaFuSoYa { get; set; }
        public static bool OtherVanillaAgility { get; set; }
        public static bool OtherVanillaHobs { get; set; }
        public static bool OtherVanillaGrowUp { get; set; }
        public static bool OtherVanillaFashion { get; set; }
        public static bool OtherVanillaTraps { get; set; }
        public static bool OtherVanillaGiant { get; set; }
        public static bool OtherVanillaZ { get; set; }
        public static bool OtherVintage { get; set; }
        public static bool OtherPushBToJump { get; set; }

        //Wacky Flags - to do

        //Spoiler Flags
        public static bool SpoilAll { get; set; }
        public static bool SpoilKeyItems { get; set; }
        public static bool SpoilRewards { get; set; }
        public static bool SpoilChars { get; set; }
        public static bool SpoilTreasure { get; set; }
        public static bool SpoilTraps { get; set; }
        public static bool SpoilShops { get; set; }
        public static bool SpoilBosses { get; set; }
        public static bool SpoilMisc { get; set; }
        public static bool SpoilSparse { get; set; }
        public static int SpoilSparsePercent { get; set; }


        public static void SetFlags(Metadata metadata)
        {
            OwinCyrstal = false;
            Kmain = false;
            Ksummon = false;
            Kmoon = false;
            Ktrap = false;
            Knofree = false;
            Kforce = false;
            KforceSetting = "";
            Kunsafe = false;
            Kunsafer = false;
            OtherPushBToJump = false;

            List<string> flagSections = metadata.flags.Split(' ').ToList();

            foreach (string section in flagSections)
            {
                if(section.ToLower().StartsWith("o"))
                {
                    string objectiveFlags = section.ToLower();

                    if(objectiveFlags.Contains("win:crystal"))
                        OwinCyrstal = true;

                    List<string> objectiveSections = section.ToLower().Split(new string[] { "req:"}, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if(objectiveSections.Count > 1 && string.IsNullOrWhiteSpace(objectiveSections[1]) == false)
                    {
                        List<string> reqSections = objectiveSections[1].Split('/').ToList();

                        if(reqSections.Any() == true)
                            ORequiredNumber = reqSections[0];
                    }
                }

                if(section.ToLower().StartsWith("k"))
                {
                    string keyItemFlags = section.ToLower();

                    if (keyItemFlags.Contains("moon"))
                        Kmoon = true;
                    if (keyItemFlags.Contains("summon"))
                        Ksummon = true;
                    if (keyItemFlags.Contains("main"))
                        Kmain = true;
                    if (keyItemFlags.Contains("trap"))
                        Ktrap = true;
                    if (keyItemFlags.Contains("nofree"))
                        Knofree = true;
                    if (keyItemFlags.Contains("unsafe"))
                        Kunsafe = true;
                    if (keyItemFlags.Contains("unsafer"))
                    {
                        Kunsafe = false;
                        Kunsafer = true;
                    }
                }

                if (section.ToLower().StartsWith("g"))
                {
                    string glitchFlags = section.ToLower();

                    if (glitchFlags.Contains("warp"))
                        Gwarp = true;
                }

            }




            if (metadata.flags.ToLower().Contains("pushbtojump"))
                OtherPushBToJump = true;
        }
    }
}
