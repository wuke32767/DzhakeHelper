using System.Collections.Generic;
using System;

namespace Celeste.Mod.DzhakeHelper {
    public class DzhakeHelperModuleSession : EverestModuleSession {
        public bool HasSequenceDash { get; set; } = false;
        public bool HasPufferDash { get; set; } = false;

        public bool ShowSkipPart = false;

        public Dictionary<string, object> StoredVariables;

    }
}
