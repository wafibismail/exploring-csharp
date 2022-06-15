﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSwap {
	public int tileNum;
	public GameObject swapPrefab;
	public GameObject guaranteedItemDrop;
	public int overrideTileNum = -1;
}

public class TileCamera : MonoBehaviour {
	static private int W, H;
	static private int[,] MAP;
	static public Sprite[] SPRITES;
	static public Transform TILE_ANCHOR;
	static public Tile[,] TILES;
	static public string COLLISIONS;

	[Header("Set in Inspector")]
	public TextAsset mapData;
	public Texture2D mapTiles;
	public TextAsset mapCollisions; // This will be used later
	public Tile tilePrefab;
	public int defaultTileNum;
	public List<TileSwap> tileSwaps;

	private Dictionary<int, TileSwap> tileSwapDict;
	private Transform enemyAnchor, itemAnchor;

	void Awake() {
		COLLISIONS = Utils.RemoveLineEndings (mapCollisions.text);
		// Now, COLLISIONS stores a 256-char string with no line breaks
		PrepareTileSwapDict();
		enemyAnchor = (new GameObject ("Enemy Anchor")).transform;
		itemAnchor = (new GameObject ("Item Anchor")).transform;
		LoadMap ();
	}

	public void LoadMap() {
		// Create the TILE_ANCHOR. All Tiles will have this as their parent.
		GameObject go = new GameObject("TILE_ANCHOR");
		TILE_ANCHOR = go.transform;

		// load all of the Sprites from mapTiles
		SPRITES = Resources.LoadAll<Sprite>(mapTiles.name);

		// Read in the map data
		string[] lines = mapData.text.Split('\n');
		H = lines.Length;
		string[] tileNums = lines[0].Split(' ');
		W = tileNums.Length;

		System.Globalization.NumberStyles hexNum;
		hexNum = System.Globalization.NumberStyles.HexNumber; // for parsing the hex num
		// Place the map data into a 2D Array for faster access
		MAP = new int[W,H];
		for (int j = 0; j < H; j++) {
			tileNums = lines[j].Split(' ');
			for( int i = 0; i < W; i++) {
				if (tileNums[i] == "..") {
					MAP[i,j] = 0;
				} else {
					MAP[i,j] = int.Parse(tileNums[i], hexNum);
				}
				CheckTileSwaps (i, j);
			}
		}
		print("Parsed " + SPRITES.Length + "sprites.");
		print("Map size: " + W + "wide by " + H + "high");

		ShowMap (); // Put the Tiles into the scene
	}

	/// <summary>
	/// Generates Tiles for the entire map all at once.
	/// </summary>
	void ShowMap() {
		TILES = new Tile[W, H];

		// Run through the entire map and instantiate Tiles where necessary
		for (int j = 0; j < H; j++) {
			for (int i = 0; i < W; i++) {
				if (MAP [i, j] != 0) {
					Tile ti = Instantiate<Tile> (tilePrefab);
					ti.transform.SetParent (TILE_ANCHOR);
					// Omitting the eTileNum in SetTIle
					// causes TileCamera.MAP tileNums to be used
					ti.SetTile (i, j);
					TILES [i, j] = ti;
				}
			}
		}
	}

	void PrepareTileSwapDict() {
		tileSwapDict = new Dictionary<int, TileSwap> ();
		foreach (TileSwap ts in tileSwaps) {
			tileSwapDict.Add (ts.tileNum, ts);
		}
	}

	void CheckTileSwaps(int i, int j) {
		int tNum = GET_MAP (i, j);
		if (!tileSwapDict.ContainsKey (tNum))
			return;

		// We do need to swap a tile
		TileSwap ts = tileSwapDict[tNum];
		if (ts.swapPrefab != null) {
			GameObject go = Instantiate (ts.swapPrefab);
			Enemy e = go.GetComponent<Enemy> ();
			if (e != null) {
				go.transform.SetParent (enemyAnchor);
			} else {
				go.transform.SetParent (itemAnchor);
			}
			go.transform.position = new Vector3 (i, j, 0);
			if (ts.guaranteedItemDrop != null) {
				if (e != null) {
					e.guaranteedItemDrop = ts.guaranteedItemDrop;
				}
			}
		}
		// Replace with another tile
		if (ts.overrideTileNum == -1) {
			SET_MAP (i, j, defaultTileNum);
		} else {
			SET_MAP (i, j, ts.overrideTileNum);
		}
	}

	/*
	 * GET_MAP & SET_MAP provide protected access to get and set MAP while preventing
	 * things like IndexOutOfRangeExceptions
	 */

	static public int GET_MAP(int x, int y) {
		if (x < 0 || x >= W || y < 0 || y >= H) {
			return -1; // Do not allow IndexOutOfRangeExceptions
		}
		return MAP[x,y];
	}

	static public int GET_MAP(float x, float y) { // A float GET_MAP() overload
		int tX = Mathf.RoundToInt(x);
		int tY = Mathf.RoundToInt(y - 0.25f);
		// The y - 0.25f accounts for the forced percpective in this game
		// where the player can have the top half of their body outside of a
		// tile and still be considered to be walking on that tile.
		return GET_MAP (tX, tY);
	}

	static public void SET_MAP(int x, int y, int tNum) {
		// Additional security or a break point could be set here.
		if ( x < 0 || x >= W || y < 0 || y >= H) {
			return; // Do not allow IndexOutOfRangeExceptions
		}
		MAP [x, y] = tNum;
	}
}
