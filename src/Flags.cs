using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESight
{
    internal class Flags
    {
        //Key Item flags
        public bool Kmain { get; set; }
        public bool Ksummon { get; set; }
        public bool Ktrap { get; set; }
        public bool Knofree { get; set; }
        public bool Kforce { get; set; }
        public string KforceSetting { get; set; }
        public bool Kunsafe { get; set; }
        public bool Kunsafer { get; set; }

        //Pass flags
        public bool Pshop { get; set; }
        public bool Pkey { get; set; }
        public bool Pchests { get; set; }

        //Character flags
        public bool Cvanilla { get; set; }
        public bool Cnofree { get; set; }
        public bool Cnoearned { get; set; }
        public List<string> StartingCharacters { get; set; }
        public List<string> CharacterPool { get; set; }
        public bool Cjspells { get; set; }
        public bool Cjabilities { get; set; }
        public bool Cnekkie { get; set; }
        public bool Cnodupes { get; set; }
        public bool Cbye { get; set; }
        public int Cmaxparty { get; set; }
        public bool Cpermajoin { get; set; }
        public bool Cpermadeath { get; set; }
        public bool Cpermadeader { get; set; }
        public bool Chero { get; set; }
        public string CheroName { get; set; }

        //Treasure flags
        public bool Tvanilla { get; set; }
        public bool Tshuffle { get; set; }
        public bool Tstandard { get; set; }
        public bool Tpro { get; set; }
        public bool Twild { get; set; }
        public bool Twildish { get; set; }
        public bool Tempty { get; set; }
        public bool Tsparse { get; set; }
        public int TsparsePercent { get; set; }
        public bool Tnoj { get; set; }
        public bool Tjunk { get; set; }
        public bool Tmoney { get; set; }

        //Shop flags
        public bool Svanilla { get; set; }
        public bool Sshuffle { get; set; }
        public bool Sstandard { get; set; }
        public bool Spro { get; set; }
        public bool Swild { get; set; }
        public bool Scabins { get; set; }
        public bool Sempty { get; set; }
        public bool Sfree { get; set; }
        public bool SsellQuarter { get; set; }
        public bool Ssell0 { get; set; }
        public bool Snoj { get; set; }
        public bool SnoApples { get; set; }
        public bool SnoSirens { get; set; }
        public bool SnoLife { get; set; }

        //Boss flags
        public bool Bvanilla { get; set; }
        public bool Bstandard { get; set; }
        public bool BnoFree { get; set; }
        public bool BaltGauntlet  { get; set; }
        public bool BwhyBurn { get; set; }
        public bool BwhichBurn { get; set; }

        //Encounter flags
        public bool Evanilla { get; set; }
        public bool Etoggle { get; set; }
        public bool Ereduce { get; set; }
        public bool EnoEncounters { get; set; }
        public bool EnoSirens { get; set; }
        public bool EnoJDrops { get; set; }
        public bool EcantRun { get; set; }
        public bool EnoExp { get; set; }

        //Glitch flags
        public bool Gdupe { get; set; }
        public bool Gmp { get; set; }
        public bool Gwarp { get; set; }
        public bool Glife { get; set; }
        public bool Gsylph { get; set; }
        public bool Gbackrow { get; set; }
        public bool G64 { get; set; }

        //Other flags
        public string OStarterKit1 { get; set; }
        public string OStarterKit2 { get; set; }
        public string OStarterKit3 { get; set; }
        public bool ONoAdamants { get; set; }
        public bool ONoCursed { get; set; }
        public bool OSpoon { get; set; }
        public bool OsmithAlt { get; set; }
        public bool OsmithSuper { get; set; }
        public bool OexpSplit { get; set; }
        public bool OexpNoBoost { get; set; }
        public bool OnoKeyBonus { get; set; }
        public bool OVanillaFuSoYa { get; set; }
        public bool OVanillaAgility { get; set; }
        public bool OVanillaHobs { get; set; }
        public bool OVanillaGrowUp { get; set; }
        public bool OVanillaFashion { get; set; }
        public bool OVanillaTraps { get; set; }
        public bool OVanillaGiant { get; set; }
        public bool OVanillaZ { get; set; }
        public bool OVintage { get; set; }
        public bool OPushBToJump { get; set; }

        //Wacky Flags - to do

        //Spoiler Flags
        public bool SpoilAll { get; set; }
        public bool SpoilKeyItems { get; set; }
        public bool SpoilRewards { get; set; }
        public bool SpoilChars { get; set; }
        public bool SpoilTreasure { get; set; }
        public bool SpoilTraps { get; set; }
        public bool SpoilShops { get; set; }
        public bool SpoilBosses { get; set; }
        public bool SpoilMisc { get; set; }
        public bool SpoilSparse { get; set; }
        public int SpoilSparsePercent { get; set; }
    }
}
