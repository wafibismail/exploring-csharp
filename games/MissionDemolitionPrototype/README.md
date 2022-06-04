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

# Paused at 630 - Providing Vection and a Sense of Speed