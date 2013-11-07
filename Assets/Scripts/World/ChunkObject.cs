using UnityEngine;
using System.Collections;

public class ChunkObject : MonoBehaviour
{
		private World world;
		private Chunk chunk;
		private MeshFilter meshFilter;
		private MeshCollider meshCollider;
		private bool dirty;
		// material animation stuff
		private float offset;
		private bool add = true;
		private int lastLevelY;

		public static ChunkObject Instance (World world, Chunk chunk)
		{
				GameObject gameObject = new GameObject ("Chunk @ (" + chunk.WorldX + "," + chunk.WorldZ + ")");
				gameObject.transform.parent = world.transform;
				gameObject.transform.position = new Vector3 (chunk.WorldX, 0, chunk.WorldZ);
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.AddComponent<MeshRenderer> ().sharedMaterials = world.materials;
				gameObject.renderer.castShadows = true;
				gameObject.renderer.receiveShadows = true;
				ChunkObject chunkObject = gameObject.AddComponent<ChunkObject> ();
				chunkObject.Initialize (world, chunk, gameObject.AddComponent<MeshFilter> (), gameObject.AddComponent<MeshCollider> ());
				return chunkObject;
		}

		public void Initialize (World world, Chunk chunk, MeshFilter meshFilter, MeshCollider meshCollider)
		{
				this.meshFilter = meshFilter;
				this.meshCollider = meshCollider;
				this.chunk = chunk;
				this.world = world;
				this.lastLevelY = world.LevelY;
		}

		public void Update ()
		{				
				if (chunk.NeighboursReady () &&
				    (this.dirty ||
				    (this.lastLevelY != world.LevelY && renderer.isVisible))) {					
						meshFilter.sharedMesh = RenderMesh ();
						meshCollider.sharedMesh = null;
						meshCollider.sharedMesh = meshFilter.sharedMesh;
						this.lastLevelY = world.LevelY;
						this.dirty = false;
				}

				
				
				// TODO Material animations
				/*
				if (add) {
						offset += Time.deltaTime * 0.05f;
						if (offset >= (1f / 8f)) {
								add = false;
						}
				} else {
						offset -= Time.deltaTime * 0.05f;
						if (offset <= 0) {
								add = true;
						}
				}
				renderer.materials [1].mainTextureOffset = new Vector2 (0, offset);
				*/				
		}

		private Mesh RenderMesh ()
		{
				world.renderer.Render (world, chunk);
				return world.renderer.ToMesh (meshFilter.sharedMesh);
				//MarchRenderer.Instance.Render (world, chunk);
				//return MarchRenderer.Instance.ToMesh (meshFilter.sharedMesh);
				//TerrainRenderer.Instance.Render (world, chunk);
				//return TerrainRenderer.Instance.ToMesh (meshFilter.sharedMesh);
		}

		public void MakeDirty ()
		{
				this.dirty = true;
		}
}
