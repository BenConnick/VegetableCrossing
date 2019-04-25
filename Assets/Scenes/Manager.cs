using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;

public class Manager
{
    public string JoystickAName { get; set; }
    public string JoystickBName { get; set; }

    private static Manager _inst;
    public static Manager Inst { 
        get {
            if (_inst == null) _inst = new Manager();
            return _inst;
        } 
    }

    private Manager()
    {
        // private constructor
    }

    public void PerFrameUpdate()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        JoystickAName = null;
        JoystickBName = null;
        //Get Joystick Names
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                    if (JoystickAName == null)
                    {
                        JoystickAName = i.ToString();
                    }
                    else
                    {
                        JoystickBName = i.ToString();
                        break;
                    }
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");
                }
            }
        }

        if (JoystickAName == null)
        {
            JoystickAName = "Keyboard1";
            JoystickbName = "Keyboard2";
        }
    }
}
