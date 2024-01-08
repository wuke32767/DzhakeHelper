using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

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
        public float ScaleX;
        public float ScaleY;
        public float Rotation;
        public Color Color;
        public SpriteBank SpriteFolder;

        private MTexture texture;

        private static float camScale = 6f;
        private static Vector2 entityOffset = new(12f, 12f);

        public CustomHiResDecal(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            base.Depth = data.Int("depth");
            Path = data.Attr("imagePath");
            ScaleX = data.Float("scaleX");
            ScaleY = data.Float("scaleY");
            Rotation = data.Float("rotation");
            Color = data.HexColor("color");
            SpriteFolder = data.Enum("pathStart", SpriteBank.Gui);
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
                texture.DrawCentered(displayPos, Color, new Vector2(ScaleX, ScaleY), Rotation / 57.2958f);
            }
        }

    }
}
