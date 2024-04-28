using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/Sequenceifer")]
    public class Sequenceifer : Entity
    {

        public int Index;
        public bool useCustomColor;
        public Color customColor;

        public Sequenceifer(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            Index = data.Int("index");
            useCustomColor = data.Bool("useCustomColor");
            customColor = data.HexColorWithAlpha("customColor");
        }

        public override void Awake(Scene scene)
        {
            foreach (Entity entity in scene.Entities)
            {
                if (entity is Sequenceifer) continue;

                if ()
                {
                    Logger.Log(LogLevel.Error,"dz","Sequenceifing " + entity.GetType().FullName);
                    entity.Add(new SequenceComponent(entity,Index,useCustomColor,customColor));
                }
            }


            base.Awake(scene);
        }
    }
}