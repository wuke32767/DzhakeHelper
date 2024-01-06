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

        public string AnimationID;

        public CustomDecal(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            Depth = data.Int("Depth");
            Logger.Log(LogLevel.Error, "DSKDlslada", Depth.ToString());
            ImagePath = data.Attr("imagePath");
            Animated = data.Bool("animated");
            if (Animated)
            {
                Add(Sprite = new Sprite(GFX.Game,ImagePath));
                Sprite.Color = Color;
                Sprite.Scale = Scale;
                Sprite.Rotation = Rotation;
            }
            else
            {
                Image = GFX.Game[ImagePath];
            }
            Scale.X = data.Float("scaleX");
            Scale.Y = data.Float("scaleY");
            Color = data.HexColor("color");
            Rotation = data.Float("rotation");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render()
        {
            if (Animated)
            {
                Sprite.Play(AnimationID);
            }
            else
            {
                Image.DrawCentered(Position, Color, Scale, Rotation);
            }
            base.Render();
        }

        public void UpdateSprite()
        { 
            if (Animated && Sprite != null)
            {
                Sprite.Color = Color;
                Sprite.Scale = Scale;
                Sprite.Rotation = Rotation;
                Sprite.Play(AnimationID);
            } else
            {
                Image = GFX.Game[ImagePath];
            }
        }
    }
}
