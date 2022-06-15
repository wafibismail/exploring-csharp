﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The Scoreboard class manages showing the score to the player
public class Scoreboard : MonoBehaviour {
	public static Scoreboard S;

	[Header("Set in Inspector")]
	public GameObject prefabFloatingScore;

	[Header("Set Dynamically")]
	[SerializeField] private int _score = 0;
	[SerializeField] private string _scoreString;

	private Transform canvasTrans;

	// The score property also sets the scoreString
	public int score {
		get { return (_score); }
		set {
			_score = value;
			scoreString = _score.ToString ("N0");
		}
	}

	// The scoreString property also sets the Text.text
	public string scoreString {
		get { return (_scoreString); }
		set {
			_scoreString = value;
			GetComponent<Text> ().text = _scoreString;
		}
	}

	void Awake() {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("ERROR: Scoreboard.Awake(): S is already set!");
		}
		canvasTrans = transform.parent;
	}

	// When called by SendMessage, this adds the fs.score to this.score
	public void FSCallback(FloatingScore fs) {
		score += fs.score;
	}

	// Instantiates a FloatingScore GameObject and returns a pointer to it
	public FloatingScore CreateFloatingScore(int amt, List<Vector2> pts) {
		GameObject go = Instantiate<GameObject> (prefabFloatingScore);
		go.transform.SetParent (canvasTrans);
		FloatingScore fs = go.GetComponent<FloatingScore> ();
		fs.score = amt;
		fs.reportFinishTo = this.gameObject; // Set fs to call back to this
		fs.Init(pts);
		return fs;
	}
}
