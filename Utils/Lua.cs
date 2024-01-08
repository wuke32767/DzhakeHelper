using MonoMod.Utils;
using NLua;
using System.Linq;
using System.Collections;
using Monocle;

namespace Celeste.Mod.DzhakeHelper.Utils
{
    public static class Lua
    {
        public static NLua.Lua State = new NLua.Lua();

        public static string PartSkipMessage = "";

        public static LuaTable CutsceneHelper = Everest.LuaLoader.Require($"{DzhakeHelperModule.Instance.Metadata.Name}:/Assets/LuaCutscenes/cutscene_helper") as LuaTable;

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
