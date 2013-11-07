using UnityEngine;
using System.Collections;

public class Dirt : Block
{
	public Dirt ()
	{
		this.alpha = false;
		this.uvBase = new Vector2 (0, Random.Range (0, 2));
	}
}
