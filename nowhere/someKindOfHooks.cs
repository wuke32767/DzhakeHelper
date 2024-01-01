/*using Celeste.Mod.DzhakeHelper.Entities;
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

        private static ILHook hook_player_updateSprite;
        private static ILHook hook_player_update;
        private static ILHook hook_player_normalUpdate;
        private static ILHook hook_player_dreamDashCheck;
        private static ILHook hook_player_onCollideH;

        public static void Load()
        {
            On.Celeste.LevelLoader.LoadingThread += CustomDashInitialize;
            On.Celeste.Player.DashBegin += CustomDashBegin;
            On.Celeste.Player.DashEnd += CustomDashEnd;
            On.Celeste.Player.Die += PlayerDeath;
            On.Celeste.Player.Update += PlayerUpdate;
            hook_player_updateSprite = new ILHook(typeof(Player).GetMethod("orig_UpdateSprite", BindingFlags.NonPublic | BindingFlags.Instance), ILPlayerUpdateSprite);
            hook_player_update = new ILHook(typeof(Player).GetMethod("orig_Update"), ILPlayerUpdate);
            hook_player_normalUpdate = new ILHook(typeof(Player).GetMethod("NormalUpdate", BindingFlags.NonPublic | BindingFlags.Instance), ILPlayerNormalUpdate);
            hook_player_dreamDashCheck = new ILHook(typeof(Player).GetMethod("DreamDashCheck", BindingFlags.NonPublic | BindingFlags.Instance), ILPlayerDreamDashCheck);
            hook_player_onCollideH = new ILHook(typeof(Player).GetMethod("DreamDashCheck", BindingFlags.NonPublic | BindingFlags.Instance), ILPlayerOnCollideH);
        }

        public static void Unload()
        {
            On.Celeste.LevelLoader.LoadingThread -= CustomDashInitialize;
            On.Celeste.Player.DashBegin -= CustomDashBegin;
            On.Celeste.Player.DashEnd -= CustomDashEnd;
            On.Celeste.Player.Die -= PlayerDeath;
            On.Celeste.Player.Update -= PlayerUpdate;
            hook_player_updateSprite?.Dispose();
            hook_player_update?.Dispose();
            hook_player_normalUpdate?.Dispose();
            hook_player_dreamDashCheck?.Dispose();
            hook_player_onCollideH?.Dispose();
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


        private static void ILPlayerUpdateSprite(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchCallvirt<Player>("get_DashAttacking")))
            {
                cursor.EmitDelegate(isDashing);
                cursor.Emit(OpCodes.And);
            }
        }

        private static void ILPlayerUpdate(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchCallvirt<Player>("get_DashAttacking")))
            {
                cursor.EmitDelegate(isDashing);
                cursor.Emit(OpCodes.And);
            }
        }

        private static void ILPlayerNormalUpdate(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchCallvirt<Player>("get_DashAttacking")))
            {
                cursor.EmitDelegate(isDashing);
                cursor.Emit(OpCodes.And);
            }
        }

        private static void ILPlayerDreamDashCheck(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdflda<Player>("DashDir") && instr.MatchLdfld<Vector2>("Y")) && cursor.TryGotoNext(MoveType.After, instr => instr.MatchBneUn(out ILLabel _)))
            {
                cursor.EmitDelegate(isDashing);
                cursor.Emit(OpCodes.Not);
                cursor.Emit(OpCodes.Or);
            }
        }
        private static void ILPlayerOnCollideH(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdfld<CollisionData>("Direction") && instr.MatchLdfld<Vector2>("X")) && cursor.TryGotoNext(MoveType.After, instr => instr.MatchConvR4()))
            {
                cursor.EmitDelegate(isDashing);
                cursor.Emit(OpCodes.Not);
                cursor.Emit(OpCodes.Or);
            }
        }

        private static bool isDashing()
        {
            return DzhakeHelperModule.Session.Dashing;
        }

    }

}
*/