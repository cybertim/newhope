using UnityEngine;
using System.Collections;

public class Rock : Block
{
		public Rock ()
		{
				this.alpha = false;
				this.uvBase = new Vector2 (0, Random.Range (2, 4));
		}
}
