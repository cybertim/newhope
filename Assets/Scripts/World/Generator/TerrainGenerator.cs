using UnityEngine;
using System.Collections;

public class TerrainGenerator
{
		public static void generate (World world, Chunk chunk)
		{

				for (int x = 0; x < world.ChunkX; x++) {
						for (int z = 0; z < world.ChunkZ; z++) {

								
								float seed = 0.035f;
								float terrain = 0.01f;
								//float holes = 0.05f;
								float waterlevel = world.ChunkY / 4;
								//int bottom = (int)(SimplexNoise.noise ((chunk.WorldX + x) * terrain, (chunk.WorldZ + z) * terrain) * world.ChunkY / 12) + (world.ChunkY / 4);								
								//int top = (int)(SimplexNoise.noise ((chunk.WorldX + x) * holes, (chunk.WorldZ + z) * holes) * world.ChunkY / 10) + (world.ChunkY / 2) - (world.ChunkY / 10);
								int height = (int)(SimplexNoise.noise ((chunk.WorldX + x) * terrain, (chunk.WorldZ + z) * terrain) * world.ChunkY / 12) + (world.ChunkY / 3);								
								//int baseHeight = world.ChunkY / 4;
								for (int y = 0; y < height; y++) {
										chunk.SetBlock (new Grass (), x, y, z);
								}
								if (height < waterlevel) {
										for (int y = height; y < waterlevel; y++) {
												chunk.SetBlock (new Water (), x, y, z);				
										}
								}
								/*
								for (int y = 0; y < height; y++) {										
										if (y < bottom) {
												chunk.SetBlock (new Element (), x, y, z);
										} else if (y < top && SimplexNoise.noise ((chunk.WorldX + x) * seed, y * seed, (chunk.WorldZ + z) * seed) > 0) {
												//chunk.SetBlock (new Element (), x, y, z);
										} else {
												chunk.SetBlock (new Element (), x, y, z);
										}
								}
								*/
						}
				}
		}
}
