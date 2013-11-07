using UnityEngine;
using System.Collections;

public class Bloom : MonoBehaviour {

	public float Quality = 90;
	public float Smoothing = 10;
	public Color SkyColor = new Color(0.1f, 0.2f, 0.3f);
	public Material BloomMaterial;
	public LayerMask RayMask = -1;
	Texture2D DTMap;
	int Width = 1;
	int Height = 1;
	int LWidth = 0;
	int LHeight = 0;
	float LQuality = 0;
	
	void Update () 
	{
		
		if(LWidth != Screen.width || LHeight != Screen.height || LQuality != Quality)
		{
		
			Width = (int)Mathf.Round((Screen.width / new Vector2(Screen.width, Screen.height).magnitude) * Quality);
			Height = (int)Mathf.Round((Screen.height / new Vector2(Screen.width, Screen.height).magnitude) * Quality);
			
			DTMap = new Texture2D(Width, Height);
			
		}
		
		LWidth = Screen.width;
		LHeight = Screen.height;
		LQuality = Quality;
		
		for(int x = 0; x<Width; x++)
		{
			
			for(int y = 0; y<Height; y++)
			{
				RaycastHit Hit;
				if(!Physics.Raycast(camera.ScreenPointToRay(new Vector3((x * new Vector2(Screen.width, Screen.height).magnitude) / Quality, (y * new Vector2(Screen.width, Screen.height).magnitude) / Quality, 0)), out Hit, Mathf.Infinity, RayMask))
				{
						
					DTMap.SetPixel(x, y, Color.Lerp(DTMap.GetPixel(x, y), SkyColor, Smoothing*Time.deltaTime));
					
				}else{
					if(Hit.transform.GetComponent<BloomObject>())
					{
					DTMap.SetPixel(x, y, Color.Lerp(DTMap.GetPixel(x, y), Hit.transform.GetComponent<BloomObject>().BloomColor, Smoothing*Time.deltaTime));
					}else{
					DTMap.SetPixel(x, y, Color.Lerp(DTMap.GetPixel(x, y), Color.black, Smoothing*Time.deltaTime));
					}
				}
				
			}
			
		}
				
		DTMap.wrapMode = TextureWrapMode.Clamp;
		DTMap.Apply();
	
	}
	
	void OnGUI ()
	{
		
		if(Event.current.type == EventType.Repaint)
		{
		
		Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), DTMap, BloomMaterial);
			
		}
		
	}
}
