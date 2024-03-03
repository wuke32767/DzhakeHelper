using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using static MonoMod.InlineRT.MonoModRule;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/CustomHiResDecal")]
    public class CustomHiResDecal : Entity
    {
        public enum SpriteBank
        {
            Game,Gui,Portraits,Misc
        };

        public string Path;
        public Vector2 Scale;
        public float Rotation;
        public Color Color;
        public SpriteBank SpriteFolder;

        private MTexture texture;

        private static float camScale = 6f;
        private static Vector2 entityOffset = new(12f, 12f);

        public CustomHiResDecal(EntityData data, Vector2 offset) : this(data.Position, offset, data.Attr("imagePath"), data.Int("depth"), new Vector2(data.Float("scaleX"), data.Float("scaleY")), data.Float("rotation"), data.HexColorWithAlpha("color"), data.Enum("pathStart", SpriteBank.Gui))
        {}
        public CustomHiResDecal(Vector2 position, Vector2 offset, string imagePath, int depth, Vector2 scale, float rotation,  Color color, SpriteBank pathStart) : base(position + offset)
        {
            base.Depth = depth;
            Path = imagePath;
            Scale = scale;
            Rotation = rotation;
            Color = color;
            SpriteFolder = pathStart;
        }
        public override void Added(Scene scene)
        {
            base.Added(scene);
            Tag = TagsExt.SubHUD;

            UpdateSprite();
        }
        public void UpdateSprite()
        {
            texture = SpriteFolder switch
            {
                SpriteBank.Game => GFX.Game[Path],
                SpriteBank.Gui => GFX.Gui[Path],
                SpriteBank.Portraits => GFX.Portraits[Path],
                SpriteBank.Misc => GFX.Misc[Path],
                _ => null,
            };
        }
        public override void Render()
        {
            base.Render();

            if (texture != null)
            {
                Camera cam = SceneAs<Level>().Camera;
                Vector2 displayPos = camScale * (this.Position - cam.Position) + camScale * entityOffset;
                texture.DrawCentered(displayPos, Color, Scale, Rotation / 57.2958f);
            }
        }

    }
}
