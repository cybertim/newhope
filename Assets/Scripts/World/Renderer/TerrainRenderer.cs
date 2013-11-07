using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainRenderer : IRenderer
{
		private World world;
		private Chunk chunk;
		private List<Vector2> uvs;
		private List<Vector3> vertices;
		private List<int>[] triangles;
		private static int subMeshCount = 2;

		public void Initialize ()
		{
				this.uvs = new List<Vector2> ();
				this.vertices = new List<Vector3> ();
				this.triangles = new List<int>[2];
				for (int i = 0; i < subMeshCount; i++) {
						triangles [i] = new List<int> ();
				}
		}

		public void Render (World world, Chunk chunk)
		{
				this.world = world;
				this.chunk = chunk;

				vertices.Clear ();
				uvs.Clear ();
				for (int i = 0; i < subMeshCount; i++) {
						triangles [i].Clear ();
				}

				Block block;
				Vector3 start;

				for (int x = 0; x < world.ChunkX; x++) {
						for (int y = 1; y < world.LevelY; y++) {
								for (int z = 0; z < world.ChunkZ; z++) {
										start = new Vector3 (x, y, z);
										block = chunk.GetBlock (x, y, z);
										if (block == null)
												continue;
										if (MustFaceBeVisible (x - 1, y, z, block)) {
												DrawFace (start + Vector3.back, start, start + Vector3.back + Vector3.up, start + Vector3.up, block);
										} 
										if (MustFaceBeVisible (x + 1, y, z, block)) {
												DrawFace (start + Vector3.right, start + Vector3.right + Vector3.back, start + Vector3.right + Vector3.up, start + Vector3.right + Vector3.back + Vector3.up, block);
										}
										if (MustFaceBeVisible (x, y - 1, z, block)) {
												DrawFace (start, start + Vector3.back, start + Vector3.right, start + Vector3.back + Vector3.right, block);
										}
										if (MustFaceBeVisible (x, y + 1, z, block)) {
												DrawFace (start + Vector3.up + Vector3.back, start + Vector3.up, start + Vector3.up + Vector3.back + Vector3.right, start + Vector3.up + Vector3.right, block);
										}
										if (MustFaceBeVisible (x, y, z - 1, block)) {
												DrawFace (start + Vector3.back + Vector3.right, start + Vector3.back, start + Vector3.back + Vector3.right + Vector3.up, start + Vector3.back + Vector3.up, block);		
										} 
										if (MustFaceBeVisible (x, y, z + 1, block)) {
												DrawFace (start, start + Vector3.right, start + Vector3.up, start + Vector3.right + Vector3.up, block);
										}
								}
						}
				}
		}

		public Mesh ToMesh (Mesh mesh)
		{
				if (vertices.Count == 0) {
						GameObject.Destroy (mesh);
						return null;
				}
				if (mesh == null)
						mesh = new Mesh ();
				mesh.Clear ();
				mesh.vertices = vertices.ToArray ();
				mesh.subMeshCount = triangles.Length;
				for (int i = 0; i < triangles.Length; i++) {
						mesh.SetTriangles (triangles [i].ToArray (), i);
				}
				mesh.uv = uvs.ToArray ();
				mesh.RecalculateNormals ();
				mesh.RecalculateBounds ();
				return mesh;
		}

		private void DrawFace (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Block block)
		{
				int i = 0;
				if (block.Alpha) {
						i = 1;
				}
				int index = vertices.Count;
				vertices.Add (v1);
				vertices.Add (v2);
				vertices.Add (v3);
				if (v4 != Vector3.zero) {
						vertices.Add (v4);
				}
				float tec = (1f / 8);
				Vector2 uvBase;
				if (block.top) {
						uvBase = new Vector2 (1 * tec, Random.Range (0, 2) * tec);
				} else {
						uvBase = block.UV * tec;
				}
				//Vector2 uvBase = new Vector2 (0, 0);
				uvs.Add (uvBase);
				uvs.Add (uvBase + new Vector2 (tec, 0));
				if (v4 != Vector3.zero) {
						uvs.Add (uvBase + new Vector2 (0, tec));
				}
				uvs.Add (uvBase + new Vector2 (tec, tec));

				triangles [i].Add (index + 0);
				triangles [i].Add (index + 1);
				triangles [i].Add (index + 2);
				if (v4 != Vector3.zero) {
						triangles [i].Add (index + 3);
						triangles [i].Add (index + 2);
						triangles [i].Add (index + 1);
				}
		}

		private bool MustFaceBeVisible (int x, int y, int z, Block block)
		{
				bool show = false;
				Block neighbourBlock = RetreiveBlock (x, y, z);
				if (neighbourBlock == null || (neighbourBlock.Alpha && !block.Alpha)) {
						show = true;
				}
				// cut-off top - show special texture
				if (!show && world.LevelY == y) {
						block.top = true;
						return true;
				} else {
						block.top = false;
				}
				return show;
		}

		private Block RetreiveBlock (int x, int y, int z)
		{
				if (y < 0 && y >= world.ChunkY) {
						return null;
				}

				Block block = null;

				if (x >= 0 && x < world.ChunkX && z >= 0 && z < world.ChunkZ) {
						block = chunk.GetBlock (x, y, z);
				} else {
						Chunk neighbourChunk = world.GetChunkWorldPos (chunk.WorldX + x, chunk.WorldZ + z);
						if (neighbourChunk.generated) {
								block = neighbourChunk.GetBlockWorldPos (chunk.WorldX + x, y, chunk.WorldZ + z);
						} 
				}

				return block;
		}
}
