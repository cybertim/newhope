using UnityEngine;
using System.Collections;

public class Chunk
{

		public bool generated;
		private Block[,,] blocks;
		private World world;
		private int chunkX, chunkZ;
		private ChunkObject chunkObject;

		public Chunk (World world, int chunkX, int chunkZ)
		{
				this.chunkX = chunkX;
				this.chunkZ = chunkZ;
				this.world = world;
				blocks = new Block[world.ChunkX, world.ChunkY, world.ChunkZ];
		}

		public void SetBlockWorldPos (Block block, int x, int y, int z)
		{
				SetBlock (block, WorldToLocateX (x), y, WorldToLocateZ (z));
		}

		public void SetBlock (Block block, int x, int y, int z)
		{				
				blocks [x, y, z] = block;
		}

		public void DelBlockWorldPos (int x, int y, int z)
		{
				DelBlock (WorldToLocateX (x), y, WorldToLocateZ (z));
		}

		public void DelBlock (int x, int y, int z)
		{				
				blocks [x, y, z] = null;
		}

		public Block GetBlockWorldPos (int x, int y, int z)
		{				
				return GetBlock (WorldToLocateX (x), y, WorldToLocateZ (z));
		}

		public Block GetBlock (int x, int y, int z)
		{
				return blocks [x, y, z];
		}

		public int WorldToLocateZ (int z)
		{
				int iz = z % world.ChunkZ;
				if (iz < 0) 
						iz = world.ChunkZ + iz;
				return iz;
		}

		public int WorldToLocateX (int x)
		{
				int ix = x % world.ChunkX;
				if (ix < 0) 
						ix = world.ChunkX + ix;						
				return ix;
		}

		public int X {
				get{ return chunkX;}
		}

		public int Z {
				get{ return chunkZ;}
		}

		public int WorldX {
				get{ return chunkX * world.ChunkX;}
		}

		public int WorldZ {
				get{ return chunkZ * world.ChunkZ;}
		}

		public ChunkObject Object {
				get {
						if (chunkObject == null) {
								chunkObject = ChunkObject.Instance (world, this);
						}
						return chunkObject;
				}
		}

		public bool NeighboursReady ()
		{
				if (world.GetChunk (chunkX - 1, chunkZ).generated
						&& world.GetChunk (chunkX, chunkZ - 1).generated
						&& world.GetChunk (chunkX + 1, chunkZ).generated
						&& world.GetChunk (chunkX, chunkZ + 1).generated
						&& world.GetChunk (chunkX + 1, chunkZ + 1).generated
						&& world.GetChunk (chunkX - 1, chunkZ - 1).generated
						&& world.GetChunk (chunkX + 1, chunkZ - 1).generated
						&& world.GetChunk (chunkX - 1, chunkZ + 1).generated)
						return true;
				return false;
		}
}
