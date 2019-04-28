using System;
using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;


public static class Manager
{
    private static List<TopDownCharacter> characters = new List<TopDownCharacter>();
    private static int prevFrame;
    private static Camera mainCam;
    public const int MAX_FARMS = 50;

    public static void Init()
    {
        SaveManager.Load();
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
}
