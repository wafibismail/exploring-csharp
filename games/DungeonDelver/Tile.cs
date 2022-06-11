using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	[Header("Set Dynamically")]
	public int x;
	public int y;
	public int tileNum;

	private BoxCollider bColl;

	void Awake() {
		bColl = GetComponent<BoxCollider> ();
	}

	public void SetTile(int eX, int eY, int eTileNum = -1) {
		x = eX;
		y = eY;
		transform.localPosition = new Vector3 (x, y, 0);
		gameObject.name = x.ToString ("D3") + y.ToString ("D3");
		// D3 means decimal (base-ten) with at least 3 characters e.g. 005

		if (eTileNum == -1) {
			eTileNum = TileCamera.GET_MAP (x, y); // or = TileCamera.MAP[x, y];
		} else {
			TileCamera.SET_MAP (x, y, eTileNum);
		}
		tileNum = eTileNum;
		GetComponent<SpriteRenderer> ().sprite = TileCamera.SPRITES [tileNum];

		SetCollider ();
	}

	// Arrange the collider for this tile
	void SetCollider() {
		bColl.enabled = true;
		char c = TileCamera.COLLISIONS [tileNum];
		switch (c) {
		case 'S': // Whole
			bColl.center = Vector3.zero;
			bColl.size = Vector3.one;
			break;
		case 'W': // Top
			bColl.center = new Vector3 (0, 0.25f, 0);
			bColl.size = new Vector3 (1, 0.5f, 1);
			break;
		case 'A': // Left
			bColl.center = new Vector3 (-0.25f, 0, 0);
			bColl.size = new Vector3 (0.5f, 1, 1);
			break;
		case 'D': // right
			bColl.center = new Vector3 (0.25f, 0, 0);
			bColl.size = new Vector3 (0.5f, 1, 1);
			break;

			// There are other settings as well not used in this prototype
			// Skipped for now as they aren't required, but may be useful
			//   for my own projects if I'm reusing these codes
			//   Refer to page 986

		default: // Handle '_' collision chars
			bColl.enabled = false;
			break;
		}
	}
}
