using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;
using System;

public static class Manager
{
    private static List<TopDownCharacter> characters = new List<TopDownCharacter>();
    private static IntSet<FarmPlot> farmPlots = new IntSet<FarmPlot>();
    private static int prevFrame;
    private static Camera mainCam;
    public const int MAX_FARMS = 50;
    private static bool started = false;

    public static void Init()
    {
        // unused for now
    }

    public static void PerFrameUpdate()
    {
        if (Time.frameCount == prevFrame) return;
        prevFrame = Time.frameCount;

        // handle player input
        InputManager.PerFrameUpdate();

        // --- EVERYTHING PAST THIS POINT IS ONLY WHEN THE GAME IS RUNNING ---
        if (!started) return;

        // handle plants
        UpdateFarmTimers();
    }

    public static List<TopDownCharacter> GetCharacters()
    {
        return characters;
    }

    public static Camera GetMainCamera()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        return mainCam;
    }

    public static void Register(FarmPlot plot)
    {
        if (farmPlots.IsSet(plot.Id))
        {
            Debug.LogError($"Cannot register plot, a plot with id {plot.Id} is already registered");
            return;
        }
        farmPlots.Set(plot.Id, plot);
    }

    public static void ClearProgressAndBeginNewGame()
    {
        SaveManager.ClearSaveData();
        
        StartGame();
    }

    public static void StartGame()
    {
        if (started)
        {
            Debug.LogError("Cannot start game: already started");
            return;
        }

        SaveManager.Load();
        InitFarms();
        started = true;
    }

    // may want to move this into save manager
    private static void InitFarms()
    {
        foreach (var plot in farmPlots)
        {
            plot.SetState(SaveManager.GetFarmState(plot.Id), SaveManager.GetFarmPlant(plot.Id));
        }
    }

    private static void UpdateFarmTimers()
    {
        const FarmPlot.FarmState GrowingState = FarmPlot.FarmState.Sapling;
        DateTime now = DateTime.Now;
        foreach (var plot in farmPlots)
        {
            int id = plot.Id;
            // if growing and time expired, enter harvestable state
            if (SaveManager.GetFarmState(id) == GrowingState 
                && SaveManager.GetFarmDoneTime(id) < now)
            {
                plot.OnFinishGowing();
            }
        }
    }
}

public enum ItemType { Default, RabbitSeed, Rabbit }