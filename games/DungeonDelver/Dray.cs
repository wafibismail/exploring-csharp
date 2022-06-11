﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover {

	public enum eMode { idle, move, attack, transition }
	[Header("Set in Inspector")]
	public float speed = 5;
	public float attackDuration = 0.25f; // Number of seconds to attack
	public float attackDelay = 0.5f; // Delay between attacks
	public float transitionDelay = 0.5f; // Room transition delay

	[Header("Set Dynamically")]
	public int dirHeld = -1; // Direction of the held movement key
	public int facing = 1; // Direction Dray is facing
	public eMode mode = eMode.idle;

	private float timeAtkDone = 0; // Time at which the attack animation should be done
	private float timeAtkNext = 0; // Time at which Dray will be able to attack again
	private float transitionDone = 0;
	private Vector2 transitionPos;

	private Rigidbody rigid;
	private Animator anim;
	private InRoom inRm;
	private Vector3[] directions = new Vector3[] {
		Vector3.right, Vector3.up, Vector3.left, Vector3.down	
	};
	private KeyCode[] keys = new KeyCode[] {
		KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow
	};

	void Awake(){
		rigid = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		inRm = GetComponent<InRoom> ();
	}

	void Update() {
		if (mode == eMode.transition) {
			rigid.velocity = Vector3.zero;
			anim.speed = 0;
			roomPos = transitionPos;
			if (Time.time < transitionDone)
				return;
			// The following line is only reached if Time.time >= transitionDone
			mode = eMode.idle;
		}

		// Handle keyboard inputs and manage eDrayModes

		dirHeld = -1;
		for (int i = 0; i < 4; i++) {
			if (Input.GetKey (keys [i]))
				dirHeld = i;
		}

		// Pressing the attack button(s)
		if (Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext) {
			mode = eMode.attack;
			timeAtkDone = Time.time + attackDuration;
			timeAtkNext = Time.time + attackDelay;
		}

		// Finishing the attack when it's over
		if (Time.time >= timeAtkDone) {
			mode = eMode.idle;
		}

		// Choosing the proper mode if we're not attacking
		if (mode != eMode.attack) {
			if (dirHeld == -1) {
				mode = eMode.idle;
			} else {
				facing = dirHeld;
				// having "facing" exclusively set during moving ensures Dray's
				// facing direction is consistent when attacking or standing still
				mode = eMode.move;
			}
		}

		// Act on the current mode

		Vector3 vel = Vector3.zero;

		switch (mode) {
		case eMode.attack:
			anim.CrossFade ("Dray_Attack_" + facing, 0);
			anim.speed = 0;
			break;

		case eMode.idle:
			anim.CrossFade ("Dray_Walk_" + facing, 0);
			anim.speed = 0;
			break;

		case eMode.move:
			vel = directions [dirHeld];
			anim.CrossFade ("Dray_Walk_" + facing, 0);
			anim.speed = 1;
			break;
		}
			
		rigid.velocity = vel * speed;
	}

	void LateUpdate() {
		// Get the half-grid location of this GameObject
		//   This setting is forced here because
		//   InRoom.DOORS are on a half-grid
		Vector2 rPos = GetRoomPosOnGrid(0.5f);

		// Check to see whether we're in a Door tile
		int doorNum;
		for (doorNum = 0; doorNum < 4; doorNum++) {
			if (rPos == InRoom.DOORS[doorNum]) {
				break;
			}
		}

		if (doorNum > 3 || doorNum != facing)
			return;

		// Move to the next room
		Vector2 rm = roomNum;
		switch (doorNum) {
		case 0:
			rm.x += 1;
			break;
		case 1:
			rm.y += 1;
			break;
		case 2:
			rm.x -= 1;
			break;
		case 3:
			rm.y -= 1;
			break;
		}

		// Make sure that the rm we want to jump to is valid
		if (rm.x>=0 && rm.x <= InRoom.MAX_RM_X) {
			if (rm.y >= 0 && rm.y <= InRoom.MAX_RM_Y) {
				roomNum = rm;
				// Switch position to the opposite end of the door entered
				transitionPos = InRoom.DOORS [(doorNum + 2) % 4];
				roomPos = transitionPos;
				mode = eMode.transition;
				transitionDone = Time.time + transitionDelay;
			}
		}
	}

	// Implementation of IFacingMover
	public int GetFacing() {
		return facing;
	}

	public bool moving {
		get { return (mode == eMode.move); }
	}

	public float GetSpeed() {
		return speed;
	}

	public float gridMult {
		get { return inRm.gridMult; }
	}

	public Vector2 roomPos {
		get { return inRm.roomPos; }
		set { inRm.roomPos = value; }
	}

	public Vector2 roomNum {
		get { return inRm.roomNum; }
		set { inRm.roomNum = value; }
	}

	public Vector2 GetRoomPosOnGrid(float mult = -1) {
		return inRm.GetRoomPosOnGrid (mult);
	}
}
