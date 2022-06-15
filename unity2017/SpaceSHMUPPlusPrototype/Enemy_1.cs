﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy {

	[Header("Set in Inspector: Enemy_1")]
	// # seconds for a full sine wave
	public float waveFrequency = 2;
	// sine wave width in meters
	public float waveWidth = 4;
	public float waveRotY = 45;

	private float x0; // initial x value of pos
	private float birthTime;

	// Start works well because it's not used by the Enemy superclass
	void Start() {
		x0 = pos.x;
		birthTime = Time.time;
	}

	public override void Move() {
		Vector3 tempPos = pos;
		// Theta adjusts base on time
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin (theta);
		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;

		// Rotate a bit about y
		Vector3 rot = new Vector3(0, sin*waveRotY, 0);
		this.transform.rotation = Quaternion.Euler (rot);

		// base.Move() still handles the movement down in y
		base.Move();
	}
}
