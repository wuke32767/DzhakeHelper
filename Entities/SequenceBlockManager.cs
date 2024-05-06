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

        public int CurrentColor { get { return DzhakeHelperModule.Session.ActiveSequenceIndex; } set { DzhakeHelperModule.Session.ActiveSequenceIndex = value; } }

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

            foreach (ManualSequenceComponent component in base.Scene.Tracker.GetComponents<ManualSequenceComponent>())
            {
                if (component.Index > typesCount)
                {
                    typesCount = component.Index;
                }
            }

            foreach (SequenceComponent component1 in base.Scene.Tracker.GetComponents<SequenceComponent>())
            {
                if (component1.Index > typesCount)
                {
                    typesCount = component1.Index;
                }
            }

            typesCount++;
            if (typesCount == 1) typesCount++; // 2 is minimum cuz why not

            CurrentColor = startWith;

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
                entity.Activated = entity.Index == CurrentColor;
            }

            foreach (ManualSequenceComponent component in base.Scene.Tracker.GetComponents<ManualSequenceComponent>())
            {
                component.Activated = component.Index == CurrentColor;
            }

            foreach (SequenceSwitchBlock switchBlock in base.Scene.Tracker.GetEntities<SequenceSwitchBlock>())
            {
                switchBlock.NextColor(CurrentColor, false);
            } 

            foreach (SequenceComponent component1 in base.Scene.Tracker.GetComponents<SequenceComponent>())
            {
                component1.Activated = component1.Index == CurrentColor;
            }

            for (int i = 0; i < 5; i++)
            {
                (Engine.Scene as Level)?.Session?.SetFlag($"DzhakeHelper_Sequence_{i}", false);
            }
            (Engine.Scene as Level)?.Session?.SetFlag($"DzhakeHelper_Sequence_{CurrentColor}", true);
        }

        public void CycleSequenceBlocks(int times = 1)
        {
            for (int i = 0;  i < times; i++)
            {
                CurrentColor++;
                CurrentColor = CurrentColor % typesCount;
            }
            // outside loop, cuz why do i need to update those each time?
            UpdateBlocks();
        }

        public void SetSequenceBlocks(int newIndex)
        {
            CurrentColor = newIndex;
            UpdateBlocks();
        }

    }
}
