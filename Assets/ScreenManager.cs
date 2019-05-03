using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ScreenManager
{
    private static readonly List<BaseScreen> ScreenStack = new List<BaseScreen>();

    public static void AddStartScreen(StartScreen startScreen)
    {
        ScreenStack.Add(startScreen);
    }

    public static void PushScreen<T>() where T : BaseScreen
    {
        // TODO 
    }

    public static void PopScreen()
    {
        // TODO
    }

    private static T InstantiateScreen<T>() where T : BaseScreen
    {
        FieldInfo field = typeof(ScreenPrefabData).GetField(typeof(T).Name);
        T prefab = (T)field.GetValue(AssetManager.Inst.ScreenPrefabs);
        return Object.Instantiate(prefab);
    }
}
