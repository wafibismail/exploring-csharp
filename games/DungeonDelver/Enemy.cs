using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	protected static Vector3[] directions = new Vector3[] {
		Vector3.right, Vector3.up, Vector3.left, Vector3.down
	};

	[Header("Set in Inspector: Enemy")]
	public float maxHealth = 1;
	public float knockbackSpeed = 10;
	public float knockbackDuration = 0.25f;
	public float invincibleDuration = 0.5f;
	public GameObject guaranteedItemDrop = null;

	[Header("Set Dynamically: Enemy")]
	public float health;
	public bool invincible = false; // Like in Dray
	public bool knockback = false; // Unlike in Dray where it's an eMode

	private float invincibleDone = 0;
	private float knockbackDone = 0;
	private Vector3 knockbackVel;

	protected Animator anim;
	protected Rigidbody rigid;
	protected SpriteRenderer sRend;

	protected virtual void Awake() {
		// declaring it protected virtual allows it to be overridden in its subclasses
		health = maxHealth;
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();
		sRend = GetComponent<SpriteRenderer> ();
	}

	protected virtual void Update() {
		// Check knockback and invincibility
		if (invincible && Time.time > invincibleDone)
			invincible = false;
		sRend.color = invincible ? Color.red : Color.white;
		if (knockback) {
			rigid.velocity = knockbackVel;
			if (Time.time < knockbackDone)
				return;
		}

		anim.speed = 1;
		knockback = false;
	}

	void OnTriggerEnter( Collider colld) {
		if (invincible) // cannot be damaged
			return;
		DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect> ();
		if (dEf == null) // no DamageEffect
			return;

		health -= dEf.damage;
		if (health <= 0)
			Die ();

		invincible = true;
		invincibleDone = Time.time + invincibleDuration;

		if (dEf.knockback) {
			// Determine the direction of knockback
			Vector3 delta = transform.position - colld.transform.root.position;
			if (Mathf.Abs (delta.x) >= Mathf.Abs (delta.y)) {
				// Knockback should be horizontal
				delta.x = (delta.x > 0) ? 1 : -1;
				delta.y = 0;
			} else {
				// Knockback should be vertical
				delta.x = 0;
				delta.y = (delta.y > 0) ? 1 : -1;
			}

			// Apply knockback speed to Rigidbody
			knockbackVel = delta * knockbackSpeed;
			rigid.velocity = knockbackVel;

			// Set mode to knockback and set time to stop knockback
			knockback = true;
			knockbackDone = Time.time + knockbackDuration;
			anim.speed = 0;
		}
	}
	void Die() {
		GameObject go;
		if (guaranteedItemDrop != null) {
			go = Instantiate<GameObject> (guaranteedItemDrop);
			go.transform.position = transform.position;
		}
		Destroy (gameObject);
	}
}
