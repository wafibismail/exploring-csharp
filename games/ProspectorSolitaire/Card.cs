using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
	[Header("Set Dynamically")]
	public string suit; // Suit of the Card (C,D,H, or S)
	public int rank; // Rank of the Card (1-14)
	public Color color = Color.black; // Color to tint pips
	public string colS = "Black"; // or "Red". Name of the Color

	// This List holds all of the Decorator GameObjects
	public List<GameObject> decoGOs = new List<GameObject>();
	// This List holds all of the Pip GameObjects
	public List<GameObject> pipGOs = new List<GameObject>();

	public GameObject back; // The GameObject of the back of the card

	public CardDefinition def; // Parsed from DeckXML.xml

	// About Sorting Layers
	//   This must be handled in this Card superclass as it is a general behaviour

	public SpriteRenderer[] spriteRenderers;

	void Start() {
		SetSortOrder (0); // Ensures that the card starts properly depth sorted
	}

	public void PopulateSpriteRenderers() {
		if (spriteRenderers == null || spriteRenderers.Length == 0) {
			spriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
		}
	}

	public void SetSortingLayerName(string tSLN) {
		PopulateSpriteRenderers ();

		foreach (SpriteRenderer tSR in spriteRenderers) {
			tSR.sortingLayerName = tSLN;
		}
	}

	public void SetSortOrder(int sOrd) {
		PopulateSpriteRenderers ();

		foreach (SpriteRenderer tSR in spriteRenderers) {
			if (tSR.gameObject == this.gameObject) {
				// if true, then that gameObject is the white card background
				tSR.sortingOrder = sOrd;
				continue;
			}

			switch (tSR.gameObject.name) {
			case "back":
				// if the back is visible, it should be on top of everything else
				tSR.sortingOrder = sOrd + 2;
				break;

			case "face":
			default:
				// pips, decorators, face, and so on
				tSR.sortingOrder = sOrd + 1;
				break;
			}
		}
	}

	public bool faceUp {
		get { return !back.activeSelf; }
		set { back.SetActive (!value); }
	}

	virtual public void OnMouseUpAsButton() {
		print (name);
	}
}

[System.Serializable]
public class Decorator {
	// This class stores information about each decorator or pip from DeckXML
	public string type; // For card pips, type = "pip"
	public Vector3 loc; // The location of the Sprite on the Card
	public bool flip = false; // Whether to flip the Sprite vertically
	public float scale = 1f; // The scale of the Sprite
}

[System.Serializable]
public class CardDefinition {
	// This class stores information for each rank of card
	public string face; // Sprite to use for each face card
	public int rank; // The rank (1-13) of this card
	public List<Decorator> pips = new List <Decorator>(); // Pips used
}