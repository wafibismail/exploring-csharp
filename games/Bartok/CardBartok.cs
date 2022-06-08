﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CBState includes both states for the game and to... states for movement
//   the to... states represent a CardBartok as it animates toward one of those states
public enum CBState {
	toDrawpile,
	drawpile,
	toHand,
	hand,
	toTarget,
	target,
	discard,
	to,
	idle
}

public class CardBartok : Card {
	static public float MOVE_DURATION = 0.5f;
	static public string MOVE_EASING = Easing.InOut;
	static public float CARD_HEIGHT = 3.5f;
	static public float CARD_WIDTH = 2f;

	[Header("Set Dynamically: CardBartok")]
	public CBState state = CBState.drawpile;

	// Fields to store info the card will use to move and rotate
	public List<Vector3> bezierPts;
	public List<Quaternion> bezierRots;
	public float timeStart, timeDuration;

	// When the card is done moving, it will call reportFinishTo
	public GameObject reportFinishTo = null;

	// MoveTo tells the card to interpolate to a new position and rotation
	public void MoveTo(Vector3 ePos, Quaternion eRot) {
		// Make new interpolation lists for the card
		// Position and Rotation will each have only two points.
		bezierPts = new List<Vector3>();
		bezierPts.Add (transform.localPosition); // Current position
		bezierPts.Add (ePos); // New position

		bezierRots = new List<Quaternion> ();
		bezierRots.Add (transform.rotation); // Current rotation
		bezierRots.Add (eRot); // New Rotation

		if (timeStart == 0) {
			timeStart = Time.time;
			// Don't overwrite timeStart if it isn't 0
			// This configuration allows us to stagger the timing of various
			//   card animation
		}

		// timeDuration always starts the same but can be overwritten later
		timeDuration = MOVE_DURATION;

		state = CBState.to;
	}

	public void MoveTo(Vector3 ePos) {
		MoveTo(ePos, Quaternion.identity);
	}

	void Update() {
		switch (state) {
		case CBState.toHand:
		case CBState.toTarget:
		case CBState.toDrawpile:
		case CBState.to:
			float u = (Time.time - timeStart) / timeDuration;
			float uC = Easing.Ease (u, MOVE_EASING);

			if (u < 0) {
				transform.localPosition = bezierPts [0];
				transform.rotation = bezierRots [0];
				return;
			} else if (u >= 1) {
				uC = 1;

				// Move from the to... state to the proper next state
				if (state == CBState.toHand)
					state = CBState.hand;
				if (state == CBState.toTarget)
					state = CBState.target;
				if (state == CBState.toDrawpile)
					state = CBState.drawpile;
				if (state == CBState.idle)
					state = CBState.idle;

				// Move to the final position
				transform.localPosition = bezierPts [bezierPts.Count - 1];
				transform.rotation = bezierRots [bezierRots.Count - 1];

				// Reset the timeStart to 0 so it gets overwritten next time
				timeStart = 0;

				if (reportFinishTo != null) {
					reportFinishTo.SendMessage ("CBCallback", this);
					reportFinishTo = null; // prevent reporting to the same GO
				} else { // If there is nothing to callback
					// Just let it stay still.
				}
			} else { // Normal interpolation behaviour (0 <= u < 1)
				Vector3 pos = Utils.Bezier (uC, bezierPts);
				transform.localPosition = pos;
				Quaternion rotQ = Utils.Bezier (uC, bezierRots);
				transform.rotation = rotQ;
			}
			break;
		}
	}
}
