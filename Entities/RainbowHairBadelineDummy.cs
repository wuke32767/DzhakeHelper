using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/RainbowHairBadelineDummy")]
    public class RainbowHairBadelineDummy : BadelineDummy
    {
        public float ShiftSpeed;

        public RainbowHairBadelineDummy(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            ShiftSpeed = data.Float("shiftSpeed",100f);
        }

        public override void Update()
        {
            base.Update();
            Hair.Color = Hair.Color.ShiftHue(Engine.DeltaTime * ShiftSpeed);
        }

    }
}
