using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;

public class Manager
{
    private string JoystickAName { get; set; }
    private string JoystickBName { get; set; }

    private List<TopDownCharacter> characters = new List<TopDownCharacter>();

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

    public string GetJoystickName(TopDownCharacter character)
    {
        // add character to list
        if (!characters.Contains(character)) characters.Add(character);
        if (character.Player2)
        {
            return JoystickBName;
        }
        else
        {
            return JoystickAName;
        }
    }

    public List<TopDownCharacter> GetCharacters()
    {
        return characters;
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

        // fall back to keyboard
        if (JoystickAName == null)
        {
            JoystickAName = "Keyboard1";
            JoystickBName = "Keyboard2";
        }
    }
}
