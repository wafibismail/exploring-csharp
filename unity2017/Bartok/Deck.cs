using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	[Header("Set in Inspector")]
	public bool startFaceUp = false;
	// Suits
	public Sprite suitClub;
	public Sprite suitDiamond;
	public Sprite suitHeart;
	public Sprite suitSpade;

	public Sprite[] faceSprites;
	public Sprite[] rankSprites;

	public Sprite cardBack;
	public Sprite cardBackGold;
	public Sprite cardFront;
	public Sprite cardFrontGold;

	// Prefabs
	public GameObject prefabCard;
	public GameObject prefabSprite;

	[Header("Set Dynamically")]
	public PT_XMLReader xmlr;
	public List<string> cardNames;
	public List<Card> cards;
	public List<Decorator> decorators;
	public List<CardDefinition> cardDefs;
	public Transform deckAnchor;
	public Dictionary<string, Sprite> dictSuits;

	// InitDeck is called by Prospector when it is ready
	public void InitDeck(string deckXMLText) {
		if (GameObject.Find ("Deck") == null) {
			GameObject anchorGO = new GameObject ("_Deck");
			deckAnchor = anchorGO.transform;
		}

		dictSuits = new Dictionary<string, Sprite> () {
			{ "C", suitClub },
			{ "D", suitDiamond },
			{ "H", suitHeart },
			{ "S", suitSpade }
		};

		ReadDeck (deckXMLText);

		MakeCards ();
	}

	// ReadDeck parses the XML file passed to it into CardDefinitions
	public void ReadDeck(string deckXMLText) {
		xmlr = new PT_XMLReader ();
		xmlr.Parse (deckXMLText);

		decorators = new List<Decorator> ();

		PT_XMLHashList xDecos = xmlr.xml["xml"][0]["decorator"];

		Decorator deco;

		for (int i = 0; i < xDecos.Count; i++) {
			deco = new Decorator ();

			deco.type = xDecos [i].att ("type");
			deco.flip = (xDecos [i].att ("flip") == "1");
			deco.scale = float.Parse (xDecos [i].att ("scale"));

			deco.loc.x = float.Parse (xDecos [i].att ("x"));
			deco.loc.y = float.Parse (xDecos [i].att ("y"));
			deco.loc.z = float.Parse (xDecos [i].att ("z"));

			decorators.Add (deco);
		}

		cardDefs = new List<CardDefinition>();

		PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];
		for (int i = 0; i < xCardDefs.Count; i++) {
			CardDefinition cDef = new CardDefinition ();

			cDef.rank = int.Parse (xCardDefs [i].att ("rank"));

			PT_XMLHashList xPips = xCardDefs [i] ["pip"];
			if (xPips != null) {
				for (int j = 0; j < xPips.Count; j++) {
					deco = new Decorator ();

					deco.type = "pip";
					deco.flip = (xPips [j].att ("flip") == "1");

					deco.loc.x = float.Parse (xPips [j].att ("x"));
					deco.loc.y = float.Parse (xPips [j].att ("y"));
					deco.loc.z = float.Parse (xPips [j].att ("z"));

					if (xPips [j].HasAtt ("scale")) {
						deco.scale = float.Parse (xPips [j].att ("scale"));
					}

					cDef.pips.Add (deco);
				}
			}

			// (Jack, Queen, & King)
			if (xCardDefs [i].HasAtt ("face")) {
				cDef.face = xCardDefs [i].att ("face"); // b
			}

			cardDefs.Add (cDef);
		}
	}

	// Get the proper CardDefinition based on Rank (1 to 14 is Ace to King)
	public CardDefinition GetCardDefinitionByRank(int rnk) {
		foreach (CardDefinition cd in cardDefs) {
			if (cd.rank == rnk) {
				return(cd);
			}
		}
		return(null);
	}

	// Make the Card GameObjects
	public void MakeCards() {
		// Each suit goes from 1 to 14 (e.g., C1 to C14 for Clubs)
		cardNames = new List<string>();
		string[] letters = new string[] {"C","D","H","S"};
		foreach (string s in letters) {
			for (int i=0; i<13; i++) {
				cardNames.Add(s+(i+1));
			}
		}

		cards = new List<Card>();
		for (int i = 0; i < cardNames.Count; i++) {
			cards.Add (MakeCard (i));
		}
	}

	private Card MakeCard(int cNum) {
		GameObject cgo = Instantiate (prefabCard) as GameObject;

		cgo.transform.parent = deckAnchor;

		Card card = cgo.GetComponent<Card> ();

		// Stack the cards into nice rows
		cgo.transform.localPosition = new Vector3 ((cNum % 13) * 3, cNum / 13 * 4, 0);

		// Assign basic values to the Card
		card.name = cardNames [cNum];
		card.suit = card.name [0].ToString ();
		card.rank = int.Parse (card.name.Substring (1));
		if (card.suit == "D" || card.suit == "H") {
			card.colS = "Red";
			card.color = Color.red;
		}
		// Pull the CardDefinition for this card
		card.def = GetCardDefinitionByRank (card.rank);
		AddDecorators (card);
		AddPips (card);
		AddFace (card);
		AddBack (card);

		return card;
	}

	// These private variables will be reused several times in helper methods
	private Sprite _tSp = null;
	private GameObject _tGO = null;
	private SpriteRenderer _tSR = null;

	private void AddDecorators(Card card) {
		foreach( Decorator deco in decorators ) {
			if (deco.type == "suit") {
				_tGO = Instantiate( prefabSprite ) as GameObject;
				_tSR = _tGO.GetComponent<SpriteRenderer>();
				_tSR.sprite = dictSuits[card.suit];
			} else {
				_tGO = Instantiate( prefabSprite ) as GameObject;
				_tSR = _tGO.GetComponent<SpriteRenderer>();
				_tSp = rankSprites[ card.rank ];
				_tSR.sprite = _tSp;
				_tSR.color = card.color;
			}
			// Make the deco Sprites render above the Card
			_tSR.sortingOrder = 1;
			_tGO.transform.SetParent( card.transform );
			_tGO.transform.localPosition = deco.loc;

			if (deco.flip) {
				// flip it
				_tGO.transform.rotation = Quaternion.Euler(0,0,180);
			}
			// Set the scale to keep decos from being too big
			if (deco.scale != 1) {
				_tGO.transform.localScale = Vector3.one * deco.scale;
			}
			// Name this GameObject so it's easy to see
			_tGO.name = deco.type;
			// Add this deco GameObject to the List card.decoGOs
			card.decoGOs.Add(_tGO);
		}
	}

	private void AddPips(Card card) {
		foreach (Decorator pip in card.def.pips) {
			_tGO = Instantiate (prefabSprite) as GameObject;
			_tGO.transform.SetParent (card.transform);
			_tGO.transform.localPosition = pip.loc;
			if (pip.flip) {
				_tGO.transform.rotation = Quaternion.Euler (0, 0, 180);
			}
			if (pip.scale != 1) {
				_tGO.transform.localScale = Vector3.one * pip.scale;
			}
			_tGO.name = "pip";
			_tSR = _tGO.GetComponent<SpriteRenderer>();
			_tSR.sprite = dictSuits[card.suit];
			_tSR.sortingOrder = 1;
			card.pipGOs.Add(_tGO);
		}
	}

	private void AddFace(Card card) {
		if (card.def.face == "") {
			return;
		}

		_tGO = Instantiate (prefabSprite) as GameObject;
		_tSR = _tGO.GetComponent<SpriteRenderer> ();

		// Generate the right name and pass it to GetFace()
		_tSp = GetFace (card.def.face + card.suit);
		_tSR.sprite = _tSp; // Assign this Sprite to _tSR
		_tSR.sortingOrder = 1; // Set the sortingOrder
		_tGO.transform.SetParent (card.transform);
		_tGO.transform.localPosition = Vector3.zero;
		_tGO.name = "face";
	}

	// Find the proper face card Sprite
	private Sprite GetFace(string faceS) {
		foreach (Sprite _tSP in faceSprites) {
			if (_tSP.name == faceS) {
				return( _tSP );
			}
		}
		return( null );
	}

	private void AddBack(Card card) {
		// Add Card Back
		// The Card_Back will be able to cover everything else on the Card
		_tGO = Instantiate( prefabSprite ) as GameObject;
		_tSR = _tGO.GetComponent<SpriteRenderer>();
		_tSR.sprite = cardBack;
		_tGO.transform.SetParent( card.transform );
		_tGO.transform.localPosition = Vector3.zero;

		// A sortingOrder that's higher than anything else's
		_tSR.sortingOrder = 2;
		_tGO.name = "back";
		card.back = _tGO;

		card.faceUp = startFaceUp;
	}

	static public void Shuffle(ref List<Card> oCards) {

		// List to temporarily hold the shuffle order
		List<Card> tCards = new List<Card>();

		int ndx;

		// Note to self: I wonder why tCards is assigned twice
		tCards = new List<Card>();

		while (oCards.Count > 0) {
			ndx = Random.Range(0,oCards.Count);
			tCards.Add (oCards[ndx]);
			oCards.RemoveAt(ndx);
		}
		// The variable oCards that is passed by reference
		//   is overwritten
		oCards = tCards;
	}
}
