### About Art Assets
- As a prototype, a game doesn't need fantastic art; It needs to work
  - Programmer art i.e. placeholder art meant to be replaced eventually, will suffice
    - Intended to get us from a concept to a working prototype as quickly as possible

### Camera projection
- Perspective
  - Like human eye views things
- Orthographic
  - Used here
  - Objects of same size regardless of distance from lens

### New objects or concepts encountered
- Text
- Canvas
  - 2D board on which GUI will be arranged
  - Scaled to match the game pane e.g. 16:9 aspect ratio in my case
- EventSystem
  - What allows buttons, sliders and other interactive GUI systems to work
- PlayerPrefs is a dictionary of values
  - is separate for each project/application

### TODOs, maybe
- Start screen
  - in its own scene
  - has
    - an image, and
    - a start button
      - which calls **SceneManager.LoadScene ( "_Scene_0" )**
- Game Over screen
  - maybe display the final score
  - maybe inform whether it exceeds the previous high score
- increasing difficulty
  - e.g. store each value of speed, chanceToChanceDirections, etc., in a list or array

### On adding either Start or Game Over screen
- need to also add every scene in the game to the Build Settings scene list one at a time, by:
  - for each scene
    - File &gt; Build Settings
    - click Add Open Scenes
  - * if a build is created, the scene numbered zero will be the one that loads when the game first runs

### WARNING
- GameObject.FindGameObjectsWithTag() is processor-intensive
  - avoid using directly on every Update() or FixedUpdate()