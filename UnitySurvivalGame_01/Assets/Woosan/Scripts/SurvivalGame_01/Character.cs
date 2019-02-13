using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.SurvivalGame00 {
    /// <summary>
    /// 캐릭터를 움직는 녀석. 유니티 예제 가져다 씀.
    /// </summary>
    public class Character : MonoBehaviour
    {
        public float speed = 6.0f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;

        private Vector3 moveDirection = Vector3.zero;
        private UnityEngine.CharacterController controller;

        void Start()
        {
            controller = GetComponent<UnityEngine.CharacterController>();

            // let the gameObject fall down
            gameObject.transform.position = new Vector3(0, 2, 0);
        }

        void Update()
        {
            if (controller.isGrounded)
            {
                // We are grounded, so recalculate
                // move direction directly from axes

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection = moveDirection * speed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            // Apply gravity
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

            // Move the controller
            controller.Move(moveDirection * Time.deltaTime);
        }

    }
}


