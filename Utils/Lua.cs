using MonoMod.Utils;
using NLua;
using System.Linq;

namespace Celeste.Mod.DzhakeHelper.Utils
{
    public static class Lua
    {
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
    }
}
