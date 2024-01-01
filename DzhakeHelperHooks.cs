using Celeste.Mod.DzhakeHelper.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Monocle;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Celeste.Mod.DzhakeHelper
{
    public class DzhakeHelperHooks
    {


        public static void Load()
        {
            On.Celeste.LevelLoader.LoadingThread += CustomDashInitialize;
            On.Celeste.Player.DashBegin += CustomDashBegin;
            On.Celeste.Player.DashEnd += CustomDashEnd;
            On.Celeste.Player.Die += PlayerDeath;
            On.Celeste.Player.Update += PlayerUpdate;
        }

        public static void Unload()
        {
            On.Celeste.LevelLoader.LoadingThread -= CustomDashInitialize;
            On.Celeste.Player.DashBegin -= CustomDashBegin;
            On.Celeste.Player.DashEnd -= CustomDashEnd; 
            On.Celeste.Player.Die -= PlayerDeath;
            On.Celeste.Player.Update -= PlayerUpdate;
        }

        private static void CustomDashInitialize(On.Celeste.LevelLoader.orig_LoadingThread orig, LevelLoader self)
        {
            ResetDashSession();
            orig(self);
        }   

        private static void CustomDashBegin(On.Celeste.Player.orig_DashBegin orig, Player self)
        {
            bool callOrig = true;

            DzhakeHelperModule.Session.Dashing = true;

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

        private static void CustomDashEnd(On.Celeste.Player.orig_DashEnd orig, Player self)
        {
            DzhakeHelperModule.Session.Dashing = false;
        }

        private static void PlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {
            DashAttackController dashAttackController = self.Scene.Tracker.GetEntity<DashAttackController>();
            if (dashAttackController != null)
            {
                DynamicData playerData = DynamicData.For(self);
                float dashAttackTimer = playerData.Get<float>("dashAttackTimer");
                if (dashAttackController != null && dashAttackController.always)
                {
                    playerData.Set("dashAttackTimer", dashAttackTimer + Engine.DeltaTime + 1f);
                }
            }

            orig(self);
        }

        private static PlayerDeadBody PlayerDeath(On.Celeste.Player.orig_Die orig, Player self, Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            ResetDashSession();
            return orig(self, direction, evenIfInvincible, registerDeathInStats);
        }


        private static void ResetDashSession()
        {
            DzhakeHelperModule.Session.HasSequenceDash = false;
            DzhakeHelperModule.Session.HasPufferDash = false;
        }

    }

}
