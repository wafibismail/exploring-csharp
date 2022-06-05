Directories hierarchy:
- __Scripts:
  - BoundsCheck.cs
  - Enemy_1.cs
  - Enemy_2.cs
  - Enemy_3.cs
  - Enemy_4.cs
  - Enemy.cs
  - Hero.cs
  - Main.cs
  - Projectile.cs
  - Shield.cs
  - Weapon.cs
- _Materials:
  - Mat_Collar.mat
  - Mat_Projectile.mat
  - Mat_Shield.mat
  - PowerUp.psd
  - Shields.psd
  - Space_Transparent.png
  - Space.png
  - UnlitAlpha.shader
- _Prefabs:
  - Enemy_0.prefab      
  - Enemy_1.prefab
  - Enemy_2.prefab
  - Enemy_3.prefab
  - Enemy_4.prefab
  - ProjectileHero.prefab 
  - Weapon.prefab

This is a continuation of the earlier Space SHMUP prototype. The author makes this distinction in his book as well. I thought there may be useful parts to refer to in the earlier version and thought that I would do it similarly too.
- .

Subclasses & MonoBehaviour methods e.g. Awake(), Start(), Update(), and so on:
- we do not need to use the keywords virtual or override to allow overriding in subclasses

Normal C# methods e.g. Move() in Enemy
- on the other hand, do need to be declared with override in the subclass for proper overriding the method which also need to be declared virtual in the superclass
- base.Move() calls the superclass method Move()

Sphere colliders
- only scale uniformly so e.g. in a flat ellipse with scale [6,3,2], its 2 will be taken as the collider's scale
  - box colliders can scale non-uniformly
  - capsule colliders can be used in approximating one direction being much longer than the others
  - mesh is the most accurate but too slow especially on mobile platforms

Bezier curve
- movements based on it slows down in the middle of the curve
  - also, the middle part is never touched; this a feature of the curve
- adding easing helps to speed up this slow middle part

Need to associate various options together into a new kind of variable?
- use an enum!
  - declare it/them between the class definition and the *using* statements in a C# script e.g. in Weapon.cs

Function Delegates:
- a function delegate can be thought of as a container for similar functions or methods that can all be called at once i.e. multicasting (can be called individually as regular functions as well)
  - if the function returns a value, the value from the last function called is returned
- if no function is attached to it, an error will be thrown on trying to call it. Check first that it is not null

Input.GetAxis("Jump")
- is equal to 1 when the spacebar or jump button on a controller is pressed

To do / to check:
- u curve function
- Quaternion
- Bezier curve function
- Expand the game further e.g.
  - experimenting with the various WeaponDefinition fields/options