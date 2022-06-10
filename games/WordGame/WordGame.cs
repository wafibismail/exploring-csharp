using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameMode {
	preGame, // Before the game starts
	loading, // The word list is loading and being parsed
	makeLevel, // The individual WordLevel is being created
	levelPrep, // The level visuals are Instantiated
	inLevel // The level is in progress
}

public class WordGame : MonoBehaviour {
	static public WordGame S;

	[Header("Set in Inspector")]
	public GameObject prefabLetter;
	public Rect wordArea = new Rect(-24, 19, 48, 28);
	public float letterSize = 1.5f;
	public bool showAllWyrds = true;
	public float bigLetterSize = 4f;
	public Color bigColorDim = new Color (0.8f, 0.8f, 0.8f);
	public Color bigColorSelected = new Color (1f, 0.9f, 0.7f);
	public Vector3 bigLetterCenter = new Vector3(0, -16, 0);

	[Header("Set Dynamically")]
	public GameMode mode = GameMode.preGame;
	public WordLevel currLevel;
	public List<Wyrd> wyrds;
	public List<Letter> bigLetters;
	public List<Letter> bigLettersActive;

	private Transform letterAnchor, bigLetterAnchor;

	void Awake() {
		S = this;
		letterAnchor = new GameObject ("LetterAnchor").transform;
		bigLetterAnchor = new GameObject ("BigLetterAnchor").transform;
	}

	void Start () {
		mode = GameMode.loading;

		WordList.INIT (); // The static Init() of WordList
	}

	// Called by the SendMessage() command from WordList
	public void WordListParseComplete() {
		mode = GameMode.makeLevel;

		// Make a level and assign it to currLevel, the current WordLevel
		currLevel = MakeWordLevel();
	}

	public WordLevel MakeWordLevel(int levelNum = -1) {
		WordLevel level = new WordLevel ();
		if (levelNum == -1) {
			// Pick a random level
			level.longWordIndex = Random.Range (0, WordList.LONG_WORD_COUNT);
		} else {
			// This will be added later in the chapter
		}
		level.levelNum = levelNum;
		level.word = WordList.GET_LONG_WORD (level.longWordIndex);
		level.charDict = WordLevel.MakeCharDict (level.word);

		StartCoroutine (FindSubWordsCoroutine (level));

		return (level);
	}

	// A coroutine that finds words that can be spelled in this level
	public IEnumerator FindSubWordsCoroutine(WordLevel level) {
		level.subWords = new List<string> ();
		string str;

		// Note Lists are passed by reference
		//   This makes this functionality relatively very fast
		List<string> words = WordList.GET_WORDS ();

		// Iterate through all the words in the wordList
		for (int i = 0; i<WordList.WORD_COUNT; i++) {
			str = words [i];
			// Check whether each one can be spelled using level.charDict
			if (WordLevel.CheckWordInLevel(str, level)) {
				level.subWords.Add (str);
			}
			// Yield if we've parsed a lot of words this frame
			if(i%WordList.NUM_TO_PARSE_BEFORE_YIELD == 0) {
				// yield until next frame
				yield return null;
			}
		}

		level.subWords.Sort (); // <- Alphabetically
		level.subWords = SortWordsByLength (level.subWords).ToList ();

		// The coroutine is complete, so call SubWordSearchComplete()
		SubWordSearchComplete();
	}

	// Use LINQ to sort the array received and return a copy
	public static IEnumerable<string> SortWordsByLength(IEnumerable<string> ws) {
		ws = ws.OrderBy (s => s.Length);
		return ws;
	}

	public void SubWordSearchComplete() {
		mode = GameMode.levelPrep;
		Layout ();
	}

	void Layout() {
		// Place the letters for each subword of currLevel on screen
		wyrds = new List<Wyrd>();

		// Declare a lot of local variables that will be used in this method
		GameObject go;
		Letter lett;
		string word;
		Vector3 pos;
		float left = 0;
		float columnWidth = 3;
		char c;
		Color col;
		Wyrd wyrd;

		// Determine how many letters will fit on screen
		int numRows = Mathf.RoundToInt(wordArea.height/letterSize);

		// Make a Wyrd for each level subWord
		for (int i = 0; i < currLevel.subWords.Count; i++) {
			wyrd = new Wyrd ();
			word = currLevel.subWords [i];

			// if the word is longer than columnWidth, expand it
			columnWidth = Mathf.Max(columnWidth, word.Length);

			// Instantiate a PrefabLetter for each letter of the word
			for (int j = 0;j < word.Length; j++) {
				c = word [j];
				go = Instantiate<GameObject> (prefabLetter);
				go.transform.SetParent (letterAnchor);
				lett = go.GetComponent<Letter> ();
				lett.c = c; // Set the c of the Letter

				// Position the Letter
				pos = new Vector3(wordArea.x + left + j*letterSize, wordArea.y, 0);

				// The % here makes multiple columns line up
				pos.y -= (i%numRows)*letterSize;

				lett.pos = pos;

				go.transform.localScale = Vector3.one * letterSize;

				wyrd.Add (lett);

			}

			if (showAllWyrds)
				wyrd.visible = true;

			wyrds.Add (wyrd);

			// If we've gotten to the numRows(th) row, start a new column
			if (i%numRows == numRows -1) {
				left += (columnWidth + 0.5f) * letterSize;
			}
		}

		// Place the big letters
		// Instantiate the List<>s for big letters
		bigLetters = new List<Letter>();
		bigLettersActive = new List<Letter> ();

		// Create a big letter for each letter in the target word
		for (int i = 0; i <currLevel.word.Length; i++) {
			// This is similar to the process for a normal Letter
			c = currLevel.word[i];
			go = Instantiate<GameObject> (prefabLetter);
			go.transform.SetParent (bigLetterAnchor);
			lett = go.GetComponent<Letter> ();
			lett.c = c;
			go.transform.localScale = Vector3.one * bigLetterSize;

			// Set the initial position of the big letters below screen
			pos = new Vector3(0, -100, 0);
			lett.pos = pos; // You'll add more code around this line later

			col = bigColorDim;
			lett.color = col;
			lett.visible = true; // This is always true for big letters
			lett.big = true;
			bigLetters.Add (lett);
		}
		// Shuffle the big letters
		bigLetters = ShuffleLetters(bigLetters);
		// Arrange them on screen
		ArrangeBigLetters();

		// Set the mode to be in-game
		mode = GameMode.inLevel;
	}

	// This method shuffles a List<Letter> randomly and returns the result
	List<Letter> ShuffleLetters(List<Letter> letts) {
		List<Letter> newL = new List<Letter> ();
		int ndx;
		while (letts.Count > 0) {
			ndx = Random.Range (0, letts.Count);
			newL.Add (letts [ndx]);
			letts.RemoveAt (ndx);
		}
		return (newL);
	}

	// This method arranges the big Letters on screen
	void ArrangeBigLetters() {
		// The halfWidth allows the big Letters to be centered
		float halfWidth = ( (float) bigLetters.Count)/2f - 0.5f;
		Vector3 pos;
		for (int i = 0; i < bigLetters.Count; i++) {
			pos = bigLetterCenter;
			pos.x += (i - halfWidth) * bigLetterSize;
			bigLetters [i].pos = pos;
		}
		// bigLettersActive
		halfWidth = ((float) bigLettersActive.Count)/2f - 0.5f;
		for (int i = 0; i < bigLettersActive.Count; i++) {
			pos = bigLetterCenter;
			pos.x += (i - halfWidth) * bigLetterSize;
			pos.y += bigLetterSize * 1.25f;
			bigLettersActive [i].pos = pos;
		}

	}
}
