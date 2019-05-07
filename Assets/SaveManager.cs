using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    // --- Enum Representing the Player Pref Keys and Associated Data ---
    public enum Category { Farms, FarmPlants, FarmEndTimes, InventoryItemTypes, InventoryItemQuantities }; // must match

    private static float lastChangeTime = 0;

    // --- Keys ---
    // farm
    private const string FarmsKey = "FarmsKey";
    private const string FarmPlantsKey = "FarmPlantsKey";
    private const string FarmEndTimesKey = "FarmEndTimesKey";
    // inventory
    private const string InventoryItemTypesKey = "InventoryItemTypesKey";
    private const string InventoryItemQuantitiesKey = "InventoryItemQuantitiesKey";

    // --- Arrays ---

    // int
    private static readonly string[] IntDataKeys = new[] { FarmsKey, FarmPlantsKey, InventoryItemTypesKey, InventoryItemQuantitiesKey }; // must match Category
    private static readonly int[] IntDataArrayLengths = new[] { Manager.MAX_FARMS, Manager.MAX_FARMS, InventoryManager.MAX_INV_SLOTS, InventoryManager.MAX_INV_SLOTS }; // must match Category
    private static readonly int[][] IntDataArrays = new int[4][]; // must match Category

    // date
    private static readonly string[] DateTimeDataKeys = new[] { FarmEndTimesKey }; // must match Category
    private static readonly int[] DateTimeDataArrayLengths = new[] { Manager.MAX_FARMS }; // must match Category
    private static readonly DateTime[][] DateTimeDataArrays = new DateTime[1][]; // must match Category

    // --- Setters and Getters ---

    private static int[] GetIntArrFromCategory(Category category)
    {
        // TODO dictionary
        switch(category)
        {
            case Category.Farms:
                return IntDataArrays[0];
            case Category.FarmPlants:
                return IntDataArrays[1];
            case Category.InventoryItemTypes:
                return IntDataArrays[2];
            case Category.InventoryItemQuantities:
                return IntDataArrays[3];
        }
        throw new Exception("no category!");
    }
    
    public static int GetInt(Category category, int entry)
    {
        int[] arr = GetIntArrFromCategory(category);
        // bounds check
        if (entry < 0 || entry >= arr.Length) {
            Debug.LogError($"SaveManager error: Entry {category}:{entry} not found!");
            return 0;
        }
        // return
        return arr[entry];
    }

    public static void SetInt(Category category, int entry, int value)
    {
        int[] arr = GetIntArrFromCategory(category);
        // bounds check
        if (entry < 0 || entry >= arr.Length)
        {
            Debug.LogError($"SaveManager error: Entry {category}:{entry} not found!");
            return;
        }
        // set
        arr[entry] = value;
        lastChangeTime = Time.time;
    }

    public static FarmPlot.FarmState GetFarmState(int id)
    {
        return (FarmPlot.FarmState)GetInt(Category.Farms, id);
    }

    public static void SetFarmState(int id, FarmPlot.FarmState state)
    {
        SetInt(Category.Farms, id, (int)state);
        lastChangeTime = Time.time;
    }

    public static FarmPlot.PlantType GetFarmPlant(int id)
    {
        return (FarmPlot.PlantType)GetInt(Category.FarmPlants, id);
    }

    public static void SetFarmPlant(int id, FarmPlot.PlantType plant)
    {
        SetInt(Category.FarmPlants, id, (int)plant);
        lastChangeTime = Time.time;
    }

    public static void SetFarmDoneTime(int id, DateTime done)
    {
        DateTimeDataArrays[0][id] = done; // TODO make setter like int arr
        lastChangeTime = Time.time;
    }

    public static DateTime GetFarmDoneTime(int id)
    {
        return DateTimeDataArrays[0][id];// TODO make getter like int arr
    }

    // --- Manager Functions ---

    public static float GetLastChangeTime()
    {
        return lastChangeTime;
    }

    public static void ClearSaveData()
    {
        foreach (string key in IntDataKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        foreach (string key in DateTimeDataKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        lastChangeTime = Time.time;
    }

    public static void Save()
    {
        // int
        for (int i = 0; i < IntDataKeys.Length; i++)
        {
            PlayerPrefs.SetString(IntDataKeys[i], ArrayToCSV(IntDataArrays[i]));
        }
        // DateTime
        for (int i = 0; i < DateTimeDataKeys.Length; i++)
        {
            PlayerPrefs.SetString(DateTimeDataKeys[i], ArrayToCSV(DateTimeDataArrays[i]));
        }
    }

    public static void Load()
    {
        // int
        for (int i = 0; i < IntDataKeys.Length; i++)
        {
            IntDataArrays[i] = CSVToIntArray(PlayerPrefs.GetString(IntDataKeys[i]), IntDataArrayLengths[i]);
        }
        // DateTime
        for (int i = 0; i < DateTimeDataKeys.Length; i++)
        {
            DateTimeDataArrays[i] = CSVToDateTimeArray(PlayerPrefs.GetString(DateTimeDataKeys[i]), DateTimeDataArrayLengths[i]);
        }
        lastChangeTime = Time.time;
    }

    // --- Utility Functions ---

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

    private static T[] CSVToTArray<T>(string csv, Action<string, IList<T>> parseFunction, int maxSize)
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
        T[] arr = new T[maxSize];
        for (int i = 0; i < list.Count; i++)
        {
            arr[i] = list[i];
        }
        return arr;
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

    private static int[] CSVToIntArray(string csv, int maxSize)
    {
        return CSVToTArray<int>(csv, ParseInt, maxSize);
    }

    private static DateTime[] CSVToDateTimeArray(string csv, int maxSize)
    {
        return CSVToTArray<DateTime>(csv, ParseDateTime, maxSize);
    }
}
