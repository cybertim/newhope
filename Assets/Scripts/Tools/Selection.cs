using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour
{
		public GameObject selection;
		private World world;
		private Vector3? start = null;
		private GameObject gameObject = null;

		void Awake ()
		{
				world = GameObject.Find ("World").GetComponent<World> ();
		}

		private Vector3? MouseToWorld (Vector3 v)
		{
				Vector3? s = null;
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (v);
				if (Physics.Raycast (ray, out hit, 99f)) {
						s = hit.point - hit.normal / 2;								
				}
				return s;						
		}

		private struct SelectionArea
		{
				public int startX, startZ, endX, endZ;

				public SelectionArea (float sx, float sz, float ex, float ez)
				{
						if (sx > ex) {
								endX = Mathf.FloorToInt (sx);
								startX = Mathf.FloorToInt (ex);
						} else {
								startX = Mathf.FloorToInt (sx);
								endX = Mathf.FloorToInt (ex);
						}
						if (sz > ez) {
								endZ = Mathf.CeilToInt (sz);
								startZ = Mathf.CeilToInt (ez);
						} else {
								startZ = Mathf.CeilToInt (sz);
								endZ = Mathf.CeilToInt (ez);
						}
				}
		}

		void Update ()
		{
				if (gameObject != null)
						GameObject.Destroy (gameObject);
				if (Input.GetMouseButtonDown (0)) {
						start = MouseToWorld (Input.mousePosition);
				}
				if (Input.GetMouseButton (0)) {
						gameObject = new GameObject ("Selection @ " + Mathf.RoundToInt (start.Value.x) + ", " + Mathf.RoundToInt (start.Value.z));
						if (start.HasValue) {
								Vector3? end = MouseToWorld (Input.mousePosition);
								if (end.HasValue) {
										// loop through all stuff between two oints on same height
										SelectionArea selectionArea = new SelectionArea (start.Value.x, start.Value.z, end.Value.x, end.Value.z);
										for (int x = selectionArea.startX; x <= selectionArea.endX; x++) {
												for (int z = selectionArea.startZ; z <= selectionArea.endZ; z++) {
														// for debugging visual
														GameObject o = (GameObject)Instantiate (selection, new Vector3 (x, Mathf.Floor (start.Value.y), z), Quaternion.identity);
														o.transform.parent = gameObject.transform;
												}
										}
								}
						}
				}
				if (Input.GetMouseButtonUp (0)) {

						Vector3? end = MouseToWorld (Input.mousePosition);
						if (end.HasValue) {
								SelectionArea selectionArea = new SelectionArea (start.Value.x, start.Value.z, end.Value.x, end.Value.z);
								for (int x = selectionArea.startX; x <= selectionArea.endX; x++) {
										for (int z = selectionArea.startZ; z <= selectionArea.endZ; z++) {
												// TEST to see if the targets are mutable
												//DeleteBlock (x, Mathf.FloorToInt (start.Value.y), z);
												// enable selected status
										}
								}
						}
						start = null;
				}
		}

		private void DeleteBlock (int x, int y, int z)
		{
				world.DelBlock (x, y, z);
				Chunk chunk = world.GetChunkWorldPos (x, z);
				world.RefreshChunkWorldPos (x, z);
		}
}
