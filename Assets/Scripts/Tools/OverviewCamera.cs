using UnityEngine;
using System.Collections;

public class OverviewCamera : MonoBehaviour
{
		public bool mouseScroll;
		public bool keyboardScroll;
		// Use this for initialization
		void Start ()
		{
	
		}
		// Update is called once per frame
		void Update ()
		{
				float mousePosX = Input.mousePosition.x; 
				float mousePosY = Input.mousePosition.y; 
				int scrollDistance = 5; 
				float scrollSpeed = 70;
				float vertical = Input.GetAxis ("Vertical") * scrollSpeed;
				float horizontal = Input.GetAxis ("Horizontal") * scrollSpeed;

				if (keyboardScroll) {
						Vector3 speed = new Vector3 (horizontal, 0f, vertical);
						transform.position = transform.position + (speed * Time.deltaTime);
				}
				if (mouseScroll) {
						if (mousePosX < scrollDistance) { 
								transform.Translate (Vector3.right * -scrollSpeed * Time.deltaTime); 
						} 

						if (mousePosX >= Screen.width - scrollDistance) { 
								transform.Translate (Vector3.right * scrollSpeed * Time.deltaTime); 
						}

						if (mousePosY < scrollDistance) { 
								transform.Translate (transform.forward * -scrollSpeed * Time.deltaTime); 
						} 

						if (mousePosY >= Screen.height - scrollDistance) { 
								transform.Translate (transform.forward * scrollSpeed * Time.deltaTime); 
						}
				}
	
		}
}
