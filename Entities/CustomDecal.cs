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

        public CustomDecal(EntityData data, Vector2 offset) : this(data.Position, offset, data.Attr("imagePath"), data.Int("depth"), data.HexColorWithAlpha("color"), new Vector2(data.Float("scaleX"), data.Float("scaleY")), data.Float("rotation"), data.Bool("animated"), data.Attr("flag"), data.Bool("updateSpriteOnlyIfFlag"), data.Bool("inversedFlag"), data.Attr("animationName"), data.Float("delay"))
        {}

        public CustomDecal(Vector2 position,Vector2 offset, string imagePath, int depth, Color color, Vector2 scale, float rotation, bool animated, string flag, bool updateSpriteOnlyIfFlag, bool inversedFlag, string animationName, float delay) : base(position + offset)
        {
            base.Depth = depth;

            Flag = flag;
            UpdateSpriteOnlyIfFlag = updateSpriteOnlyIfFlag;
            InversedFlag = inversedFlag;

            ImagePath = imagePath;
            Animated = animated;
            AnimationName = animationName;
            Delay = delay;

            Color = color;
            Scale = scale;
            Rotation = rotation;

            UpdateSprite();
        }

        public override void Render()
        {
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
