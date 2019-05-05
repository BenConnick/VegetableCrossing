using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;


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

        InputManager.PerFrameUpdate();
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
        farmPlots.Add(plot.Id, plot);
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
    }

    // may want to move this into save manager
    private static void InitFarms()
    {
        foreach (var plot in farmPlots)
        {
            plot.SetState(SaveManager.GetFarmState(plot.Id), SaveManager.GetFarmPlant(plot.Id));
        }
    }
}
