About active checkboxes:
- Like GameObjects, Components too have them
  - While some Components allow these to be set in code directly via SetActive(), others do not.
    - Use other ways around inconsistencies like this, e.g. create a GameObject just to contain the Component in

isKinematic
- When a Rigidbody is kinematic, it is not moved automatically by physics but is still part of the simulation
  - meaning, a kinematic Rigidbody will not move as a result of a collision or gravity, but can still cause other nonkinematic Rigidbodies to move

Mnemonic for vector subtractions:
- "A minus B looks at A"

Rigidbody collosion detection:
- Continuous collision detection takes more processing power than discrete, but is more accurate for fast-moving objects like the projectile

Local vs. world coordinates & scales
- Position
  - *localPosition*: position relative to center of parent
  - *position*: world position
- Scale
  - *localScale*: scale relative to parent transform
  - *lossyScale*: estimated scale in world coordinates
    - there is no *scale*

Also in this project:
- Organizing the Project pane.
  - See the how neat the main MissionDemolitionPrototype folder is compared to e.g. Boid, ApplePickerPrototype, etc.
  - The Project pane corresponds to:
    - the Assets folder on local storage, or
    - the main MissionDemolitionPrototype folder in this repository
  - Note the use of underscores and double underscore:
    - __Scripts
    - _Materials
    - _Prefabs
  - I commit only the scripts to keep this repository clean.

# PAUSED AT 639 - MAKING WALLS AND SLABS