using XInputDotNetPure;
using UnityEngine;
using System.Collections.Generic;

public static class InputManager
{
    private struct InputWrapper
    {
        public PlayerIndex Index;
        public GamePadState Prev;
        public GamePadState State;
        public bool Found;
    }

    public const int PLAYER1 = 0;
    public const int PLAYER2 = 1;

    private static InputWrapper[] playerInputs = new[] {
        new InputWrapper { Index = (PlayerIndex) PLAYER1 },
        new InputWrapper { Index = (PlayerIndex) PLAYER2 }
    };

    private static readonly List<int> assignedControllers = new List<int>();

    public static Vector2 GetDirectional(int id)
    {
        // check if valid player
        ValidityCheck(id);

        // get left joystick input
        if (playerInputs[id].Found)
        {
            // left stick for movement
            var l = playerInputs[id].State.ThumbSticks.Left;
            return new Vector2(l.X,l.Y);
        }
        // use keyboard as a fallback
        else
        {
            int plusone = id + 1;
            return new Vector2 (Input.GetAxis("HorizontalKeyboard" + plusone), Input.GetAxis("VerticalKeyboard" + plusone));
        }        
    }

    public static bool InteractionButtonIsHeld(int id)
    {
        bool pressed;
        bool unused;
        GetInteractionButtonState(id, out pressed, out unused);
        return pressed;
    }

    public static bool InteractionButtonReleasedThisFrame(int id)
    {
        bool pressed;
        bool pressedPrev;
        GetInteractionButtonState(id, out pressed, out pressedPrev);
        return pressed && !pressedPrev;
    }

    private static void GetInteractionButtonState(int id, out bool pressed, out bool pressedPrev)
    {
        // check if valid player
        ValidityCheck(id);

        // get left joystick input
        if (playerInputs[id].Found)
        {
            // "a" button to interact
            pressed = playerInputs[id].State.Buttons.A == ButtonState.Pressed;
            pressedPrev = playerInputs[id].Prev.Buttons.A == ButtonState.Pressed;
        }
        // use keyboard as a fallback
        else
        {
            int plusone = id + 1;
            string inputName = "ActionKeyboard" + plusone;
            pressed = Input.GetButton(inputName);
            if (Input.GetButtonDown(inputName))
            {
                pressedPrev = false;
            }
            else if (Input.GetButtonUp(inputName))
            {
                pressedPrev = true;
            }
            else
            {
                pressedPrev = pressed;
            }
        }
    }

    private static void ValidityCheck(int id)
    {
        if (id < 0 || id >= playerInputs.Length) throw new System.Exception("Player Index out of range: " + id);
    }

    public static void PerFrameUpdate()
    {
        AssignControllers();
    }

    /// Assume controllers may have been disconnected
    private static void AssignControllers()
    {
        // see if any controllers are available
        for (int i = 0; i < 4; ++i)
        {
            if (assignedControllers.Contains(i)) continue;
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            // controller available!
            if (testState.IsConnected && testState.Buttons.A == ButtonState.Pressed)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                for (int player = 0; player < playerInputs.Length; player++)
                {
                    // found a player in need of a controller
                    if (!playerInputs[player].Found)
                    {
                        Debug.Log($"Assigning {(int) testPlayerIndex} to {player}");
                        HUD.ShowToast($"{player.ToPlayerName()} connected!");
                        playerInputs[player].Index = testPlayerIndex;
                        playerInputs[player].Found = true;
                        assignedControllers.Add(i);
                        break; // one player per controller
                    }
                }
            }
        }
        for (int player = 0; player < playerInputs.Length; player++)
        {
            if (playerInputs[player].Found)
            {
                playerInputs[player].Prev = playerInputs[player].State;
                playerInputs[player].State = GamePad.GetState(playerInputs[player].Index);
            }
        }
    }
}
