using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/SequenceBlockManager")]
    
    [Tracked]

    public class SequenceBlockManager : Entity
    {

        private int currentIndex;

        private int startWith;

        private int typesCount = -1;

        public bool everyDash;



        public SequenceBlockManager(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            startWith = data.Int("startWith");
            everyDash = data.Bool("everyDash");
        }

        public override void Awake(Scene scene)
        {

            base.Awake(scene);

            foreach (SequenceBlock entity in scene.Tracker.GetEntities<SequenceBlock>())
            {
                if (entity.Index > typesCount)
                {
                    typesCount = entity.Index;
                }
            }

            typesCount++;

            currentIndex = startWith;

            UpdateBlocks();

        }



        public override void Update()
        {
            base.Update();
        }


        public void UpdateBlocks()
        {
            foreach (SequenceBlock entity in base.Scene.Tracker.GetEntities<SequenceBlock>())
            {
                entity.Activated = entity.Index == currentIndex;
            }
        }

        public void CycleSequenceBlocks()
        {

            currentIndex++;
            currentIndex = currentIndex % typesCount;
            UpdateBlocks();

        }

        public void SetSequenceBlocks(int newIndex)
        {
            currentIndex = newIndex;
            UpdateBlocks();
        }

    }
}
