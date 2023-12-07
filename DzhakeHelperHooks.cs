using Celeste.Mod.DzhakeHelper.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using static Celeste.GaussianBlur;

namespace Celeste.Mod.DzhakeHelper
{
    public class DzhakeHelperHooks
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
            DzhakeResetDashSession();
            orig(self);
        }   

        private static void DzhakeCustomDashBegin(On.Celeste.Player.orig_DashBegin orig, Player self)
        {
            bool callOrig = true;
            
            SequenceBlockManager manager = self.Scene.Tracker.GetEntity<SequenceBlockManager>();
            if (manager != null)
            {
                if (DzhakeHelperModule.Session.HasSequenceDash && manager != null)
                {
                    DzhakeHelperModule.Session.HasSequenceDash = false;
                    manager.CycleSequenceBlocks();
                }
                if (manager != null && manager.everyDash)
                {
                    manager.CycleSequenceBlocks();
                }
            }
            /*if (DzhakeHelperModule.Session.HasPufferDash)
            {
                Vector2 launch = self.ExplodeLaunch(self.Center - self.DashDir, false, false);
                self.Dashes--;
                Logger.Log(LogLevel.Error,"DzhakeHelper/DashHook", "X: " + launch.X.ToString() + " Y: " + launch.Y.ToString());
                Logger.Log(LogLevel.Error, "DzhakeHelper/DashHook", "X: " + self.DashDir.X.ToString() + " Y: " + self.DashDir.Y.ToString());
                Logger.Log(LogLevel.Error, "DzhakeHelper/DashHook", "new line");
                self.SceneAs<Level>().Displacement.AddBurst(self.Center, 0.3f, 8f, 32f, 0.8f, null, null);
                self.SceneAs<Level>().DirectionalShake(launch, 0.15f);
                DzhakeHelperModule.Session.HasPufferDash = false;
                callOrig = false;
            }*/

            if (callOrig)
            {
                orig(self);
            }
        }

        private static PlayerDeadBody DzhakeCustomDashDeath(On.Celeste.Player.orig_Die orig, Player self, Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            DzhakeResetDashSession();
            return orig(self, direction, evenIfInvincible, registerDeathInStats);
        }


        private static void DzhakeResetDashSession()
        {
            DzhakeHelperModule.Session.HasSequenceDash = false;
            DzhakeHelperModule.Session.HasPufferDash = false;
        }

    }

}
