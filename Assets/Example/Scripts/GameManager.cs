using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public PlayerController Player;


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Dont destroy on reloading the scene
        DontDestroyOnLoad(gameObject);

 
    }

    public string JoystickAName { get; set; }
    public string JoystickBName { get; set; }

    private void Update()
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
                    if (JoystickAName == null) JoystickAName = i.ToString();
                    else JoystickBName = i.ToString();
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");

                }
            }
        }
    }
    
}
