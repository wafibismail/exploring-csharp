using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speed = 10f; // in m/s
	public float firerate = 0.3f; // in Shots/second
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public virtual void Move() {
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	void onCollisionEnter( Collision coll ) {
		GameObject other = coll.gameObject;
		switch (other.tag) {
		case "Hero":
			break;

		case "HeroLaser":
			Destroy(this.gameObject);
			break;
		}
	}

	public Vector3 pos {
		get {
			return (this.transform.position);
		}
		set {
			this.transform.position = value;
		}
	}
}
