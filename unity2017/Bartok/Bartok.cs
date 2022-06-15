﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This enum contains the different phases of a game turn

public enum TurnPhase {
	idle,
	pre,
	waiting,
	post,
	gameOver
}

public class Bartok : MonoBehaviour {
	static public Bartok S;
	static public Player CURRENT_PLAYER;

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
	public TurnPhase phase = TurnPhase.idle;

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

		// Set the CardBartok to call CBCallback on this Bartok when it is done
		tCB.reportFinishTo = this.gameObject;
	}

	// This callback is used by the last card to be dealt at the beginning
	public void CBCallback(CardBartok cb) {
		// You sometimes want to have reporting of method calls like this
		Utils.tr ("Bartok:CBCallback()", cb.name);
		StartGame ();
	}

	public void StartGame() {
		// Pick the player to the left of the human to go first
		PassTurn(1);
	}

	public void PassTurn(int num = -1) {
		// If no number was passed in, pick the next player
		if (num == -1) {
			int ndx = players.IndexOf (CURRENT_PLAYER);
			num = (ndx + 1) % 4;
		}
		int lastPlayerNum = -1;
		if (CURRENT_PLAYER != null) {
			lastPlayerNum = CURRENT_PLAYER.playerNum;

			// Check for Game Over and need to reshuffle discards
			//   * Do this before advancing the turn
			//     .. so that GameOverUI and RoundResultUI can read
			//     .. from CURRENT_PLAYER i.e. the player that won
			if (CheckGameOver()) {
				return;
			}
		}
		CURRENT_PLAYER = players [num];
		phase = TurnPhase.pre;

		CURRENT_PLAYER.TakeTurn();

		// Report the turn passing
		Utils.tr(
			"Bartok:PassTurn()",
			"Old: " + lastPlayerNum,
			"New: " + CURRENT_PLAYER.playerNum
		);
	}

	public bool CheckGameOver() {
		// See if need to reshuffle the discard pile into the draw pile
		if (drawPile.Count == 0) {
			List<Card> cards = new List<Card> ();
			foreach (CardBartok cb in discardPile) {
				cards.Add (cb);
			}
			discardPile.Clear ();
			Deck.Shuffle (ref cards);
			drawPile = UpgradeCardsList (cards);
			ArrangeDrawPile ();
		}

		// Check to see if the current player has won
		if (CURRENT_PLAYER.hand.Count == 0) {
			// The player that just played has won
			phase = TurnPhase.gameOver;
			// wait for one second before restarting
			Invoke ("RestartGame", 1);
			return (true);
		}

		return (false);
	}

	public void RestartGame() {
		CURRENT_PLAYER = null;
		SceneManager.LoadScene ("__Bartok_Scene_0");
	}

	// ValidPlay verifies that the card chosen can be played on the discard pile
	public bool ValidPlay(CardBartok cb) {
		// It's a valid play if the rank is the same
		if (cb.rank == targetCard.rank)
			return (true);

		// It's a valid play if the suit is the same
		if (cb.suit == targetCard.suit) {
			return (true);
		}

		return (false);
	}

	// This makes the new card the target
	public CardBartok MoveToTarget(CardBartok tCB) {
		tCB.timeStart = 0;
		tCB.MoveTo (layout.discardPile.pos + Vector3.back);
		tCB.state = CBState.toTarget;
		tCB.faceUp = true;

		tCB.SetSortingLayerName ("10");
		tCB.eventualSortLayer = layout.target.layerName;
		if (targetCard != null) {
			MoveToDiscard (targetCard);
		}

		targetCard = tCB;

		return(tCB);
	}

	public CardBartok MoveToDiscard(CardBartok tCB) {
		tCB.state = CBState.discard;
		discardPile.Add (tCB);
		tCB.SetSortingLayerName (layout.discardPile.layerName);
		tCB.SetSortOrder (discardPile.Count * 4);
		tCB.transform.localPosition = layout.discardPile.pos + Vector3.back / 2;

		return (tCB);
	}

	// Pull and return a single card from the drawPile
	public CardBartok Draw() {
		CardBartok cd = drawPile [0]; // pull the 0th CardBartok

		if (drawPile.Count == 0) { // If the drawPile is now empty
			// Shuffle the discards into the drawPile

			int ndx;
			// NOTE it is easier to do this:
			//   Use this while loop to pull random cards from the
			//     discardPile
			// .. than to:
			//   Convert the discardPile from List<CardBartok> to List<Card>
			//     in order to call Deck.Shuffle() on it.
			while (discardPile.Count > 0) {
				// Pull a random card from the discard pile
				ndx = Random.Range (0, discardPile.Count);
				drawPile.Add (discardPile [ndx]);
				discardPile.RemoveAt (ndx);
			}
			ArrangeDrawPile ();

			// Show the cards moving to the drawPile
			float t = Time.time;
			foreach (CardBartok tCB in drawPile) {
				tCB.transform.localPosition = layout.discardPile.pos;
				tCB.callbackPlayer = null;
				tCB.MoveTo (layout.drawPile.pos);
				tCB.timeStart = t;
				t += 0.02f;
				tCB.state = CBState.toDrawpile;
				tCB.eventualSortLayer = "0";
			}
		}
		drawPile.RemoveAt (0);
		return (cd);
	}

	public void CardClicked(CardBartok tCB) {
		if (CURRENT_PLAYER.type != PlayerType.human)
			return;
		if (phase == TurnPhase.waiting)
			return;

		// If neither waiting nor AI player's turn:

		switch (tCB.state) {
		case CBState.drawpile:
			// Draw the top card, not necessarily the one clicked.
			CardBartok cb = CURRENT_PLAYER.AddCard (Draw ());
			cb.callbackPlayer = CURRENT_PLAYER;
			Utils.tr ("Bartok:CardClicked()", "Draw", cb.name);
			phase = TurnPhase.waiting;
			break;

		case CBState.hand:
			// Check to see whether the card is valid
			if (ValidPlay (tCB)) {
				CURRENT_PLAYER.RemoveCard (tCB);
				MoveToTarget (tCB);
				tCB.callbackPlayer = CURRENT_PLAYER;
				Utils.tr ("Bartok:CardClicked()", "Play", tCB.name,
					targetCard.name + " is target");
				phase = TurnPhase.waiting;
			} else {
				// Just ignore it but report what the player tried
				Utils.tr ("Bartok:CardClicked()", "Attempted to Play",
					tCB.name, targetCard.name + " is target");
			}
			break;
		}
	}

	/* No longer in use
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
	*/
}
