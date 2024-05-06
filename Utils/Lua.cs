using MonoMod.Utils;
using NLua;
using System.Linq;
using Monocle;
using System.Collections.Generic;
using System.Collections;
using System;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.DzhakeHelper.Utils
{
    /// <summary>
    /// Class, which you can easily use from lua, but technically can be used from c# too
    /// </summary>
    public static class Lua
    {

        // Privating
        public static object GetPrivateMember(object obj, string name)
        {
            DynamicData objData = DynamicData.For(obj);
            return objData.Get<object>(name);
        }
        public static void SetPrivateMember(object obj, string name, object newValue)
        {
            DynamicData objData = DynamicData.For(obj);
            objData.Set(name, newValue);
        }
        public static object CallMethod(object obj, string name, LuaTable args)
        {
            object[] argArray = args.Values.Cast<object>().ToArray();
            DynamicData objData = DynamicData.For(obj);
            return objData.Invoke(name, argArray);
        }
        public static object CallMethod(object obj, string name)
        {
            DynamicData objData = DynamicData.For(obj);
            return objData.Invoke(name);
        }


        // Storing
        public static void StoreVariable(object obj, string name) 
        {
            DzhakeHelperModule.Session.StoredVariables.Add(name,obj);
        }
        public static object GetStoredVariable(string name)
        {
            DzhakeHelperModule.Session.StoredVariables.TryGetValue(name, out object value);
            return value;
        }
        public static void RemoveStoredVariable(string name)
        {
            DzhakeHelperModule.Session.StoredVariables.Remove(name);   
        }

        //EntityData generation, which doesn't work, lmao, fuck lua
        /*public static EntityData GenerateEntityData(LuaTable data)
        {
            Vector2 offset = Util.GetLevel().LevelOffset;
            EntityData entityData = new EntityData();

            //these really, REALLY should exist (but not for controllers)
            if (data["_x"] != null) entityData.Position.X = (float)data["_x"];
            if (data["_y"] != null)  entityData.Position.Y = (float)data["_y"];

            //these may exist
            if (data["_name"] != null) entityData.Name = (string)data["_name"];
            if (data["_width"] != null) entityData.Width = (int)data["_width"];
            if (data["_height"] != null) entityData.Height = (int)data["_height"];
            if (data["_nodes"] != null)
            {
                entityData.Nodes = LuaNodesToVector2Array((LuaTable)data["_nodes"]);
            }
            if (data["_id"] != null)
            {
                entityData.ID = (int)data["_id"];
            }
            else
            {
                entityData.ID = Util.random.Next(500, 9999);
            }

            //these I set myself
            entityData.Level = Util.GetLevel().Session.LevelData;

            //these are everything else
            foreach (KeyValuePair<string,object> kvp in data)
            {
                if (kvp.Key == "_x" || kvp.Key == "_y" || kvp.Key == "_id" || kvp.Key == "_name" || kvp.Key == "_nodes" || kvp.Key == "_width" || kvp.Key == "_width") continue;
                entityData.Values[kvp.Key] = kvp.Value;
            }

            return entityData;
        }*/


        // https://github.com/Cruor/LuaCutscenes/blob/master/Helpers/LuaHelper.cs

        #region Conversions
        public static LuaTable DictionaryToLuaTable(IDictionary<object, object> dict)
        {
            NLua.Lua lua = Everest.LuaLoader.Context;
            LuaTable table = lua.DoString("return {}").FirstOrDefault() as LuaTable;

            foreach (KeyValuePair<object, object> pair in dict)
            {
                table[pair.Key] = pair.Value;
            }

            return table;
        }

        public static LuaTable ListToLuaTable(IList list)
        {
            NLua.Lua lua = Everest.LuaLoader.Context;
            LuaTable table = lua.DoString("return {}").FirstOrDefault() as LuaTable;

            int ptr = 1;

            foreach (var value in list)
            {
                table[ptr++] = value;
            }


            return table;
        }

        public static Vector2[] LuaNodesToVector2Array(LuaTable table)
        {
            Vector2[] array = new Vector2[table.Keys.Count];
            int i = 0;
            foreach (Vector2 value in table.Values)
            {
                array[i] = value;
                i++;
            }
            return array;
        }
        #endregion


        // just some random stuff lol

        public static IEnumerator CustomWalkTo(Player player, float x, bool walkBackwards = false, float speedMultiplier = 1f, bool keepWalkingIntoWalls = false, bool changeSpriteAfter = true)
        {
            player.StateMachine.State = 11;
            if (Math.Abs(player.X - x) > 4f && !player.Dead)
            {
                player.DummyMoving = true;
                if (walkBackwards)
                {
                    player.Sprite.Rate = -1f;
                    player.Facing = (Facings)Math.Sign(player.X - x);
                }
                else
                {
                    player.Facing = (Facings)Math.Sign(x - player.X);
                }

                while (Math.Abs(x - player.X) > 4f && Engine.Scene != null && (keepWalkingIntoWalls || !player.CollideCheck<Solid>(player.Position + Vector2.UnitX * Math.Sign(x - player.X))))
                {
                    player.Speed.X = Calc.Approach(player.Speed.X, (float)Math.Sign(x - player.X) * 64f * speedMultiplier, 1000f * Engine.DeltaTime);
                    yield return null;
                }

                if (changeSpriteAfter)
                {
                    player.Sprite.Rate = 1f;
                    player.Sprite.Play("idle");
                }
                player.DummyMoving = false;
            }
        }




        // Skipping (Probably not going to make)

        /*public static void SkipPart()
        {
            foreach (Entity entity in (Engine.Scene as Level).Entities)
            {
                if (entity.GetType() == typeof(LuaCutsceneEntity)) // entity is LuaCutsceneEntity!
                {
                    Logger.Log(LogLevel.Error, "DzhakeHerlper (anyfhow", "whaaaaaa");
                    (entity as LuaCutsceneEntity).onBeginRoutine.Reset();  //  Doesn't work because... publicizer issue? not sure. Would be bad anyway, LuaCutscenes PR is better, but I didn't manage to make it...
                }
            }
        }

        public static void TogglePartSkip()
        {
            DzhakeHelperModule.Session.ShowSkipPart = !DzhakeHelperModule.Session.ShowSkipPart;
        }
        public static void SetPartSkip(bool to)
        {
            DzhakeHelperModule.Session.ShowSkipPart = to;
        }*/
    }
}
