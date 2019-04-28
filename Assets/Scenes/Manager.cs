using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;


public static class Manager
{
    private static List<TopDownCharacter> characters = new List<TopDownCharacter>();
    private static int prevFrame;

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
}
