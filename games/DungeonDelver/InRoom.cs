﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoom : MonoBehaviour {
	static public float ROOM_W = 16;
	static public float ROOM_H = 11;
	static public float WALL_T = 2;

	[Header("Set in Inspector")]
	public bool keepInRoom = true;
	public float gridMult = 1;

	void LateUpdate() {
		if (keepInRoom) {
			Vector2 rPos = roomPos;
			rPos.x = Mathf.Clamp (rPos.x, WALL_T, ROOM_W - 1 - WALL_T);
			rPos.y = Mathf.Clamp (rPos.y, WALL_T, ROOM_H - 1 - WALL_T);
			roomPos = rPos;
		}
	}

	// Location of this character in room coordinates
	//   Allows getting and setting the position
	//     relative to the bottom-left corner of the room
	public Vector2 roomPos {
		get {
			Vector2 tPos = transform.position;
			tPos.x %= ROOM_W;
			tPos.y %= ROOM_H;
			return tPos;
		}
		set {
			Vector2 rm = roomNum;
			rm.x *= ROOM_W;
			rm.y *= ROOM_H;
			rm += value;
			transform.position = rm;
		}
	}

	// Which room is the character in?
	//   Allows getting and setting the room in which the GO is located
	//     while maintaining the relative roomPos
	public Vector2 roomNum {
		get {
			Vector2 tPos = transform.position;
			tPos.x = Mathf.Floor (tPos.x / ROOM_W);
			tPos.y = Mathf.Floor (tPos.y / ROOM_H);
			return tPos;
		}
		set {
			Vector2 rPos = roomPos;
			Vector2 rm = value;
			rm.x *= ROOM_W;
			rm.y *= ROOM_H;
			transform.position = rm + rPos;
		}
	}
}
