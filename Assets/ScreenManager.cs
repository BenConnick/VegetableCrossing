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
        T screen = InstantiateScreen<T>();
        ScreenStack.Add(screen);
        screen.Canvas.sortingOrder = ScreenStack.Count;
    }

    public static void PopScreen()
    {
        BaseScreen top = ScreenStack[ScreenStack.Count - 1];
        ScreenStack.Remove(top);
        Object.Destroy(top.gameObject);
    }

    private static T InstantiateScreen<T>() where T : BaseScreen
    {
        FieldInfo field = typeof(ScreenPrefabData).GetField(typeof(T).Name);
        T prefab = (T)field.GetValue(AssetManager.Inst.ScreenPrefabs);
        return Object.Instantiate(prefab);  
    }

    public static bool HasScreen<T>()
    {
        foreach (var item in ScreenStack)
        {
            if (item is T)
            {
                return true;
            }
        }
        return false;
    }
}
