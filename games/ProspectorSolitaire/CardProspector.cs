using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCardState {
	drawPile,
	tableau,
	target,
	discard
}

public class CardProspector : Card {
	[Header("Set Dynamically: CardProspector")]
	public eCardState state = eCardState.drawPile;
	public List<CardProspector> hiddenBy = new List<CardProspector>();

	// for matching this card to the tableau XML if it's a tableau card
	//   i.e. one of the initial 28 cards in the mine
	public int layoutID;

	public SlotDef slotDef;

	override public void OnMouseUpAsButton() {
		Prospector.S.CardClicked (this);
		base.OnMouseUpAsButton ();
	}
}
