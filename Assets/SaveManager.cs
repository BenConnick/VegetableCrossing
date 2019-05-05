using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    private const string FarmsKey = "FarmsKey";
    private const string FarmPlantsKey = "FarmPlantsKey";
    private const string FarmEndTimesKey = "FarmEndTimesKey";

    private static readonly string[] SaveDataKeys = new[] { FarmsKey, FarmPlantsKey, FarmEndTimesKey };

    private static int[] farms;
    private static int[] farmPlants;
    private static DateTime[] farmCooldowns;

    public static FarmPlot.FarmState GetFarmState(int id)
    {
        if (id < 0 || id >= farms.Length) return default(FarmPlot.FarmState);
        return (FarmPlot.FarmState)farms[id];
    }

    public static void SetFarmState(int id, FarmPlot.FarmState state)
    {
        farms[id] = (int)state;
    }

    public static FarmPlot.PlantType GetFarmPlant(int id)
    {
        return (FarmPlot.PlantType)farmPlants[id];
    }

    public static void SetFarmPlant(int id, FarmPlot.PlantType plant)
    {
        farmPlants[id] = (int)plant;
    }

    public static void ClearSaveData()
    {
        foreach (string key in SaveDataKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        Load();
    }

    public static void Save()
    {
        PlayerPrefs.SetString(FarmsKey, ArrayToCSV(farms));
        PlayerPrefs.SetString(FarmPlantsKey, ArrayToCSV(farmPlants));
        PlayerPrefs.SetString(FarmEndTimesKey, ArrayToCSV(farmCooldowns));
    }

    public static void Load()
    {
        Debug.Log("Farms:");
        Debug.Log(PlayerPrefs.GetString(FarmsKey));
        Debug.Log("Plants:");
        Debug.Log(PlayerPrefs.GetString(FarmPlantsKey));
        Debug.Log("Times:");
        Debug.Log(PlayerPrefs.GetString(FarmEndTimesKey));
        farms = CSVToIntArray(PlayerPrefs.GetString(FarmsKey));
        farmPlants = CSVToIntArray(PlayerPrefs.GetString(FarmPlantsKey));
        farmCooldowns = CSVToDateTimeArray(PlayerPrefs.GetString(FarmEndTimesKey));
    }

    // int[] to string
    private static string ArrayToCSV(int[] array)
    {
        string s = "";
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i];
            if (i < array.Length - 1) s += ",";
        }
        return s;
    }

    // DateTime[] to string
    private static string ArrayToCSV(DateTime[] array)
    {
        string s = "";
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i].ToString();
            if (i < array.Length - 1) s += ",";
        }
        return s;
    }

    private static void ParseInt(string toParse, IList<int> listToAdd)
    {
        listToAdd.Add(int.Parse(toParse));
    }

    private static void ParseDateTime(string toParse, IList<DateTime> listToAdd)
    {
        listToAdd.Add(DateTime.Parse(toParse));
    }

    private static T[] CSVToTArray<T>(string csv, Action<string, IList<T>> parseFunction)
    {
        List<T> list = new List<T>();
        int leftIndex = 0;
        for (int i = 0; i < csv.Length; i++)
        {
            if (csv[i] == ',' || i == csv.Length - 1)
            {
                if (i == leftIndex) continue;
                int length = (i == csv.Length - 1) ? csv.Length - leftIndex : i - leftIndex;
                parseFunction(csv.Substring(leftIndex, length), list);
                leftIndex = i + 1;
            }
        }
        return CreateFarmArray<T>(list);
    }

    private static T[] CreateFarmArray<T>(List<T> list)
    {
        T[] farmArray = new T[Manager.MAX_FARMS];
        for (int i = 0; i < list.Count; i++)
        {
            farmArray[i] = list[i];
        }
        return farmArray;
    }

    private static int[] CSVToIntArray(string csv)
    {
        return CSVToTArray<int>(csv, ParseInt);
    }

    private static DateTime[] CSVToDateTimeArray(string csv)
    {
        return CSVToTArray<DateTime>(csv, ParseDateTime);
    }
}
