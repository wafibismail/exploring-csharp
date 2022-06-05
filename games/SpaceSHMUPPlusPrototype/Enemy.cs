﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[Header("Set in Inspector: Enemy")]
	public float speed = 10f;
	public float fireRate = 0.3f; // seconds per shot
	public float health = 10;
	public int score = 100; // points earned for destroying this
	public float showDamageDuration = 0.1f; // # seconds to show damage

	[Header("Set Dynamically")]
	public Color[] originalColors;
	public Material[] materials; // All the materials of this & its children
	public bool showingDamage = false;
	public float damageDoneTime; // Time to stop showing damage
	public bool notifiedOfDestruction = false; // Will be used later

	protected BoundsCheck bndCheck;

	void Awake() {
		bndCheck = GetComponent<BoundsCheck> ();
		// Get materials and colors for this GameObject and its children
		materials = Utils.GetAllMaterials( gameObject );
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}
	}

	public Vector3 pos {
		get { return (this.transform.position); }
		set { this.transform.position = value; }
	}

	void Update() {
		Move ();

		if (showingDamage && Time.time > damageDoneTime) {
			UnShowDamage ();
		}

		if (bndCheck != null && bndCheck.offDown) {
			Destroy (gameObject);
		}
	}

	public virtual void Move() {
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	void OnCollisionEnter (Collision coll) {
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag) {
		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile> ();
			// If this Enemy is off screen, don't damage it.
			if (!bndCheck.isOnScreen) {
				Destroy (otherGO);
				break;
			}

			// Hurt this Enemy
			ShowDamage();
			// Get the damage amount from the Main WEAP_DICT
			health -= Main.GetWeaponDefinition (p.type).damageOnHit;
			if (health <= 0) {
				Destroy (this.gameObject);
			}
			Destroy (otherGO);
			break;

		default:
			print ("Enemy hit by non-ProjectileHero: " + otherGO.name);
			break;
		}
	}

	void ShowDamage() {
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage() {
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
		showingDamage = false;
	}
}
