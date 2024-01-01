using Microsoft.Xna.Framework;
using Monocle;
using Celeste.Mod.Entities;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/BackgroundWall")]
    public class BackgroundWall : Entity
    {

        private bool blend;

        private char fillTile;

        private TileGrid tiles;


        public BackgroundWall(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            blend = data.Bool("blend");
            fillTile = data.Char("tiletype", '3');
            base.Collider = new Hitbox(data.Width, data.Height);
            base.Depth = data.Int("depth");
        }

        public override void Added(Scene scene)
        {
            base.Added(scene);
            int tilesX = (int)base.Width / 8;
            int tilesY = (int)base.Height / 8;
            if (blend)
            {
                Level level = SceneAs<Level>();
                Rectangle tileBounds = level.Session.MapData.TileBounds;
                VirtualMap<char> solidsData = level.SolidsData;
                int x = (int)base.X / 8 - tileBounds.Left;
                int y = (int)base.Y / 8 - tileBounds.Top;
                tiles = GFX.BGAutotiler.GenerateOverlay(fillTile, x, y, tilesX, tilesY, solidsData).TileGrid;
            }
            else
            {
                tiles = GFX.BGAutotiler.GenerateBox(fillTile, tilesX, tilesY).TileGrid;
            }
            Add(tiles);
            Add(new TileInterceptor(tiles, highPriority: false));
        }

        public override void Render()
        {
            if (blend)
            {
                Level level = base.Scene as Level;
                if (level.ShakeVector.X < 0f && level.Camera.X <= (float)level.Bounds.Left && base.X <= (float)level.Bounds.Left)
                {
                    tiles.RenderAt(Position + new Vector2(-3f, 0f));
                }
                if (level.ShakeVector.X > 0f && level.Camera.X + 320f >= (float)level.Bounds.Right && base.X + base.Width >= (float)level.Bounds.Right)
                {
                    tiles.RenderAt(Position + new Vector2(3f, 0f));
                }
                if (level.ShakeVector.Y < 0f && level.Camera.Y <= (float)level.Bounds.Top && base.Y <= (float)level.Bounds.Top)
                {
                    tiles.RenderAt(Position + new Vector2(0f, -3f));
                }
                if (level.ShakeVector.Y > 0f && level.Camera.Y + 180f >= (float)level.Bounds.Bottom && base.Y + base.Height >= (float)level.Bounds.Bottom)
                {
                    tiles.RenderAt(Position + new Vector2(0f, 3f));
                }
            }
            base.Render();
        }
    }
}

