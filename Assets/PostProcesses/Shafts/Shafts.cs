using UnityEngine;
using System.Collections;
using System;

public class Shafts : MonoBehaviour
{

		public float Intensity = 0.38f;
		public float Range = 150;
		public int SpeedCheck = 10;
		public int SpeedCount = 0;
		public float Quality = 90;
		public float Smoothing = 10;
		public Transform Sun;
		public Material BloomMaterial;
		public LayerMask RayMask = -1;
		Texture2D DTMap;
		int Width = 1;
		int Height = 1;
		int LWidth = 0;
		int LHeight = 0;
		float LQuality = 0;
		Vector3 pos;
		float forw;

		void Start ()
		{
				Ray ray = camera.ScreenPointToRay (pos);
		}

		void Update ()
		{
				SpeedCount = SpeedCount - 1;
				if (SpeedCount <= 0) {  
						forw = Vector3.Dot (transform.forward, Sun.forward);
						if (LWidth != Screen.width || LHeight != Screen.height || LQuality != Quality) {
								Width = (int)Mathf.Round ((Screen.width / new Vector2 (Screen.width, Screen.height).magnitude) * Quality);
								Height = (int)Mathf.Round ((Screen.height / new Vector2 (Screen.width, Screen.height).magnitude) * Quality);
								DTMap = new Texture2D (Width, Height);
						}
						LWidth = Screen.width;
						LHeight = Screen.height;
						LQuality = Quality; 
						GetShafts ();
						SpeedCount = SpeedCheck;  
						Vector3 p = camera.WorldToScreenPoint (transform.position - Sun.forward);
						BloomMaterial.SetFloat ("_CenX", p.x / Screen.width);
						BloomMaterial.SetFloat ("_CenY", p.y / Screen.height);
						DTMap.wrapMode = TextureWrapMode.Clamp;
						DTMap.Apply ();
				}
		}

		void OnGUI ()
		{
				if (Event.current.type == EventType.Repaint) {
						Graphics.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), DTMap, BloomMaterial);
				}
		}

		void GetShafts ()
		{
				bool inangle = false;
				Vector2 ss = new Vector2 (LWidth, LHeight);
				if (forw < 0)
						inangle = true;
				//RaycastHit Hit;
				for (int x = 0; x<Width; x++) {
						float xQ = (x * ss.magnitude) / Quality;
						for (int y = 0; y<Height; y++) {
								pos = new Vector3 (xQ, (y * ss.magnitude) / Quality, 0);
								if (inangle) {
										bool hitting = false;
										//if(Physics.Raycast(camera.ScreenPointToRay(pos), Range, RayMask) && inangle)
										Ray ray = getray (pos);  
										if (Cast (ray) && inangle) {
												hitting = false;
										} else {

												hitting = true;
										}
										Col (x, y, hitting);
								} else {
										Col (x, y, false); 
								}   
						}
				}
				//});
		}

		Ray getray (Vector3 pos)
		{
				Ray ray = camera.ScreenPointToRay (pos);
				return ray;
		}

		bool Cast (Ray ray)
		{
				bool boool = false;
				if (Physics.Raycast (ray, Range, RayMask)) {
						boool = true;
				}
				return boool;
		}

		void Col (int x, int y, bool hitting)
		{
				if (hitting) {
						DTMap.SetPixel (x, y, Color.Lerp (DTMap.GetPixel (x, y), Color.white, Smoothing * Time.deltaTime));
				} else {
						DTMap.SetPixel (x, y, Color.Lerp (DTMap.GetPixel (x, y), Color.black, Smoothing * Time.deltaTime));
				}
		}
}