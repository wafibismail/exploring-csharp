using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour {
	[Header("Set Dynamically")]
	public List<Boid> neighbors;
	private SphereCollider coll;

	void Start() {
		neighbors = new List<Boid> ();
		coll = GetComponent<SphereCollider> ();
		coll.radius = Spawner.S.neighborDist / 2;
	}

	void FixedUpdate() {
		if (coll.radius != Spawner.S.neighborDist / 2) {
			coll.radius = Spawner.S.neighborDist / 2;
		}
	}

	// Called when something else enters this SphereCollider trigger
	// * a trigger is a collider that allows other things to pass through it
	void OnTriggerEnter(Collider other) {
		// To ensure it is in fact a Boid that is on the other collider
		Boid b = other.GetComponent<Boid> ();
		if (b != null) {
			if (neighbors.IndexOf(b) == -1) {
				neighbors.Add(b);
			}
		}
	}

	// In order to remove a Boid from the neighbors list once it is no
	// longer touching this Boid's trigger:
	void OnTriggerExit(Collider other) {
		Boid b = other.GetComponent<Boid> ();
		if (b != null) {
			if (neighbors.IndexOf(b) != -1) {
				neighbors.Remove(b);
			}
		}
	}

	public Vector3 avgPos {
		get {
			Vector3 avg = Vector3.zero;
			if (neighbors.Count == 0)
				return avg;

			for (int i = 0; i < neighbors.Count; i++) {
				avg += neighbors [i].pos;
			}
			avg /= neighbors.Count;

			return avg;
		}
	}

	public Vector3 avgVel {
		get {
			Vector3 avg = Vector3.zero;
			if (neighbors.Count == 0)
				return avg;

			for (int i = 0; i < neighbors.Count; i++) {
				avg += neighbors [i].rigid.velocity;
			}
			avg /= neighbors.Count;

			return avg;
		}
	}

	// within collisionDist from Spawner singleton
	public Vector3 avgClosePos {
		get {
			Vector3 avg = Vector3.zero;
			Vector3 delta;
			int nearCount = 0;
			for (int i = 0; i <neighbors.Count; i++) {
				delta = neighbors [i].pos - transform.position;
				if (delta.magnitude <= Spawner.S.collDist) {
					avg += neighbors [i].pos;
					nearCount++;
				}
			}
			// If there were no neighbors too close, return Vector3.zero
			if (nearCount == 0) return avg;

			// Otherwise, average their locations
			avg /= nearCount;
			return avg;
		}
	}
}
