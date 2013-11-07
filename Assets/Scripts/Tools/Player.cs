using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

				public float movementSpeed = 4.5f;
		public float mouseSensitivity = 5.0f;
		public float jumpSpeed = 8.0f;
		public float upDownRange = 45.0f;
		protected float verticalRotation = 0;
		protected float verticalVelocity = 0;
		protected CharacterController characterController;
		protected bool gravity = false;
		protected bool running = false;

		void Start ()
		{
				//Screen.lockCursor = true;
				characterController = GetComponent<CharacterController> ();
		}

		void Update ()
		{				
				// Rotation
				float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
				transform.Rotate (0, rotLeftRight, 0);

				verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
				verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
				Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

				// Moving forward
				float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
				// Maybe running?
				if (running) {
						forwardSpeed = forwardSpeed * 2f;
				}
				
				float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

				if (Input.GetKey (KeyCode.LeftShift)) {						
						running = true;
				} else {
						running = false;
				}

				if (Input.GetButton ("Fire1")) {
						gravity = true;
				}

				if (gravity) {
						verticalVelocity += Physics.gravity.y * Time.deltaTime * movementSpeed / 4;
				}

				// Doing a jump!
				if (characterController.isGrounded && Input.GetButton ("Jump")) {
						verticalVelocity = jumpSpeed;
				}

				Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);

				speed = transform.rotation * speed;

				characterController.Move (speed * Time.deltaTime);
		}
}
