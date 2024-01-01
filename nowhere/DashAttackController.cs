using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/DashAttackController")]

    [Tracked]

    public class DashAttackController : Entity
    {

        public bool always;

        public DashAttackController(EntityData data, Vector2 offset) : base(data.Position + offset)
        {
            always = data.Bool("always");
        }
    }
}
