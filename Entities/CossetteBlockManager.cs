using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/CossetteBlockManager")]
    
    [Tracked]

    public class CossetteBlockManager : Entity
    {

        private int currentIndex;

        private int startWith;

        private int typesCount = -1;

        public bool everyDash;



        public CossetteBlockManager(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            startWith = data.Int("startWith");
            everyDash = data.Bool("everyDash");
            if (everyDash )
            {
                DzhakeHelperModule.Session.HasCossetteDash = true;
            }
        }

        public override void Awake(Scene scene)
        {

            base.Awake(scene);

            foreach (CossetteBlock entity in scene.Tracker.GetEntities<CossetteBlock>())
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
            foreach (CossetteBlock entity in base.Scene.Tracker.GetEntities<CossetteBlock>())
            {
                entity.Activated = entity.Index == currentIndex;
            }
        }

        public void CycleCossetteBlocks()
        {

            currentIndex++;
            currentIndex = currentIndex % typesCount;
            UpdateBlocks();

        }

        public void SetCossetteBlocks(int newIndex)
        {
            currentIndex = newIndex;
            UpdateBlocks();
        }

    }
}
