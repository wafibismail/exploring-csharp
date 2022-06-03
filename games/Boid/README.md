#### Scripts

- Boid
  - to be attached to the Boid prefab,
  - to handle the movement of each individual Boid
    - Each Boid will think for itself and react to its own individual understanding of the world
- Neighborhoow
  - to be attached to the Boid prefab,
  - to keep track of which other Boids are nearby
    - The key to each Boid's understanding of the world is its knowledge of which other Boids are close enough to worry about.
- Attractor
  - to be attached to a GameObject which has the purpose of the thing which Boids flock around
- Spawner
  - to be attached to the Main Camera.
  - to store the fields that are shared by all Boids and instantiates all the instances of the Boid prefab.
- LookAtAttractor
  - to be attached to the Main Camera.
  - causes the camera to turn and look at the Attractor each frame.

By following an expansion of Object-Oriented Programming known as Component-Oriented Design, each resulting script (though being more) would be no larger than necessary.