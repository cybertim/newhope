using UnityEngine;
using System.Collections;

public class Grid<T>
{

		private T[,] grid;
		private int minX, minZ;
		private int maxX, maxZ;

		public Grid ()
		{
				grid = new T[0, 0];
		}

		public void Set (T obj, int x, int z)
		{
				grid [z - minZ, x - minX] = obj;
		}

		public T Get (int x, int z)
		{
				return grid [z - minZ, x - minX];
		}

		public T SafeGet (int x, int z)
		{
				if (!IsCorrectIndex (x, z))
						return default(T);
				return grid [z - minZ, x - minX];
		}

		public void AddOrReplace (T obj, int x, int z)
		{
				int dMinX = 0, dMinY = 0, dMinZ = 0;
				int dMaxX = 0, dMaxY = 0, dMaxZ = 0;

				if (x < minX)
						dMinX = x - minX;
				if (z < minZ)
						dMinZ = z - minZ;

				if (x >= maxX)
						dMaxX = x - maxX + 1;
				if (z >= maxZ)
						dMaxZ = z - maxZ + 1;

				if (dMinX != 0 || dMinZ != 0 || 
						dMaxX != 0 || dMaxZ != 0) {
						Increase (dMinX, dMinZ, 
						          dMaxX, dMaxZ);
				}

				grid [z - minZ, x - minX] = obj;
		}

		private void Increase (int dMinX, int dMinZ, 
		                       int dMaxX, int dMaxZ)
		{
				int oldMinX = minX;
				int oldMinZ = minZ;

				int oldMaxX = maxX;
				int oldMaxZ = maxZ;

				T[,] oldGrid = grid;

				minX += dMinX;
				minZ += dMinZ;

				maxX += dMaxX;
				maxZ += dMaxZ;

				int sizeX = maxX - minX;
				int sizeZ = maxZ - minZ;
				grid = new T[sizeZ, sizeX];

				for (int z=oldMinZ; z<oldMaxZ; z++) {
						for (int x=oldMinX; x<oldMaxX; x++) {
								grid [z - minZ, x - minX] = oldGrid [z - oldMinZ, x - oldMinX];
						}
				}
		}

		public bool IsCorrectIndex (int x, int z)
		{
				if (x < minX || z < minZ)
						return false;
				if (x >= maxX || z >= maxZ)
						return false;
				return true;
		}

		public int GetMinX ()
		{
				return minX;
		}

		public int GetMinZ ()
		{
				return minZ;
		}

		public int GetMaxX ()
		{
				return maxX;
		}

		public int GetMaxZ ()
		{
				return maxZ;
		}
}