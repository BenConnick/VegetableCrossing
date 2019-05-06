#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif
using UnityEngine;
using System.Collections.Generic;

public static class InputManager
{
#if UNITY_STANDALONE_WIN
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
#endif

    public static Vector2 GetDirectional(int id)
    {
#if UNITY_STANDALONE_WIN
        // check if valid player
        ValidityCheck(id);

        // get left joystick input
        if (playerInputs[id].Found)
        {
            // left sticfor movement
            var l = playerInputs[id].State.ThumbSticks.Left;
            return new Vector2(l.X,l.Y);
        }
#endif

        // use keyboard as a fallback
        int plusone = id + 1;
        return new Vector2 (Input.GetAxis("HorizontalKeyboard" + plusone), Input.GetAxis("VerticalKeyboard" + plusone));
       
    }

    public static bool InteractionButtonIsHeld(int id)
    {
        GetInteractionButtonState(id, out bool pressed, out bool unused);
        return pressed;
    }

    public static bool InteractionButtonReleasedThisFrame(int id)
    {
        GetInteractionButtonState(id, out bool pressed, out bool pressedPrev);
        return pressed && !pressedPrev;
    }

    private static void GetInteractionButtonState(int id, out bool pressed, out bool pressedPrev)
    {
#if UNITY_STANDALONE_WIN
        // check if valid player
        ValidityCheck(id);

        // get left joystick input
        if (playerInputs[id].Found)
        {
            // "a" button to interact
            pressed = playerInputs[id].State.Buttons.A == ButtonState.Pressed;
            pressedPrev = playerInputs[id].Prev.Buttons.A == ButtonState.Pressed;
            return;
        }
#endif
        // use keyboard as a fallback
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

    private static void ValidityCheck(int id)
    {
#if UNITY_STANDALONE_WIN
        if (id < 0 || id >= playerInputs.Length) throw new System.Exception("Player Index out of range: " + id);
#endif
    }

    public static void PerFrameUpdate()
    {
        AssignControllers();
    }

    /// Assume controllers may have been disconnected
    private static void AssignControllers()
    {
#if UNITY_STANDALONE_WIN
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
#endif
    }
}
