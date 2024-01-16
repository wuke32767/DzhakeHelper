using Microsoft.Xna.Framework;
using System;
using Monocle;

namespace Celeste.Mod.DzhakeHelper.Utils
{
    /// <summary>
    /// A class representing color as Hue, Saturation, and Value.
    /// </summary>
    public struct HSVColor
    {

        public float H;
        public float S;
        public float V;

        public HSVColor(float h = 0.0f, float s = .0f, float v = 0.0f)
        {
            H = h;
            S = s;
            V = v;
        }

        public HSVColor(HSVColor rvalue)
        {
            H = rvalue.H;
            S = rvalue.S;
            V = rvalue.V;
        }

        public HSVColor(Color inColor)
        {
            FromColor(inColor);
        }

        public void FromColor(Color ColorIn)
        {
            float r = ColorIn.R / 255f;
            float g = ColorIn.G / 255f;
            float b = ColorIn.B / 255f;
            float min, max, delta;
            min = Math.Min(Math.Min(r, g), b);
            max = Math.Max(Math.Max(r, g), b);
            V = max;
            delta = max - min;
            if (max != 0)
            {
                S = delta / max;
                if (r == max)
                    H = (g - b) / delta;
                else if (g == max)
                    H = 2 + (b - r) / delta;
                else
                    H = 4 + (r - g) / delta;
                H *= 60f;
                if (H < 0)
                    H += 360f;
            }
            else
            {
                S = 0f;
                H = 0f;
            }
            if (float.IsNaN(H))
                H = 0.0f;
            if (float.IsNaN(S))
                S = 0.0f;
            if (float.IsNaN(V))
                V = 0.0f;
        }

        public Color ToColor()
        {
            return Calc.HsvToColor(H, S, V);
        }
    }
}