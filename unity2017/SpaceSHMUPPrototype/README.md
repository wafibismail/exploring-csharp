Directories hierarchy
- __Scripts
  - BoundsCheck.cs
  - Enemy.cs
  - Hero.cs
  - Main.cs
  - Projectile.cs
  - Shield.cs
- _Materials
  - Mat_Projectiles.mat
  - Mat_Shield.mat
  - PowerUp.psd
  - Shields.psd
  - Space_Transparent.png
  - Space.png
  - UnlitAlpha.shader
- _Prefabs
  - Enemy_0.prefab
  - Enemy_1.prefab
  - Enemy_2.prefab
  - Enemy_3.prefab
  - Enemy_4.prefab
  - ProjectileHero.prefab

Rigidbody
- If a moving GameObject doesn't have a Rigidbody component, the GameObject's collider location will not move with the GameObject
- but if it does have one, the colliders of both it and all of its children are updated every frame

Random.Range()
- with integer, it acts as expected i.e. returning a number from [min,max)
- however with floats, it returns a number from [min,max]

collider.gameObject.transform.root gets the parent transform of the gameObject
- collider.gameObject.transform.root.gameObject for the parent's GameObject

Lighting issue fix if needed (ship turning dark after rerendering):
- Window > Lighting > Settings
- click Scene at the top of the Lighting pane
  - uncheck Auto Generate
  - click Generate Lighting
- Need to do only once (unless the lighting is readjusted)