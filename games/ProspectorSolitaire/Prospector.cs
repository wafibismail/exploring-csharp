using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prospector : MonoBehaviour {
	static public Prospector S;

	[Header("Set in Inspector")]
	public TextAsset deckXML;
	public TextAsset layoutXML;
	public float xOffset = 3;
	public float yOffset = -2.5f;
	public Vector3 layoutCenter;
	public Vector2 fsPosMid = new Vector2( 0.5f, 0.90f);
	public Vector2 fsPosRun = new Vector2( 0.5f, 0.75f);
	public Vector2 fsPosMid2 = new Vector2( 0.4f, 1.0f);
	public Vector2 fsPosEnd = new Vector2( 0.5f, 0.95f);
	public float reloadDelay = 2f; // 2 sec between rounds
	public Text gameOverText, roundResultText, highScoreText;

	[Header("Set Dynamically")]
	public Deck deck;
	public Layout layout;
	public List<CardProspector> drawPile;
	public Transform layoutAnchor;
	public CardProspector target;
	public List<CardProspector> tableau;
	public List<CardProspector> discardPile;
	public FloatingScore fsRun;

	void Awake() {
		S = this;
		SetUpUITexts ();
	}

	void SetUpUITexts() {
		// HighScore UI Text
		GameObject go = GameObject.Find("HighScore");
		if (go != null) {
			highScoreText = go.GetComponent<Text> ();
		}
		int highScore = ScoreManager.HIGH_SCORE;
		string hScore = "High Score: " + Utils.AddCommasToNumber (highScore);
		go.GetComponent<Text> ().text = hScore;

		//  UI Texts that show at the end of the round
		go = GameObject.Find("GameOver");
		if (go != null) {
			gameOverText = go.GetComponent<Text> ();
		}

		go = GameObject.Find ("RoundResult");
		if (go != null) {
			roundResultText = go.GetComponent<Text> ();
		}

		// Make these end of round texts invisible
		ShowResultsUI (false);
	}

	void ShowResultsUI(bool show) {
		gameOverText.gameObject.SetActive (show);
		roundResultText.gameObject.SetActive (show);
	}

	void Start() {
		Scoreboard.S.score = ScoreManager.SCORE;

		deck = GetComponent<Deck> (); // Get the Deck
		deck.InitDeck (deckXML.text); // Pass the XML to it

		Deck.Shuffle (ref deck.cards);

		/*
		// Just laying out the cards; Code no longer needed
		Card c;
		for (int cNum = 0; cNum < deck.cards.Count; cNum++) {
			c = deck.cards [cNum];
			c.transform.localPosition = new Vector3 ((cNum % 13) * 3, cNum / 13 * 4, 0);
		}
		*/

		layout = GetComponent<Layout> ();
		layout.ReadLayout(layoutXML.text);

		drawPile = ConvertListCardsToListCardProspectors (deck.cards);
		LayoutGame ();
	}

	List<CardProspector> ConvertListCardsToListCardProspectors(List<Card> lCD) {
		List<CardProspector> lCP = new List<CardProspector> ();
		CardProspector tCP;
		foreach (Card tCD in lCD) {
			// "as" will only work if tCD is already a CardProcpector to begin with
			//   If it is instead an instance of the superclass Card,
			//   "as" would return null
			tCP = tCD as CardProspector;
			lCP.Add (tCP);
		}
		return(lCP);
	}
		
	CardProspector Draw() {
		CardProspector cd = drawPile [0];
		drawPile.RemoveAt (0);
		return cd;
	}

	void LayoutGame() {
		if (layoutAnchor == null) {
			GameObject tGO = new GameObject ("_LayoutAnchor");
			layoutAnchor = tGO.transform;
			layoutAnchor.transform.position = layoutCenter;
		}

		CardProspector cp;

		foreach (SlotDef tSD in layout.slotDefs) {
			cp = Draw ();
			cp.faceUp = tSD.faceUp;
			cp.transform.parent = layoutAnchor;

			cp.transform.localPosition = new Vector3 (
				layout.multiplier.x * tSD.x,
				layout.multiplier.y * tSD.y,
				-tSD.layerID
			);

			cp.layoutID = tSD.id;
			cp.slotDef = tSD;

			cp.state = eCardState.tableau;

			cp.SetSortingLayerName (tSD.layerName);

			tableau.Add (cp);
		}

		// Set which cards are hiding others
		foreach(CardProspector tCP in tableau) {
			foreach (int hid in tCP.slotDef.hiddenBy) {
				cp = FindCardByLayoutID (hid);
				tCP.hiddenBy.Add (cp);
			}
		}

		// Set up the initial target card
		MoveToTarget (Draw ());

		// Set up the Draw pile
		UpdateDrawPile ();
	}

	CardProspector FindCardByLayoutID(int layoutID) {
		foreach (CardProspector tCP in tableau) {
			if (tCP.layoutID == layoutID) {
				return (tCP);
			}
		}
		return (null);
	}

	// This turns cards in the Mine face-up or face-down
	void SetTableauFaces() {
		foreach (CardProspector cd in tableau) {
			bool faceUp = true;
			foreach (CardProspector cover in cd.hiddenBy) {
				// If either of the cards are in the tableau
				if (cover.state == eCardState.tableau) {
					faceUp = false;
				}

			}
			cd.faceUp = faceUp;
		}
	}

	void MoveToDiscard(CardProspector cd) {
		cd.state = eCardState.discard;
		discardPile.Add (cd);
		cd.transform.parent = layoutAnchor;

		// Position it on the discard pile
		cd.transform.localPosition = new Vector3 (
			layout.multiplier.x * layout.discardPile.x,
			layout.multiplier.y * layout.discardPile.y,
			-layout.discardPile.layerID + 0.5f
		);
		cd.faceUp = true;

		// Place it on top of the pile for depth sorting
		cd.SetSortingLayerName(layout.discardPile.layerName);
		cd.SetSortOrder (-100 + discardPile.Count);
	}

	void MoveToTarget(CardProspector cd) {
		if (target != null)
			MoveToDiscard (target);
		target = cd;

		cd.state = eCardState.target;
		cd.transform.parent = layoutAnchor;

		cd.transform.localPosition = new Vector3 (
			layout.multiplier.x * layout.discardPile.x,
			layout.multiplier.y * layout.discardPile.y,
			-layout.discardPile.layerID
		);

		cd.faceUp = true;

		cd.SetSortingLayerName (layout.discardPile.layerName);
		cd.SetSortOrder (0);
	}

	// Arranges all cards of the drawPile to show how many are left
	void UpdateDrawPile() {
		CardProspector cd;

		for (int i = 0; i < drawPile.Count; i++) {
			cd = drawPile [i];
			cd.transform.parent = layoutAnchor;

			// Position it correctly with the layout.drawPile.stagger
			Vector2 dpStagger = layout.drawPile.stagger;
			cd.transform.localPosition = new Vector3 (
				layout.multiplier.x * (layout.drawPile.x + i * dpStagger.x),
				layout.multiplier.y * (layout.drawPile.y + i * dpStagger.y),
				-layout.drawPile.layerID + 0.1f * i
			);

			cd.faceUp = false;
			cd.state = eCardState.drawPile;

			// Set depth sorting
			cd.SetSortingLayerName (layout.drawPile.layerName);
			cd.SetSortOrder (-10 * i);
		}
	}

	public void CardClicked(CardProspector cd) {

		switch (cd.state) {
		case eCardState.target:
			// Nothing happens
			break;
		case eCardState.drawPile:
			// Draw the next card on clicking any card from the drawPile
			MoveToDiscard (target);
			MoveToTarget (Draw ());
			UpdateDrawPile ();
			ScoreManager.EVENT (eScoreEvent.draw);
			FloatingScoreHandler (eScoreEvent.draw);
			break;

		case eCardState.tableau:
			// Check if it's a valid play
			bool validMatch = true;
			if (!cd.faceUp) {
				validMatch = false;
			}
			if (!AdjacentRank (cd, target)) {
				validMatch = false;
			}
			if (!validMatch)
				return;

			tableau.Remove (cd);
			MoveToTarget (cd);
			SetTableauFaces ();
			ScoreManager.EVENT (eScoreEvent.mine);
			FloatingScoreHandler (eScoreEvent.mine);
			break;
		}
		CheckForGameOver ();
	}

	void CheckForGameOver() {
		if (tableau.Count == 0) {
			GameOver (true); // win
			return;
		}

		// drawPile still has cards
		if (drawPile.Count > 0) {
			return;
		}

		// validplay still possible
		foreach (CardProspector cd in tableau) {
			if (AdjacentRank (cd, target)) {
				return;
			}
		}

		GameOver (false); // lose
	}

	void GameOver(bool won) {
		int score = ScoreManager.SCORE;
		if (fsRun != null)
			score += fsRun.score;
		if (won) {
			gameOverText.text = "Round Over";
			roundResultText.text = "You won this round!\nRound Score: " + score;
			ShowResultsUI (true);

			// print ("Game Over. You won! :)");

			ScoreManager.EVENT(eScoreEvent.gameWin);
			FloatingScoreHandler (eScoreEvent.gameWin);
		} else {
			gameOverText.text = "Game Over";
			if (ScoreManager.HIGH_SCORE <= score) {
				string str = "You got the high score!\nHigh Score: " + score;
				roundResultText.text = str;
			} else {
				roundResultText.text = "Your final score was: " + score;
			}
			ShowResultsUI (true);

			// print ("Game over. You lost. :(");

			ScoreManager.EVENT(eScoreEvent.gameLoss);
			FloatingScoreHandler (eScoreEvent.gameLoss);
		}

		// Give the score a moment to travel before reloading
		Invoke ("ReloadLevel", reloadDelay);
	}

	void ReloadLevel() {
		SceneManager.LoadScene ("__Prospector_Scene_0");
	}

	public bool AdjacentRank(CardProspector c0, CardProspector c1) {
		if (!c0.faceUp || !c1.faceUp)
			return (false);

		if (Mathf.Abs (c0.rank - c1.rank) == 1) {
			return (true);
		}

		if (c0.rank == 1 && c1.rank == 13)
			return (true);

		if (c0.rank == 13 && c1.rank == 1)
			return (true);

		return (false);
	}

	void FloatingScoreHandler(eScoreEvent evt) {
		List<Vector2> fsPts;
		switch (evt) {
			// Same things need to happen whether it's a draw, a win, or a loss
		case eScoreEvent.draw:
		case eScoreEvent.gameWin:
		case eScoreEvent.gameLoss:

			if (fsRun != null) {
				fsPts = new List<Vector2> ();
				fsPts.Add (fsPosRun);
				fsPts.Add (fsPosMid2);
				fsPts.Add (fsPosEnd);
				fsRun.reportFinishTo = Scoreboard.S.gameObject;
				fsRun.Init (fsPts, 0, 1);

				// Also adjust the fontSize
				fsRun.fontSizes = new List<float> (new float[] { 28, 36, 4 });
				fsRun = null; // Clear fsRun so it's created again
			}
			break;

		case eScoreEvent.mine: // Remove a mine card
			FloatingScore fs;
			// Move the fs from the mousePosition to fsPosRun
			Vector2 p0 = Input.mousePosition;
			p0.x /= Screen.width;
			p0.y /= Screen.height;
			fsPts = new List<Vector2> ();
			fsPts.Add (p0);
			fsPts.Add (fsPosMid);
			fsPts.Add (fsPosRun);
			fs = Scoreboard.S.CreateFloatingScore (ScoreManager.CHAIN, fsPts);
			fs.fontSizes = new List<float> (new float[] { 4, 50, 28 });

			if (fsRun == null) {
				fsRun = fs;
				fsRun.reportFinishTo = null;
			} else {
				fs.reportFinishTo = fsRun.gameObject;
			}
			break;
		}
	}
}
