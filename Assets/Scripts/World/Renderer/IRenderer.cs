using UnityEngine;
using System.Collections;

public interface IRenderer
{
		void Initialize ();

		void Render (World world, Chunk chunk);

		Mesh ToMesh (Mesh mesh);
}
