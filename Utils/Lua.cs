using MonoMod.Utils;
using NLua;
using System.Linq;
using Monocle;
using System.Collections.Generic;
using System.Collections;

namespace Celeste.Mod.DzhakeHelper.Utils
{
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


        // FIND HIM!

        public static Entity FindEntity(string type)
        {
            foreach (Entity entity in Engine.Scene.Entities)
            {
                if (entity.GetType().FullName == type)
                {
                    return entity;
                }
            }
            return null;
        }

        public static List<Entity> FindEntities(string type)
        {
            List<Entity> entities = [];
            foreach (Entity entity in Engine.Scene.Entities)
            {
                if (entity.GetType().FullName == type)
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }


        // https://github.com/Cruor/LuaCutscenes/blob/master/Helpers/LuaHelper.cs


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
