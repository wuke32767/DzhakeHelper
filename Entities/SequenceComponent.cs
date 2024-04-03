using System;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper.Entities;

[Tracked(false)]
public class SequenceComponent : Component
{
    public Color color;
    public Color pressedColor;

    public int Index;
    public bool Activated = false;

    public bool UseCustomColor = false;

    public Entity entity;


    public SequenceComponent(EntityData data, Entity entity)
        : base(active: true, visible: false)
    {
        this.entity = entity;

        Index = data.Int("index");

        UseCustomColor = data.Bool("useCustomColor");
        if (UseCustomColor)
        {
            color = data.HexColorWithAlpha("color");
        }
        else
        {
            switch (Index)
            {
                default:
                    color = Calc.HexToColor("5c5bda");
                    break;
                case 1:
                    color = Calc.HexToColor("ff0051");
                    break;
                case 2:
                    color = Calc.HexToColor("ffd700");
                    break;
                case 3:
                    color = Calc.HexToColor("49dc88");
                    break;
            }
        }

        Color c = Calc.HexToColor("667da5");
        pressedColor = new Color((float)(int)c.R / 255f * ((float)(int)color.R / 255f), (float)(int)c.G / 255f * ((float)(int)color.G / 255f), (float)(int)c.B / 255f * ((float)(int)color.B / 255f), 1f);
    }




}
