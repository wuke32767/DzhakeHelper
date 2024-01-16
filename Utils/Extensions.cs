using Celeste.Mod.DzhakeHelper.Utils;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DzhakeHelper;

public static class Extensions
{
    public static Color Mult(this Color color, Color other)
    {
        color.R = (byte)(color.R * other.R / 256f);
        color.G = (byte)(color.G * other.G / 256f);
        color.B = (byte)(color.B * other.B / 256f);
        color.A = (byte)(color.A * other.A / 256f);
        return color;
    }

    public static Color ShiftHue(this Color color, float amount)
    {
        HSVColor hsvColor = new(color);
        hsvColor.H = (hsvColor.H + amount) % 360f;
        return Calc.HsvToColor(hsvColor.H / 360, hsvColor.S, hsvColor.V); ;
    }

    public static Rectangle GetBounds(this Camera camera)
    {
        int top = (int)camera.Top;
        int bottom = (int)camera.Bottom;
        int left = (int)camera.Left;
        int right = (int)camera.Right;

        return new(left, top, right - left, bottom - top);
    }


}