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