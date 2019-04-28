using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Top down character movement
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.TopDown
{
    public class TopDownCharacter : MonoBehaviour
    {
        public int player_id;

        [Header("Movement")]
        public float move_accel = 1f;
        public float move_deccel = 1f;
        public float move_max = 1f;

        [Header("Parts")]
        public GameObject hold_hand;

        [HideInInspector] public UnityAction onDeath;
        
        private Rigidbody2D rigid;
        private Animator animator;
        private AutoOrderLayer auto_order;
        private ContactFilter2D contact_filter;

        private CarryItem carry_item;
        private Vector2 move;
        private Vector2 move_input;
        private Vector2 lookat = Vector2.zero;
        private float side = 1f;
        private bool disable_controls = false;

        private Tooltip tooltip;
        private IInteractionTrigger currentSelection;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            auto_order = GetComponent<AutoOrderLayer>();
            Manager.GetCharacters().Add(this);
        }

        void OnDestroy()
        {
            Manager.GetCharacters().Remove(this);
        }

        //Handle physics
        void FixedUpdate()
        {
            //Movement velocity
            float desiredSpeedX = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float accelerationX = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            move.x = Mathf.MoveTowards(move.x, desiredSpeedX, accelerationX * Time.fixedDeltaTime);
            float desiredSpeedY = Mathf.Abs(move_input.y) > 0.1f ? move_input.y * move_max : 0f;
            float accelerationY = Mathf.Abs(move_input.y) > 0.1f ? move_accel : move_deccel;
            move.y = Mathf.MoveTowards(move.y, desiredSpeedY, accelerationY * Time.fixedDeltaTime);

            //Move
            rigid.velocity = move;
            
        }

        //Handle render and controls
        void Update()
        {
            move_input = Vector2.zero;

            //Controls
            if (!disable_controls)
            {
                // Get Input for axis
                Vector2 movementInput = InputManager.GetDirectional(player_id);
                move_input = movementInput.normalized;

                // Get Input for button
                bool released = InputManager.InteractionButtonReleasedThisFrame(player_id);
                if (released)
                {
                    if (currentSelection != null)
                    {
                        DoInteraction();
                    }
                }
            }

            //Update lookat side
            if (move.magnitude > 0.1f)
                lookat = move.normalized;
            if (Mathf.Abs(lookat.x) > 0.02)
                side = Mathf.Sign(lookat.x);
            
            //Anims
            animator.SetFloat("Speed", move.magnitude);
            animator.SetInteger("Side", GetSideAnim());
        }

        private void DoInteraction()
        {
            if (currentSelection == null) return;
            currentSelection.GetInteractAction(player_id)?.Invoke();
        }

        public void Kill()
        {
            //To Do
            //Not done because right now there is nothing beyond the demo level.
            //Could make it lose a life, or reload the level
        }

        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            move = Vector2.zero;
        }

        public Vector3 GetMove()
        {
            return move;
        }

        public int GetSortOrder()
        {
            return auto_order.GetSortOrder();
        }

        //Get Character side
        public float GetSide()
        {
            return side; //Return 1 frame before to let anim do transitions
        }

        public int GetSideAnim()
        {
            return (side >= 0) ? 1 : 3;
        }

        public Vector3 GetHandPos()
        {
            return hold_hand.transform.position;
        }

        public bool IsAlive()
        {
            return true; //TO DO
        }

        public void DisableControls() { disable_controls = true; }
        public void EnableControls() { disable_controls = false; }

        void OnCollisionStay2D(Collision2D coll)
        {
            
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            var plot = coll.GetComponent<IInteractionTrigger>();            
            if (plot != null && plot.IsInteractable())
            {
                EnterInteractable(plot, coll.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D coll)
        {
            var plot = coll.GetComponent<IInteractionTrigger>();
            if (plot != null)
            {
                ExitInteractable(plot);
            }
        }

        private void EnterInteractable(IInteractionTrigger trigger, Transform target)
        {
            if (trigger == currentSelection || !trigger.IsInteractable()) return;
            currentSelection = trigger;
            Tooltip prev = tooltip;
            tooltip = HUD.ShowTooltip(trigger.GetTooltipText(player_id), target);
            if (prev != null) prev.Hide();
        }

        private void ExitInteractable(IInteractionTrigger trigger)
        {
            if (currentSelection != trigger) return;
            if (tooltip != null) tooltip.Hide();
            currentSelection = null;
        }

        public FarmPlot.PlantType GetHeldSeed()
        {
            return FarmPlot.PlantType.Rabbit;
        }

        /// --------- STATIC UTILITIES --------------

        public static void LockGameplay()
        {
            foreach (TopDownCharacter character in GetAll())
                character.DisableControls();
        }

        public static void UnlockGameplay()
        {
            foreach (TopDownCharacter character in GetAll())
                character.EnableControls();
        }

        public static TopDownCharacter GetNearest(Vector3 pos, float range = 999f, bool alive_only=true)
        {
            TopDownCharacter nearest = null;
            float min_dist = range;
            foreach (TopDownCharacter character in Manager.GetCharacters())
            {
                if (!alive_only || character.IsAlive())
                {
                    float dist = (pos - character.transform.position).magnitude;
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        nearest = character;
                    }
                }
            }
            return nearest;
        }

        public static TopDownCharacter Get(int player_id)
        {
            foreach (TopDownCharacter character in Manager.GetCharacters())
            {
                if (character.player_id == player_id)
                {
                    return character;
                }
            }
            return null;
        }

        public static TopDownCharacter[] GetAll()
        {
            return Manager.GetCharacters().ToArray();
        }
    }
}
