using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZig : Enemy {
	public override void Move() {
		Vector3 tempPos = pos;
		tempPos.x = Mathf.Sin (Time.time * Mathf.PI * 2) * 4;
		pos = tempPos; // uses pos property on the superclass
		base.Move (); // calls Move() on the superclass
	}
}
