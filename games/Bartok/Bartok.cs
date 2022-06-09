﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bartok : MonoBehaviour {
	static public Bartok S;

	[Header("Set in Inspector")]
	public TextAsset deckXML;
	public TextAsset layoutXML;
	public Vector3 layoutCenter = Vector3.zero;
	public float handFanDegrees = 10f;
	public int numStartingCards = 7;
	public float drawTimeStagger = 0.1f;

	[Header("Set Dynamically")]
	public Deck deck;
	public List<CardBartok> drawPile;
	public List<CardBartok> discardPile;
	public List<Player> players;
	public CardBartok targetCard;

	private BartokLayout layout;
	private Transform layoutAnchor;

	void Awake() {
		S = this;
	}

	void Start() {
		deck = GetComponent<Deck> ();
		deck.InitDeck (deckXML.text);
		Deck.Shuffle (ref deck.cards);

		layout = GetComponent<BartokLayout> ();
		layout.ReadLayout (layoutXML.text);

		drawPile = UpgradeCardsList (deck.cards);
		LayoutGame ();
	}

	List<CardBartok> UpgradeCardsList (List<Card> lCD){
		List<CardBartok> lCB = new List<CardBartok> ();
		foreach (Card tCD in lCD) {
			lCB.Add (tCD as CardBartok);
		}
		return (lCB);
	}

	public void ArrangeDrawPile() {
		CardBartok tCB;

		for (int i = 0; i < drawPile.Count; i++) {
			tCB = drawPile [i];
			tCB.transform.SetParent (layoutAnchor);
			tCB.transform.localPosition = layout.drawPile.pos;
			// Rotation should start at 0
			tCB.faceUp = false;
			tCB.SetSortingLayerName (layout.drawPile.layerName);
			tCB.SetSortOrder (-i * 4);
			tCB.state = CBState.drawpile;
		}
	}

	public void LayoutGame() {
		// Create an empty GameObject to serve as the tableau's anchor
		if (layoutAnchor == null) {
			GameObject tGO = new GameObject ("_LayoutAnchor");
			layoutAnchor = tGO.transform;
			layoutAnchor.transform.position = layoutCenter;
		}

		// Position the drawPile cards
		ArrangeDrawPile();

		// Set up the players
		Player pl;
		players = new List<Player> ();
		foreach (SlotDef tSD in layout.slotDefs) {
			pl = new Player ();
			pl.handSlotDef = tSD;
			players.Add (pl);
			pl.playerNum = tSD.player;
		}
		players [0].type = PlayerType.human; // Make only 0th player human

		CardBartok tCB;
		// Deal seven cards to each player
		for (int i = 0; i < numStartingCards; i++) {
			for (int j = 0; j < 4; j++) {
				tCB = Draw ();
				// Stagger the draw time a bit
				tCB.timeStart = Time.time + drawTimeStagger * (i * 4 + j);

				players [(j + 1) % 4].AddCard (tCB);
			}
		}

		Invoke ("DrawFirstTarget", drawTimeStagger * (numStartingCards * 4 + 4));
	}

	public void DrawFirstTarget() {
		// Flip up the first target card from the drawPile
		CardBartok tCB = MoveToTarget(Draw());
	}

	// This makes the new card the target
	public CardBartok MoveToTarget(CardBartok tCB) {
		tCB.timeStart = 0;
		tCB.MoveTo (layout.discardPile.pos + Vector3.back);
		tCB.state = CBState.toTarget;
		tCB.faceUp = true;

		targetCard = tCB;

		return(tCB);
	}

	// Pull a single card from the drawPile
	public CardBartok Draw() {
		CardBartok cd = drawPile [0];
		drawPile.RemoveAt (0);
		return (cd);
	}

	// Update() is temporarily used to test adding cards to players' hands
	void Update() {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			players [0].AddCard (Draw ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			players [1].AddCard (Draw ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			players [2].AddCard (Draw ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			players [3].AddCard (Draw ());
		}
	}
}
