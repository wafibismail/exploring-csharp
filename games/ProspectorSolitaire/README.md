File > Build Settings
- switch platform

Importing sprites:
- texture to Sprite(2D and UI)
- sprite atlas?
  - change its mode to multiple
  - in sprite editor
    - slice > type : grid by cell size
- just like anything on screen, sprites need to be inclosed in GameObjects

Pass by reference:
- Just as & is used in C++ to pass a variable by reference
  - in C#, it's the keyword *ref*
    - used in both the function definition and function call e.g.:
      - static public void Shuffle(ref List&lt;Card&gt; oCards) { }
      - Shuffle(ref cards);

Layers in Unity2D:
- In Unity2D, objects are layered in order with the use of sorting layers
  - everything in a lower sorting layer is rendered behind everything in a higher sorting layer
- Edit > Project Settings > Tags and Layers

Usability:
- Card and Deck are designed to be reusable
- CardProspector is the Card's subclass that is specifically meant to be used in Prospector

object conversion e.g:
- **as**
  - can be used when converting a subclass to its more general superclass
  - but will return null if trying to convert superclass to its subclass
    - e.g. card as CardProspector, where card is an instance of the superclass Card
    - The solution? Make card a CardProspector to begin with.
      - This way, both methods of CardProspector and Card would work when called on it as CardProspector can always be referred to as a Card

Note on implementing game logic
- it is useful first to delineate possible actions that can happen in a game
  - which are passive e.g. flipping cards that are no longer hidden
  - which are active e.g. moved by players
  - etc.

Note on ScoreManager
- In the book's first edition, the author actually implemented ScoreManager as a method of Prospector rather than a separate class.
- However, by adopting the software pattern, ScoreManager, being a class of its own, is made to something that could be reused in the future.
  - In addition, the decision leads to a simpler code.

To do/check later:
- implement scoring system that doubles scores in runs that have gold cards