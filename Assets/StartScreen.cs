﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : BaseScreen
{
    // Start is called before the first frame update
    void Awake()
    {
        ScreenManager.AddStartScreen(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewGamePressed()
    {
        Manager.ClearProgressAndBeginNewGame();
        ScreenManager.PopScreen();
    }

    public void OnContinuePressed()
    {
        Manager.StartGame();
        ScreenManager.PopScreen();
    }

    public void OnOptionsPressed()
    {

    }
}
