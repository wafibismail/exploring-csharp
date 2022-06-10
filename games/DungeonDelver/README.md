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
