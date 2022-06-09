using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerType {
	human,
	ai
}

[System.Serializable]
public class Player {
	public PlayerType type = PlayerType.ai;
	public int playerNum;
	public SlotDef handSlotDef;
	public List<CardBartok> hand; // The cards in the player's hand

	// Add a card to the hand
	public CardBartok AddCard(CardBartok eCB) {
		if (hand == null)
			hand = new List<CardBartok> ();

		hand.Add (eCB);

		// Sort the cards by rank using LINQ if this is a human
		if (type == PlayerType.human) {
			CardBartok[] cards = hand.ToArray ();

			// LINQ call
			cards = cards.OrderBy (cd => cd.rank).ToArray ();

			hand = new List<CardBartok> (cards);
		}

		eCB.SetSortingLayerName ("10"); // Sorts the moving card to the top
		eCB.eventualSortLayer = handSlotDef.layerName;


		FanHand ();

		return eCB;
	}

	// Remove a card from the hand
	public CardBartok RemoveCard(CardBartok cb) {
		if (hand == null || !hand.Contains (cb))
			return null;
		hand.Remove (cb);
		FanHand ();
		return (cb);
	}

	public void FanHand() {
		// startRot is the rotation about Z of the first card
		float startRot = 0;
		startRot = handSlotDef.rot;
		if (hand.Count > 1) {
			startRot += Bartok.S.handFanDegrees * (hand.Count - 1) / 2;
		}

		// Move all the cards to their new positions
		Vector3 pos;
		float rot;
		Quaternion rotQ;
		for (int i = 0; i < hand.Count; i++) {
			rot = startRot - Bartok.S.handFanDegrees * i;
			rotQ = Quaternion.Euler (0, 0, rot);

			pos = Vector3.up * CardBartok.CARD_HEIGHT / 2f;

			pos = rotQ * pos;

			// Add the base position of the player's hand (which will be at the
			// bottom-center of the fan of the cards)
			pos += handSlotDef.pos;
			pos.z = -0.5f * i; // staggering pos.zs prevent the 3D box colliders from overlapping

			// If not the initial deal, start moving the card immediately.
			//   (we want the card to start moving immediately)
			//   (unlike at the beginning of the game when card movements are staggered)
			if (Bartok.S.phase != TurnPhase.idle) {
				hand [i].timeStart = 0;
			}

			// Set the localPosition and rotation of the ith card in the hand
			hand[i].MoveTo(pos,rotQ); // Tell CardBartok to interpolate
			hand[i].state = CBState.toHand;
			// After finish moving, CardBartok will set the state to CBState.hand
			
			/* OLD CODE : instant placement
			hand[i].transform.localPosition = pos;
			hand [i].transform.rotation = rotQ;
			hand [i].state = CBState.hand;
			*/

			// Only the player's cards should be face up
			hand [i].faceUp = (type == PlayerType.human);

			// Set the SortOrder of the cards so that they overlap properly
			//hand[i].SetSortOrder(i*4); // old code
			hand[i].eventualSortOrder = i*4;
		}
	}

	// The TakeTurn() function enables the AI of the computer Players
	public void TakeTurn() {
		Utils.tr ("Player.TakeTurn");

		// Don't need to do anything if this is the human player.
		if (type == PlayerType.human) return;

		Bartok.S.phase = TurnPhase.waiting;

		CardBartok cb;

		// If this is an AI player, need to make a choice about what to play
		// Find valid plays
		List<CardBartok> validCards = new List<CardBartok>();
		foreach (CardBartok tCB in hand) {
			if (Bartok.S.ValidPlay (tCB)) {
				validCards.Add (tCB);
			}
		}
		// If there are no valid cards
		if(validCards.Count == 0) {
			// ... then draw a card
			cb = AddCard(Bartok.S.Draw());
			cb.callbackPlayer = this;
			return;
		}

		// So, there is a card or more to play, so pick one
		cb = validCards[Random.Range(0, validCards.Count)];
		RemoveCard (cb);
		Bartok.S.MoveToTarget (cb);
		cb.callbackPlayer = this;
	}

	// CardBartok should call this when it's done moving
	//   However because Player does not extend MonoBehaviour,
	//   .. SendMessage() cannot be used.
	//   .. Instead, a reference to this Player must be passed to the CardBartok
	public void CBCallback(CardBartok tCB) {
		Utils.tr ("Player.CBCallback()", tCB.name, "Player " + playerNum);
		// The card is done moving, so pass the turn
		Bartok.S.PassTurn();
	}
}
