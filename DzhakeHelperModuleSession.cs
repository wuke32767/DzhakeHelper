using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace Celeste.Mod.DzhakeHelper {
    public class DzhakeHelperModuleSession : EverestModuleSession {
        public bool HasSequenceDash { get; set; } = false;
        public bool HasPufferDash { get; set; } = false;

        public Dictionary<string, object> StoredVariables;

        public float TimedKillTriggerTime { get; set; } = 0f;

        public bool TimedKillTriggerTimeChanged { get; set; } = false;

        public float TimedKillTriggerMaxTime { get; set; } = 0f;

        public Color TimedKillTriggerColor { get; set; } = Color.White;

    }
}
