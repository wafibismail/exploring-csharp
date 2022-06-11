"Resources" is one of Unity's special project folder names
- any files in it will be included in a compiled project by Unity,
  - regardless of whether or not it is included in the scene
- files in the folder can be loaded in code using Resources class (part of UnityEngine)

Anti-aliasing works well with 3D graphics
- 2x Multi Sampling (render an image at double the screensize and shrink to regular size)
  - but can make 2D graphics look .... not so well
    - disable anti-aliasing at Edit > Project Settings > Quality
    - alternatively, for selectively turning off on individual cameras
      - select the camera, set *Allow MSAA (MultiSample Anti-Aliasing)* to false

Sorting Layers:
- Sprite Renderers have a separate sorting layer from Physics

animator.CrossFade(string stateName, float normalizedTransitionDuration)
- self explanatory
- in the case of Dray's animation,
  - stateName could be one of the animations I created in the _Animation folder, e.g. Dray_Walk_0
  - normalizedTransitionDuration can be 0 for instant transition

Metrics
- 1 Unity unit = 1 meter = 1 tile in this prototype

LateUpdate()
- called after Update()
- useful for cleanup operations e.g. returning wandering characters to the room they are supposed to be in

Mathf.Clamp(val, min, max)
- ensures that val is between the min and max args

Interfaces vs Superclasses
- A class may implement several different interfaces simultaneously
- A class can only extend a single superclass
- Any class, regardless of superclass ancestry, can implement the same interface
- Interfaces can be thought of as promises
  - Any class that implements the interface promises to have specific methods or properties that can be called safely
  - Interface names often start with an I e.g. IFacingMover

OnTriggerEnter() vs OnCollisionEnter()
- Passed objects:
  - OnTriggerEnter - a Collider
  - OnCollisionEnter - a Collision