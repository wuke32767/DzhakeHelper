using Celeste.Mod.DzhakeHelper.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper
{
    public static class DzhakeHelperHooks
    {

        public static void Load()
        {
            On.Celeste.LevelLoader.LoadingThread += DzhakeCustomDashInitialize;
            On.Celeste.Player.DashBegin += DzhakeCustomDashBegin;
            On.Celeste.Player.Die += DzhakeCustomDashDeath;
        }

        public static void Unload()
        {
            On.Celeste.LevelLoader.LoadingThread -= DzhakeCustomDashInitialize;
            On.Celeste.Player.DashBegin -= DzhakeCustomDashBegin;
            On.Celeste.Player.Die -= DzhakeCustomDashDeath;
        }

        private static void DzhakeCustomDashInitialize(On.Celeste.LevelLoader.orig_LoadingThread orig, LevelLoader self)
        {
            orig(self);
            DzhakeResetDashSession();
        }

        private static void DzhakeCustomDashBegin(On.Celeste.Player.orig_DashBegin orig, Player self)
        {
            CossetteBlockManager manager = self.Scene.Tracker.GetEntity<CossetteBlockManager>();
            if (manager != null)
            {
                if (DzhakeHelperModule.Session.HasCossetteDash)
                {
                    DzhakeHelperModule.Session.HasCossetteDash = false;
                    manager.CycleCossetteBlocks();
                }
                if (manager != null && manager.everyDash)
                {
                    manager.CycleCossetteBlocks();
                }
            }

            orig(self);

        }

        private static PlayerDeadBody DzhakeCustomDashDeath(On.Celeste.Player.orig_Die orig, Player self, Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            DzhakeResetDashSession();
            return orig(self, direction, evenIfInvincible, registerDeathInStats);
        }


        private static void DzhakeResetDashSession()
        {
            DzhakeHelperModule.Session.HasCossetteDash = false;
        }


    }

}
