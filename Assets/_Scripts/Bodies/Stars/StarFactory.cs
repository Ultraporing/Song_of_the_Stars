
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Bodies.Stars
{
    public enum ESpectralType
    {
        UNKNOWN = 0, O3, O5, O8, B0, B3, B5, B8, A0, A5, F0, F5, G0, G2, G5, K0, K5, M0, M5, M8, M9, L0, L2, L5, T0, T5, T8
    };

    public enum ELuminosityType
    {
        UNKNOWN = 0, MAIN_SEQUENCE_STAR, GIANT_STAR, SUPERGIANT_STAR, BROWN_DWARF, RED_DWARF, WHITE_DWARF, BLACK_HOLE, SUPERMASSIVE_BLACK_HOLE
    };

    public class StarFactory : MonoBehaviour
    {
        private int Seed;
        private Dictionary<KeyValuePair<int, int>, StarGenerationRange> StarGenRanges = new Dictionary<KeyValuePair<int, int>, StarGenerationRange>();
        private List<string> StarNamesBase = new List<string>();
        private List<string> UsedStarNames = new List<string>();

        public static string ELuminosityTypeToString(int type)
        {
            switch ((ELuminosityType)type)
            {
                case ELuminosityType.MAIN_SEQUENCE_STAR:
                    return "V (MAIN SEQUENCE STAR)";
                case ELuminosityType.GIANT_STAR:
                    return "III (GIANT STAR)";
                case ELuminosityType.SUPERGIANT_STAR:
                    return "I (SUPERGIANT STAR)";
                case ELuminosityType.BROWN_DWARF:
                    return "(BROWN DWARF)";
                case ELuminosityType.RED_DWARF:
                    return "(RED DWARF)";
                case ELuminosityType.WHITE_DWARF:
                    return "(WHITE DWARF)";
                case ELuminosityType.BLACK_HOLE:
                    return "(BLACK HOLE)";
                case ELuminosityType.SUPERMASSIVE_BLACK_HOLE:
                    return "(SUPERMASSIVE BLACK HOLE)";
                default:
                    return "UNKNOWN";
            }
        }

        public static string ESpectralTypeToString(int type)
        {
            switch ((ESpectralType)type)
            {
                case ESpectralType.O3:
                    return "O3";
                case ESpectralType.O5:
                    return "O5";
                case ESpectralType.O8:
                    return "O8";
                case ESpectralType.B0:
                    return "B0";
                case ESpectralType.B3:
                    return "B3";
                case ESpectralType.B5:
                    return "B5";
                case ESpectralType.B8:
                    return "B8";
                case ESpectralType.A0:
                    return "A0";
                case ESpectralType.A5:
                    return "A5";
                case ESpectralType.F0:
                    return "F0";
                case ESpectralType.F5:
                    return "F5";
                case ESpectralType.G0:
                    return "G0";
                case ESpectralType.G2:
                    return "G2";
                case ESpectralType.G5:
                    return "G5";
                case ESpectralType.K0:
                    return "K0";
                case ESpectralType.K5:
                    return "K5";
                case ESpectralType.M0:
                    return "M0";
                case ESpectralType.M5:
                    return "M5";
                case ESpectralType.M8:
                    return "M8";
                case ESpectralType.M9:
                    return "M9";
                case ESpectralType.L0:
                    return "L0";
                case ESpectralType.L2:
                    return "L2";
                case ESpectralType.L5:
                    return "L5";
                case ESpectralType.T0:
                    return "T0";
                case ESpectralType.T5:
                    return "T5";
                case ESpectralType.T8:
                    return "T8";
                default:
                    return "UNKNOWN";
            }
        }

        void Start()
        {

        }

        public void InitStarFactory(int seed)
        {
            this.Seed = seed;
            LoadGenerationRangesFromFile("StarRanges");
            LoadStarNamesFromFile("StarNames");
        }

        private void LoadGenerationRangesFromFile(string file)
        {
            TextAsset textAss = Resources.Load(file) as TextAsset;
            string[] fileLines = textAss.text.Split('}');
            //new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries
            foreach (string s in fileLines)
            {
                if (s == string.Empty || s == "\n")
                    continue;

                string sp = FixJSONline(s);

                try
                {
                    StarGenerationRange range = JsonUtility.FromJson<StarGenerationRange>(sp);
                    StarGenRanges.Add(new KeyValuePair<int, int>(range.SpectralType, range.LuminosityType), range);
                }
                catch
                {
                    Debug.Log("error");
                }
                
            }
        }

        private void LoadStarNamesFromFile(string file)
        {
            TextAsset textAss = Resources.Load(file) as TextAsset;
            string[] fileLines = textAss.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in fileLines)
            {
                if (s == string.Empty)
                    continue;

                StarNamesBase.Add(s);
            }
        }

        public static string FixJSONline(string s)
        {
            string sp = s + '}';
            sp = Regex.Replace(sp, @"\s+", string.Empty);
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", '[', ']')); // comments are in []
            sp = regex.Replace(sp, string.Empty);

            return sp;
        }

        public Star GenerateStar()
        {
            Seed = GalaxyFactory.Seed;
            RandomGenerator.SetSeed(Seed);
            Star s = new Star();
            //s.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
            //s.SpectralType = (int)ESpectralType.O3;
            GenerateLuminosityType(ref s);
            GenerateStarData(ref s);

            return s;
        }

        private bool GenerateStarData(ref Star star)
        {
            if (StarGenRanges.ContainsKey(new KeyValuePair<int, int>(star.SpectralType, star.LuminosityType)))
            {
                StarGenerationRange range = StarGenRanges[new KeyValuePair<int, int>(star.SpectralType, star.LuminosityType)];

                star.MassSuns = RandomGenerator.GenerateDouble(range.SunMassesMin, range.SunMassesMax);
                star.RadiusSuns = RandomGenerator.GenerateDouble(range.SunRadiusesMin, range.SunRadiusesMax);
                star.LuminositySuns = RandomGenerator.GenerateDouble(range.SunLuminositiesMin, range.SunLuminositiesMax);
                star.LifetimeMyears = range.LifetimeMyears;
                star.HabitableZoneAU = RandomGenerator.GenerateDouble(range.HabitableZoneAUMin, range.HabitableZoneAUMax);
                star.TemperatureKelvin = RandomGenerator.GenerateInt(range.TemperatureKelvinMin, range.TemperatureKelvinMax);

                string sName = StarNamesBase[RandomGenerator.GenerateInt(0, StarNamesBase.Count)] + " - " + RandomGenerator.GenerateInt(0, 1000000);
                if (!UsedStarNames.Contains(sName))
                {
                    UsedStarNames.Add(sName);
                    star.Name = sName;
                }
                else
                {
                    do
                    {
                        sName = StarNamesBase[RandomGenerator.GenerateInt(0, StarNamesBase.Count)] + " - " + RandomGenerator.GenerateInt(0, 1000000);
                    } while (UsedStarNames.Contains(sName));

                    UsedStarNames.Add(sName);
                    star.Name = sName;
                }


                return true;
            }

            return false;
        }

        private void GenerateLuminosityType(ref Star star)
        {
            int r1 = RandomGenerator.GenerateInt(0, 2000000000);

            // Black Hole chance: 0.000000000005
            if (r1 == 0)
            {
                star.LuminosityType = (int)ELuminosityType.BLACK_HOLE;
                star.SpectralType = (int)ESpectralType.UNKNOWN;
                return;
            }
            else
            {
                int r = RandomGenerator.GenerateInt(0, 100000000);

                // MainSpectral O Class Star chance: 0.00000001
                if (r == 0)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 3);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.O3;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.O5;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.O8;
                            break;
                    }
                }
                // MainSpectral B Class Star chance: 0.001
                else if (r > 0 && r < 100001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 4);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.B0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.B3;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.B5;
                            break;
                        case 3:
                            star.SpectralType = (int)ESpectralType.B8;
                            break;
                    }
                }
                // MainSpectral A Class Star chance: 0.007
                else if (r > 100000 && r < 700001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 2);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.A0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.A5;
                            break;
                    }
                }
                // MainSpectral F Class Star chance: 0.02
                else if (r > 700000 && r < 2700001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 2);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.F0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.F5;
                            break;
                    }
                }
                // MainSpectral G Class Star chance: 0.035
                else if (r > 2700000 && r < 6200001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 3);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.G0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.G2;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.G5;
                            break;
                    }
                }
                // MainSpectral K Class Star chance: 0.08
                else if (r > 6200000 && r < 14200001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 2);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.K0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.K5;
                            break;
                    }
                }
                // MainSpectral M Class Star chance: 0.80
                else if (r > 14200000 && r < 94200001)
                {
                    star.LuminosityType = (int)ELuminosityType.MAIN_SEQUENCE_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 3);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.M0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.M5;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.M8;
                            break;
                    }
                }
                // Giant Star chance: 0.01933333
                else if (r > 94200000 && r < 96133334)
                {
                    star.LuminosityType = (int)ELuminosityType.GIANT_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 10);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.B0;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.B5;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.A0;
                            break;
                        case 3:
                            star.SpectralType = (int)ESpectralType.F0;
                            break;
                        case 4:
                            star.SpectralType = (int)ESpectralType.G0;
                            break;
                        case 5:
                            star.SpectralType = (int)ESpectralType.G5;
                            break;
                        case 6:
                            star.SpectralType = (int)ESpectralType.K0;
                            break;
                        case 7:
                            star.SpectralType = (int)ESpectralType.K5;
                            break;
                        case 8:
                            star.SpectralType = (int)ESpectralType.M0;
                            break;
                        case 9:
                            star.SpectralType = (int)ESpectralType.M5;
                            break;
                    }
                }
                // Super Giant Star chance: 0.01933333
                else if (r > 96133333 && r < 98066667)
                {
                    star.LuminosityType = (int)ELuminosityType.SUPERGIANT_STAR;
                    int rr = RandomGenerator.GenerateInt(0, 9);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.O5;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.B0;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.A0;
                            break;
                        case 3:
                            star.SpectralType = (int)ESpectralType.F0;
                            break;
                        case 4:
                            star.SpectralType = (int)ESpectralType.G0;
                            break;
                        case 5:
                            star.SpectralType = (int)ESpectralType.G5;
                            break;
                        case 6:
                            star.SpectralType = (int)ESpectralType.K0;
                            break;
                        case 7:
                            star.SpectralType = (int)ESpectralType.K5;
                            break;
                        case 8:
                            star.SpectralType = (int)ESpectralType.M0;
                            break;
                    }
                }
                // Brown Dwarf chance: 0.01933333
                else if (r > 98066666)
                {
                    star.LuminosityType = (int)ELuminosityType.BROWN_DWARF;
                    int rr = RandomGenerator.GenerateInt(0, 8);
                    switch (rr)
                    {
                        case 0:
                            star.SpectralType = (int)ESpectralType.M8;
                            break;
                        case 1:
                            star.SpectralType = (int)ESpectralType.M9;
                            break;
                        case 2:
                            star.SpectralType = (int)ESpectralType.L0;
                            break;
                        case 3:
                            star.SpectralType = (int)ESpectralType.L2;
                            break;
                        case 4:
                            star.SpectralType = (int)ESpectralType.L5;
                            break;
                        case 5:
                            star.SpectralType = (int)ESpectralType.T0;
                            break;
                        case 6:
                            star.SpectralType = (int)ESpectralType.T5;
                            break;
                        case 7:
                            star.SpectralType = (int)ESpectralType.T8;
                            break;
                    }
                } 
            }
        }
    }
}
