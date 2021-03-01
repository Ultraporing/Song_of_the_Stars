using Assets._Scripts;
using System;
using UnityEngine;

namespace Bodies.Stars
{
    public class Star
    {
        public int SpectralType;
        public int LuminosityType;
        public int TemperatureKelvin;
        public double RadiusSuns;
        public double MassSuns;
        public double LuminositySuns;
        public int LifetimeMyears;
        public double HabitableZoneAU;
        public string Name;
        public Vector2d Position;
        public Color32 StarColor;

        public Star(int spectralType = 0, int luminosityType = 0, int temperatureKelvin = 0, double radiusSuns = 0, double massSuns = 0, double luminositySuns = 0, int lifetimeMyears = 0, double habitableZoneAU = 0, string name = "", double posX = 0, double posY = 0)
        {
            SpectralType = spectralType;
            LuminosityType = luminosityType;
            TemperatureKelvin = temperatureKelvin;
            RadiusSuns = radiusSuns;
            MassSuns = massSuns;
            LuminositySuns = luminositySuns;
            LifetimeMyears = lifetimeMyears;
            HabitableZoneAU = habitableZoneAU;
            Position = new Vector2d(posX, posY);
            Name = name;

            uint r = 0, g = 0, b = 0;
            GameHelper.getRGBfromTemperature(ref r, ref g, ref b, (uint)TemperatureKelvin);
            StarColor = new Color32((byte)r, (byte)g, (byte)b, 255);
        }

        public new string ToString()
        {
            return "Star [Name: " + Name + 
                        "][Position:" + Position.ToString() +
                        "][SpectralType: " + StarFactory.ESpectralTypeToString(SpectralType) + 
                        "][LuminosityType: " + StarFactory.ELuminosityTypeToString(LuminosityType) + 
                        "][SunMasses: " + MassSuns + 
                        "][SunRadiuses: " + RadiusSuns + 
                        "][SunLuminosities: " + LuminositySuns + 
                        "][LifetimeMyears: " + LifetimeMyears + 
                        "][HabitableZoneAU: " + HabitableZoneAU + 
                        "][TemperatureKelvin: " + TemperatureKelvin + "]";
        }

        
    }
}


