using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/CustomDecal")]
    internal class CustomDecal : Entity
    {
        public string ImagePath;

        public MTexture Image;

        public Color Color;

        public Vector2 Scale;

        public float Rotation;

        public Sprite Sprite;

        public bool Animated;

        public string Flag;

        public bool UpdateSpriteOnlyIfFlag;

        public bool InversedFlag;

        public string AnimationName;

        public float Delay;

        public CustomDecal(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            base.Depth = data.Int("depth");
            Flag = data.Attr("flag");
            UpdateSpriteOnlyIfFlag = data.Bool("updateSpriteOnlyIfFlag");
            InversedFlag = data.Bool("inversedFlag");
            ImagePath = data.Attr("imagePath");
            Animated = data.Bool("animated");
            AnimationName = data.Attr("animationName");
            Delay = data.Float("delay");
            Scale.X = data.Float("scaleX");
            Scale.Y = data.Float("scaleY");
            Color = Calc.HexToColorWithAlpha((string)data.Values["color"]);
            Rotation = data.Float("rotation");
            UpdateSprite();
        }

        public override void Render()
        {
            Logger.Log(LogLevel.Error, "DhzkaeHelper/customDecals",Flag);
            if (FlagIsTrue())
            {
                if (!Animated && Image != null)
                {
                    Image.DrawCentered(Position, Color, Scale, Rotation);
                }
                base.Render();
            }
        }

        public override void Update()
        {
            if (Animated && Sprite != null && UpdateSpriteOnlyIfFlag)
            {
                if (FlagIsTrue())
                {
                    Sprite.Rate = 1f;
                }
                else
                {
                    Sprite.Rate = 0f;
                }
            }
            base.Update();
        }

        public void UpdateSprite()
        {
            if (Sprite != null)
            {
                Remove(Sprite);
            }
            if (Animated)
            {
                Sprite = new Sprite(GFX.Game, ImagePath);
                Add(Sprite);
                Sprite.Color = Color;
                Sprite.Scale = Scale;
                Sprite.Rotation = Rotation;
                Sprite.AddLoop("normal",AnimationName,Delay);
                Sprite.Play("normal");
            }
            else
            {
                Image = GFX.Game[ImagePath];
            }

        }

        public bool FlagIsTrue()
        {
            return Flag == "" || ((base.Scene as Level).Session.GetFlag(Flag) != InversedFlag);
        }
    }
}
