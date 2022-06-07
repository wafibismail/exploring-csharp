﻿using System.Collections;
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

	[Header("Set Dynamically")]
	public Deck deck;
	public Layout layout;
	public List<CardProspector> drawPile;
	public Transform layoutAnchor;
	public CardProspector target;
	public List<CardProspector> tableau;
	public List<CardProspector> discardPile;

	void Awake() {
		S = this;
	}

	void Start() {
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
		if (won) {
			// print ("Game Over. You won! :)");
			ScoreManager.EVENT(eScoreEvent.gameWin);
		} else {
			// print ("Game over. You lost. :(");
			ScoreManager.EVENT(eScoreEvent.gameLoss);
		}

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
}
