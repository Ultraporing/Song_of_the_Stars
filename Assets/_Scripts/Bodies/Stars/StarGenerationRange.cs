using System;
using System.Collections.Generic;

namespace Bodies.Stars
{
    [Serializable]
    public class StarGenerationRange
    {
        public int SpectralType;
        public int LuminosityType;
        public double SunMassesMin;
        public double SunMassesMax;
        public double SunRadiusesMin;
        public double SunRadiusesMax;
        public double SunLuminositiesMin;
        public double SunLuminositiesMax;
        public int LifetimeMyears;
        public double HabitableZoneAUMin;
        public double HabitableZoneAUMax;
        public int TemperatureKelvinMin;
        public int TemperatureKelvinMax;
    }
}
