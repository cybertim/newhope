using UnityEngine;
using System.Collections;

public class Water :Block
{
		public Water ()
		{
				this.alpha = true;
				this.uvBase = new Vector2 (0, Random.Range (6, 8));
		}

		protected override void OnUpdate (World world, int x, int y, int z)
		{
				if (world.GetBlock (x, y, z) == null) {
						world.SetBlock (new Water (), x, y, z);
				}
		}
}
