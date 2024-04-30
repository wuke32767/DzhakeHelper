using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [Obsolete($"Use {nameof(CustomDecal)} instead.")]
    public class ObsoleteCustomDecal : Entity
    {

        // -_-
        private static float camScale = 6f;
        private static Vector2 entityOffset = new(12f, 12f);

        public enum SpriteBank
        {
            Game, Gui, Portraits, Misc
        };
        public SpriteBank PathRoot;

        public string ImagePath;

        public Sprite Sprite;

        public bool Animated;

        public string Flag;

        public bool UpdateSpriteOnlyIfFlag;

        public bool InversedFlag;

        public string AnimationName;
        public string fullPath;

        public float Delay;

        public Vector2 shakeOffset;

        public bool HD;

        public float Rotation;
        public Color Color;
        public Vector2 Scale;

        public MTexture Image;

        public ObsoleteCustomDecal(EntityData data, Vector2 offset) : this(data.Position, offset, data.Attr("imagePath"), data.Int("depth"), data.HexColorWithAlpha("color"), new Vector2(data.Float("scaleX"),
            data.Float("scaleY")), data.Float("rotation"), data.Bool("animated"), data.Attr("flag"), data.Bool("updateSpriteOnlyIfFlag"), data.Bool("inversedFlag"), data.Attr("animationName"),
            data.Float("delay"), data.Bool("attached"), data.Bool("hiRes"), data.Enum("pathRoot", SpriteBank.Game))
        { }

        public ObsoleteCustomDecal(Vector2 position, Vector2 offset, string imagePath, int depth, Color color, Vector2 scale, float rotation, bool animated, string flag, bool updateSpriteOnlyIfFlag, bool inversedFlag,
            string animationName, float delay, bool attached, bool hd, SpriteBank PathRoot) : base(position + offset)
        {
            base.Depth = depth;

            this.PathRoot = PathRoot;

            HD = hd;
            if (HD)
            {
                Tag = TagsExt.SubHUD;
            }

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

            fullPath = Animated ? ImagePath + AnimationName : ImagePath;


            if (attached)
            {
                Add(new StaticMover
                {
                    OnShake = OnShake,
                    OnEnable = OnEnable,
                    OnDisable = OnDisable
                });
            }

            UpdateSprite();
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

        public override void Render()
        {
            if (FlagIsTrue())
            {
                Vector2 displayPos = new Vector2(0, 0);
                if (HD)
                {
                    Camera cam = SceneAs<Level>().Camera;
                    displayPos = camScale * (this.Position - cam.Position) + camScale * entityOffset;
                }



                if (!Animated && Image != null)
                {
                    Image.DrawCentered(displayPos, Color, Scale, Rotation / 57.2958f);
                }
                Position += displayPos;
                base.Render();
                Position -= displayPos;
            }
        }

        public void UpdateSprite()
        {
            Atlas atlas = PathRoot switch
            {
                SpriteBank.Game => GFX.Game,
                SpriteBank.Gui => GFX.Gui,
                SpriteBank.Portraits => GFX.Portraits,
                SpriteBank.Misc => GFX.Misc,
                _ => GFX.Game,
            };

            if (Sprite != null)
            {
                Remove(Sprite);
            }
            if (Animated)
            {
                Sprite = new Sprite(atlas, ImagePath);
                Add(Sprite);
                Sprite.Color = Color;
                Sprite.Scale = Scale;
                Sprite.Rotation = Rotation;
                Sprite.AddLoop("normal", AnimationName, Delay);
                Sprite.Play("normal");
            }
            else
            {
                Image = atlas[ImagePath];
            }
        }

        public bool FlagIsTrue()
        {
            return Flag == "" || ((base.Scene as Level).Session.GetFlag(Flag) != InversedFlag);
        }


        private void OnShake(Vector2 amount)
        {
            shakeOffset += amount;
        }

        private void OnEnable()
        {
            Active = (Visible = (Collidable = true));
        }

        private void OnDisable()
        {
            Active = Visible = Collidable = false;
        }
    }
}
