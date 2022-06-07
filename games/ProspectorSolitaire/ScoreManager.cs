﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An enum to handle all possible scoring events
public enum eScoreEvent {
	draw,
	mine,
	mineGold,
	gameWin,
	gameLoss
}

// ScoreManager handles all of the scoring
public class ScoreManager : MonoBehaviour {
	static private ScoreManager S;

	static public int SCORE_FROM_PREV_ROUND = 0;
	static public int HIGH_SCORE = 0;

	[Header("Set Dynamically")]
	// Fields to track score info
	public int chain = 0;
	public int scoreRun = 0;
	public int score = 0;

	void Awake() {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("ERROR: ScoreManager.Awake(): S is already set!");
		}

		// Check for a high score in PlayerPrefs
		if(PlayerPrefs.HasKey("ProspectorHighScore")) {
			HIGH_SCORE = PlayerPrefs.GetInt ("ProspectorHighScore");
		}

		score += SCORE_FROM_PREV_ROUND;

		SCORE_FROM_PREV_ROUND = 0;
	}

	static public void EVENT(eScoreEvent evt) {
		try { // Stops an error from breaking the program
			S.Event(evt);
		} catch (System.NullReferenceException nre) {
			Debug.LogError("ScoreManager:EVENT() called while S=null.\n"+nre);
		}
	}

	void Event(eScoreEvent evt) {
		switch (evt) {
		// Same things need to happen whether it's a draw, win, or loss
		case eScoreEvent.draw: // Drawing a card
		case eScoreEvent.gameWin: // Win the round
		case eScoreEvent.gameLoss: // Lose the round
			chain = 0; // Resets the chain
			score += scoreRun; // Add scoreRun to total score
			scoreRun = 0; // reset scoreRun
			break;

		case eScoreEvent.mine: // Remove a mine card
			chain++; // Increase the score chain
			scoreRun += chain; // Add score for this card to run
			break;
		}

		// This second switch statement handles round wins and losses
		switch (evt) {
		case eScoreEvent.gameWin:
			// Score to be added to the next round
			// Static fields are not reset by SceneManager.LoadScene()
			SCORE_FROM_PREV_ROUND = score;
			print ("You won this round! Round score: " + score);
			break;

		case eScoreEvent.gameLoss:
			if (HIGH_SCORE <= score) {
				print ("You got the high score! High score: " + score);
				HIGH_SCORE = score;
				PlayerPrefs.SetInt ("ProspectorHighScore", score);
			} else {
				print ("Your final score for the game was: " + score);
			}
			break;

		default:
			print ("Score:" + score + " scoreRun:" + scoreRun + " chain:" + chain);
			break;
		}
	}

	static public int CHAIN { get { return S.chain; } }
	static public int SCORE { get { return S.score; } }
	static public int SCORE_RUN { get { return S.scoreRun; } }
}
