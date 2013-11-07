using UnityEngine;
using System.Collections;

public class Sand : Block
{
	public Sand ()
	{
		this.alpha = false;
		this.uvBase = new Vector2 (0, Random.Range (0, 2));
	}
}
