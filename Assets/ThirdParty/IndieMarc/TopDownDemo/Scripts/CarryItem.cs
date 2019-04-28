﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for allowing character to carry items
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.TopDown
{

    public class CarryItem : MonoBehaviour
    {
        public string item_type;
        public bool rotate_item;
        public float carry_size = 1f;
        public Vector2 carry_offset = Vector2.zero;
        public float carry_angle_deg = 0f;
        public bool reset_on_death;
        
        [HideInInspector]
        public UnityAction<GameObject> OnTake;
        [HideInInspector]
        public UnityAction<GameObject> OnDrop;

        private TopDownCharacter bearer;
        private Vector3 initial_pos;
        private Vector3 start_size;
        private Quaternion start_rot;
        private int start_order;
        private int start_layer;
        private bool start_auto_order;
        private bool trigger_at_start;
        private Vector3 last_motion = Vector3.right;
        
        private SpriteRenderer sprite_render;
        private Collider2D collide;
        private AutoOrderLayer auto_sort;
        private AudioSource audio_source;
        private float over_obstacle_count = 0f;
        private bool throwing = false;
        private TopDownCharacter last_bearer;
        private float destroy_timer = 0f;
        private float take_timer = 0f;
        private float flipX = 1f;
        private bool destroyed = false;

        private Vector3 target_pos;
        private Quaternion target_rotation;

        private static List<CarryItem> item_list = new List<CarryItem>();

        private void Awake()
        {
            item_list.Add(this);

            collide = GetComponent<Collider2D>();
            sprite_render = GetComponentInChildren<SpriteRenderer>();
            auto_sort = GetComponent<AutoOrderLayer>();
            audio_source = GetComponent<AudioSource>();
            initial_pos = transform.position;
            trigger_at_start = collide.isTrigger;
            start_size = transform.localScale;
            start_rot = transform.rotation;
            start_order = sprite_render.sortingOrder;
            start_layer = sprite_render.sortingLayerID;
            start_auto_order = auto_sort.auto_sort;
        }

        private void OnDestroy()
        {
            item_list.Remove(this);
        }

        void Start()
        {

        }

        void Update()
        {
            take_timer += Time.deltaTime;

            if (over_obstacle_count > 0f)
                over_obstacle_count -= Time.deltaTime;

            if (!bearer && !throwing)
            {
                //Called from bearer to sync when has bearer, otherwise called here
                UpdateCarryItem();
            }

            //Destroyed
            if (reset_on_death && !sprite_render.enabled)
            {
                destroy_timer += Time.deltaTime;
                if (destroy_timer > 3f)
                {
                    Reset();
                }
            }
            
        }
        
        public void UpdateCarryItem()
        {
            AdaptOrderInLayer();
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            target_pos = transform.position;
            float target_angle = 0f;
            target_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, target_angle);

            if (bearer)
            {
                TopDownCharacter player = bearer.GetComponent<TopDownCharacter>();
                Vector3 motion = player.GetMove();
                if (bearer.GetComponent<TopDownCharacter>())
                {
                    if (motion.magnitude > 0.1f)
                    {
                        last_motion = motion;
                    }
                }
                
                //Update position of the item
                flipX = player.GetSide();
                GameObject hand = bearer.hold_hand;
                target_pos = hand.transform.position + hand.transform.up * carry_offset.y + hand.transform.right * carry_offset.x * flipX;
                Vector3 rot_vector_forw = Quaternion.Euler(0f, 0f, carry_angle_deg * flipX) * hand.transform.forward;
                Vector3 rot_vector_up = Quaternion.Euler(0f, 0f, carry_angle_deg * flipX) * hand.transform.up;
                target_rotation = Quaternion.LookRotation(rot_vector_forw, rot_vector_up);
            }

            //Move the object
            transform.position = target_pos;
            transform.rotation = target_rotation;

            //Flip
            transform.localScale = bearer || throwing ? start_size * carry_size : start_size;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * flipX, transform.localScale.y, transform.localScale.z);

        }

        private void AdaptOrderInLayer()
        {
            auto_sort.RefreshSort();

            if (bearer)
            {
                if (!rotate_item)
                {
                    TopDownCharacter char_bearer = bearer.GetComponent<TopDownCharacter>();
                    int offset = char_bearer.GetSide() == 0 ? 5 : -5;
                    sprite_render.sortingOrder = char_bearer.GetSortOrder() + offset;
                }
            }
        }

        public bool CanTake(GameObject taker)
        {
            TopDownCharacter player = taker.GetComponent<TopDownCharacter>();
            CarryItem current_item = null; // player.GetHoldingItem();
            
            if (current_item != null && item_type == current_item.item_type)
                return false;
            
            if (take_timer >= -0.01f)
            {
                //Avoid taking back an item you just threw
                return (!throwing || last_bearer != taker);
            }

            return false;
        }

        public void Take(TopDownCharacter bearer)
        {
            this.bearer = bearer;
            last_bearer = bearer;
            collide.isTrigger = true;

            sprite_render.sortingLayerID = bearer.GetComponent<SpriteRenderer>().sortingLayerID;
            auto_sort.auto_sort = false; //Will be sorted based on character instead
            transform.localScale = start_size * carry_size;
            
            if (OnTake != null)
            {
                OnTake.Invoke(bearer.gameObject);
            }

            UpdateCarryItem();
        }

        public void Drop()
        {
            last_bearer = this.bearer;
            this.bearer = null;
            collide.isTrigger = throwing ? false : trigger_at_start;
            take_timer = -0.01f;

            //Reset sorting order/layer
            sprite_render.sortingOrder = start_order;
            sprite_render.sortingLayerID = start_layer;
            
            if (last_bearer && !throwing)
            {
                //Reset straight floor position
                transform.position = new Vector3(last_bearer.transform.position.x, last_bearer.transform.position.y, initial_pos.z);
                transform.localScale = start_size;
                transform.rotation = start_rot;
                flipX = 1f;
            }

            //Reset auto sort/rotate
            auto_sort.auto_sort = start_auto_order;
            auto_sort.RefreshAutoRotate();
            auto_sort.RefreshSort();

            if (OnDrop != null)
            {
                OnDrop.Invoke(last_bearer.gameObject);
            }
        }
        
        public bool IsThrowing()
        {
            return throwing;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public void UseItem()
        {
            if (bearer)
            {
                //Add code to use
            }
        }
        
        void PlayAudio()
        {
            if (audio_source)
                audio_source.Play();
        }

        public void Destroy()
        {
            if (bearer && bearer.GetComponent<TopDownCharacter>())
            {
                //bearer.GetComponent<TopDownCharacter>().DropItem();
            }
            destroyed = true;
            collide.enabled = false;
            sprite_render.enabled = false;
            destroy_timer = 0f;
        }

        public void SetStartingPos(Vector3 start_pos)
        {
            this.initial_pos = start_pos;
        }

        public TopDownCharacter GetBearer()
        {
            return this.bearer;
        }

        public bool HasBearer()
        {
            return (this.bearer != null);
        }

        public Vector3 GetOrientation()
        {
            return last_motion;
        }

        public float GetFlipX()
        {
            return flipX;
        }

        public bool IsOverObstacle()
        {
            return (over_obstacle_count > 0.01f);
        }

        public void Reset()
        {
            if (reset_on_death)
            {
                destroyed = false;
                collide.enabled = true;
                sprite_render.enabled = true;
                over_obstacle_count = 0f;
                transform.position = initial_pos;
            }
        }

        public static CarryItem[] GetAll()
        {
            return item_list.ToArray();
        }

        //Triggered on death
        public static void ResetAll()
        {
            for (int i = 0; i < item_list.Count; i++)
            {
                item_list[i].Reset();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Wall"
                || collision.gameObject.tag == "Door")
            {
                over_obstacle_count = 0.2f;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wall"
                || collision.gameObject.tag == "Door")
            {
                over_obstacle_count = 0.2f;
            }
            
        }
        
    }

}