﻿using UnityEngine;
using System.Collections;

public class Grass : Block
{
		public Grass ()
		{
				this.alpha = false;
				this.uvBase = new Vector2 (0, Random.Range (0, 2));
		}
}
