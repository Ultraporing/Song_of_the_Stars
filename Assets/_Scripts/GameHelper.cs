using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Scripts
{
    public class GameHelper
    {
        private static Color Tint(Color source, Color tint, float alpha)
        {
            //(tint -source)*alpha + source
            var red = Convert.ToInt32(((tint.r - source.r) * alpha + source.r));
            var blue = Convert.ToInt32(((tint.b - source.b) * alpha + source.b));
            var green = Convert.ToInt32(((tint.g - source.b) * alpha + source.g));
            return new Color(red, green, blue, source.a);
        }

        public static Color[] GetPixelAlphaBlendPut(Color[] source, Color[] target, Color32 tint)
        {
            Color[] retCol = new Color[source.Length];

            for (int i = 0; i < retCol.Length; i++)
            {
                Color tC = Tint(source[i], new Color(tint.r/255, tint.g/255, tint.b/255), 1);
                retCol[i].a = source[i].a + target[i].a * (1-source[i].a);
                retCol[i].r = (tC.r * source[i].a + target[i].r * target[i].a * (1-source[i].a)) / retCol[i].a;
                retCol[i].g = (tC.g * source[i].a + target[i].g * target[i].a * (1 - source[i].a)) / retCol[i].a;
                retCol[i].b = (tC.b * source[i].a + target[i].b * target[i].a * (1 - source[i].a)) / retCol[i].a;
            }

            return retCol;
        }

        public static void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
        {
            var y = r;
            var d = 1 / 4 - r;
            var end = Mathf.Ceil(r / Mathf.Sqrt(2));

            for (int x = 0; x <= end; x++)
            {
                tex.SetPixel(cx + x, cy + y, col);
                tex.SetPixel(cx + x, cy - y, col);
                tex.SetPixel(cx - x, cy + y, col);
                tex.SetPixel(cx - x, cy - y, col);
                tex.SetPixel(cx + y, cy + x, col);
                tex.SetPixel(cx - y, cy + x, col);
                tex.SetPixel(cx + y, cy - x, col);
                tex.SetPixel(cx - y, cy - x, col);

                d += 2 * x + 1;
                if (d > 0)
                {
                    d += 2 - 2 * y--;
                }
            }
        }

        public static void getRGBfromTemperature(ref uint r, ref uint g, ref uint b, uint tmpKelvin)
        {
            double tmpCalc;

            // Brown dwarf
            if (tmpKelvin <= 2550)
            {
                r = 255;
                g = 0;
                b = 255;
                return;
            }

            //Temperature must fall between 1000 and 40000 degrees
            if (tmpKelvin > 40000) tmpKelvin = 40000;

            //All calculations require tmpKelvin \ 100, so only do the conversion once
            tmpKelvin /= 100;

            //Calculate each color in turn

            //First: red
            if (tmpKelvin <= 66)
                r = 255;
            else
            {
                //Note: the R-squared value for this approximation is .988
                tmpCalc = tmpKelvin - 60;
                tmpCalc = 329.698727446 * Math.Pow(tmpCalc, -0.1332047592);
                r = (uint)tmpCalc;
                if (r > 255) r = 255;
            }

            //Second: green
            if (tmpKelvin <= 66)
            {
                //Note: the R-squared value for this approximation is .996
                tmpCalc = tmpKelvin;
                tmpCalc = 99.4708025861 * Math.Log(tmpCalc) - 161.1195681661;
                g = (uint)tmpCalc;
                if (g > 255) g = 255;
            }
            else
            {
                //Note: the R-squared value for this approximation is .987
                tmpCalc = tmpKelvin - 60;
                tmpCalc = 288.1221695283 * Math.Pow(tmpCalc, -0.0755148492);
                g = (uint)tmpCalc;
                if (g > 255) g = 255;
            }

            //Third: blue
            if (tmpKelvin >= 66)
                b = 255;
            else if (tmpKelvin <= 19)
                b = 0;
            else
            {
                //Note: the R-squared value for this approximation is .998
                tmpCalc = tmpKelvin - 10;
                tmpCalc = 138.5177312231 * Math.Log(tmpCalc) - 305.0447927307;

                b = (uint)tmpCalc;
                if (b > 255) b = 255;
            }
        }
    }
}
