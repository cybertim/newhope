using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{
		public int sightRadius;
		public Vector3 chunkSize;
		public GameObject mainCamera;
		public Material[] materials;
		//public IRenderer renderer = new TerrainRenderer ();
		public IRenderer renderer = new MarchRenderer ();
		private Grid<Chunk> grid = new Grid<Chunk> ();
		private Profiler profiler = new Profiler ();
		private int levelY = 50;

		void OnGUI ()
		{
				GUILayout.Label ("Memory used: " + (System.GC.GetTotalMemory (false) / (1024 * 1024)) + "\n" +
				"Grid size: x " + grid.GetMinX () + "/" + grid.GetMaxX () + " z " + grid.GetMinZ () + "/" + grid.GetMaxZ () + "\n" +
				"current Z level: " + levelY);
		}

		void Awake ()
		{
				renderer.Initialize ();
		}

		void Update ()
		{
				if (Input.GetKey (KeyCode.E)) {
						levelY++;
				}
				if (Input.GetKey (KeyCode.Q)) {
						levelY--;
				}
				Chunk nearestEmptyChunk = NearestEmptyChunk ();
				if (nearestEmptyChunk != null) {
						GenerateChunk (nearestEmptyChunk);						
						nearestEmptyChunk.Object.MakeDirty ();
				}
		}

		private Chunk NearestEmptyChunk ()
		{
				Vector3 centre = mainCamera.transform.position;
				Vector3? near = null;
				for (int x = (int)centre.x - sightRadius; x < (int)centre.x + sightRadius; x++) {
						for (int z = (int)centre.z - sightRadius; z < (int)centre.z + sightRadius; z++) {
								if (GetChunkWorldPos (x, z).generated)
										continue;
								Vector3 current = new Vector3 (x, 0, z);
								float distance = Vector3.Distance (centre, current);
								if (distance > sightRadius * sightRadius)
										continue;
								if (!near.HasValue) {
										near = current;
								} else {
										float _distance = Vector3.Distance (centre, near.Value);
										if (distance < _distance)
												near = current;
								}
						}
				}
				if (near.HasValue) {
						return GetChunkWorldPos ((int)near.Value.x, (int)near.Value.z);
				}
				return null;
		}

		public Block GetBlock (int x, int y, int z)
		{
				Chunk chunk = GetChunkWorldPos (x, z);
				if (chunk.generated) {
						return chunk.GetBlockWorldPos (x, y, z);
				}
				return null;
		}

		public void SetBlock (Block block, int x, int y, int z)
		{
				Chunk chunk = GetChunkWorldPos (x, z);
				if (chunk.generated) {
						chunk.SetBlockWorldPos (block, x, y, z);
				}
		}

		public void DelBlock (int x, int y, int z)
		{
				Chunk chunk = GetChunkWorldPos (x, z);
				if (chunk.generated) {
						chunk.DelBlockWorldPos (x, y, z);
				}
		}

		public Chunk GetChunkWorldPos (int x, int z)
		{
				if (x < 0)
						x = (0 - ((ChunkX - x - 1) / ChunkX));
				else
						x = x / ChunkX;
				if (z < 0)
						z = (0 - ((ChunkZ - z - 1) / ChunkZ));
				else
						z = z / ChunkZ;
				return GetChunk (x, z);
		}

		public Chunk GetChunk (int x, int z)
		{
				Chunk chunk = grid.SafeGet (x, z);
				if (chunk == null) {
						chunk = new Chunk (this, x, z);
						grid.AddOrReplace (chunk, x, z);
				}
				return chunk;
		}
		// TODO fix update mechanism
		public void UpdateBlockWorldPos (int x, int y, int z)
		{
				if (GetBlock (x + 1, y, z) != null) {
						GetBlock (x + 1, y, z).Update (this, x, y, z);
				}
				if (GetBlock (x, y + 1, z) != null) {
						GetBlock (x, y + 1, z).Update (this, x, y, z);
				}
				if (GetBlock (x, y, z + 1) != null) {
						GetBlock (x, y, z + 1).Update (this, x, y, z);
				}
				if (GetBlock (x - 1, y, z) != null) {
						GetBlock (x - 1, y, z).Update (this, x, y, z);
				}
				if (GetBlock (x, y - 1, z) != null) {
						GetBlock (x, y - 1, z).Update (this, x, y, z);
				}
				if (GetBlock (x, y, z - 1) != null) {
						GetBlock (x, y, z - 1).Update (this, x, y, z);
				}
		}

		public void RefreshChunkWorldPos (int x, int z)
		{
				Chunk chunk = GetChunkWorldPos (x, z);
				chunk.Object.MakeDirty ();
				if (chunk.WorldToLocateX (x) == 0) {
						GetChunk (chunk.X - 1, chunk.Z).Object.MakeDirty ();
				}
				if (chunk.WorldToLocateX (x) == ChunkX - 1) {
						GetChunk (chunk.X + 1, chunk.Z).Object.MakeDirty ();
				}
				if (chunk.WorldToLocateZ (z) == 0) {
						GetChunk (chunk.X, chunk.Z - 1).Object.MakeDirty ();
				}
				if (chunk.WorldToLocateZ (x) == ChunkZ - 1) {
						GetChunk (chunk.X, chunk.Z + 1).Object.MakeDirty ();
				}
		}

		public int ChunkX {
				get{ return Mathf.RoundToInt (chunkSize.x); }
		}

		public int ChunkY {
				get{ return Mathf.RoundToInt (chunkSize.y); }
		}

		public int ChunkZ {
				get{ return Mathf.RoundToInt (chunkSize.z); }
		}

		public int LevelY {
				get{ return levelY; }
		}

		private void GenerateChunk (Chunk chunk)
		{
				if (chunk.generated)
						return;
				TerrainGenerator.generate (this, chunk);
				chunk.generated = true;
		}
}
