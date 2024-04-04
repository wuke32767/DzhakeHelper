using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/SequenceBlockManager")]
    
    [Tracked]

    public class SequenceBlockManager : Entity
    {
        private int startWith;

        public int typesCount = -1;

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
            if (typesCount == 1) typesCount++; // 2 is minimum cuz why not

            DzhakeHelperModule.Session.ActiveSequenceIndex = startWith;

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
                entity.Activated = entity.Index == DzhakeHelperModule.Session.ActiveSequenceIndex;
            }

            foreach (SequenceComponent component in base.Scene.Tracker.GetComponents<SequenceComponent>())
            {
                component.Activated = component.Index == DzhakeHelperModule.Session.ActiveSequenceIndex;
            }

            foreach (SequenceSwitchBlock switchBlock in base.Scene.Tracker.GetEntities<SequenceSwitchBlock>())
            {
                switchBlock.NextColor(DzhakeHelperModule.Session.ActiveSequenceIndex, false);
            } 
        }

        public void CycleSequenceBlocks(int times = 1)
        {
            for (int i = 0;  i < times; i++)
            {
                DzhakeHelperModule.Session.ActiveSequenceIndex++;
                DzhakeHelperModule.Session.ActiveSequenceIndex = DzhakeHelperModule.Session.ActiveSequenceIndex % typesCount;
            }
            // outside loop, cuz why do i need to update those each time?
            UpdateBlocks();
        }

        public void SetSequenceBlocks(int newIndex)
        {
            DzhakeHelperModule.Session.ActiveSequenceIndex = newIndex;
            UpdateBlocks();
        }

    }
}
