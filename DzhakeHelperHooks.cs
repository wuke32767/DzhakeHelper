using Celeste.Mod.DzhakeHelper.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Collections;

namespace Celeste.Mod.DzhakeHelper
{
    public class DzhakeHelperHooks
    {


        public static void Load()
        {
            On.Celeste.LevelLoader.LoadingThread += CustomDashInitialize;
            On.Celeste.Player.DashBegin += CustomDashBegin;
            On.Celeste.Player.Die += PlayerDeath;
        }


        public static void Unload()
        {
            On.Celeste.LevelLoader.LoadingThread -= CustomDashInitialize;
            On.Celeste.Player.DashBegin -= CustomDashBegin;
            On.Celeste.Player.Die -= PlayerDeath;
        }

        private static void CustomDashInitialize(On.Celeste.LevelLoader.orig_LoadingThread orig, LevelLoader self)
        {
            ResetDashSession();
            orig(self);
        }   

        private static void CustomDashBegin(On.Celeste.Player.orig_DashBegin orig, Player self)
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


        private static PlayerDeadBody PlayerDeath(On.Celeste.Player.orig_Die orig, Player self, Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            ResetDashSession();
            return orig(self, direction, evenIfInvincible, registerDeathInStats);
        }


        private static void ResetDashSession()
        {
            if (DzhakeHelperModule.Session != null)
            {
                DzhakeHelperModule.Session.HasSequenceDash = false;
                DzhakeHelperModule.Session.HasPufferDash = false;
            }
        }


    }

}
