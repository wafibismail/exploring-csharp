using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up.
/// Items marked [NI] below are not implemented here nor in the book
/// </summary>
public enum WeaponType {
	none, // The default / no weapon
	blaster, // A simple blaster
	spread, // Two shots simultaneously
	phaser, // [NI] Shots that move in waves
	missile, // [NI] Homing missiles
	laser, // [NI] Damage over time
	shield // Raise shieldLevel
}

/// <summary>
/// The WeaponDefinition class allows setting the properties of
/// a specific weapon in the Inspector. The Main class has an array
/// of WeaponDefinitions that makes this possible.
/// </summary>
// Making it serializable allows for the fields to be edited
//   within the Unity Inspector
[System.Serializable]
public class WeaponDefinition { // not a subclass of MonoBehaviour
	public WeaponType type = WeaponType.none;
	public string letter; // Letter to show on the power-up
	public Color color = Color.white; // Color of collar & power-up
	public GameObject projectilePrefab;
	public Color projectileColor = Color.white;
	public float damageOnHit = 0; // Amount of damage caused
	public float continuousDamage = 0; // Damage per second (Laser)
	public float delayBetweenShots = 0;
	public float velocity = 20; // Speed of projectiles
}

public class Weapon : MonoBehaviour {
	
}
