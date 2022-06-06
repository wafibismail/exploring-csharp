using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	[Header("Set in Inspector")]
	// These Vector2 variables store Min & Max values
	//   to be used in Random.Range later on
	public Vector2 rotMinMax = new Vector2(15, 90);
	public Vector2 driftMinMax = new Vector2(.25f, 2);
	public float lifeTime = 6f; // Seconds the PowerUp exists
	public float fadeTime = 4f; // Seconds it will then fade

	[Header("Set Dynamically")]
	public WeaponType type; // The type of the PowerUp
	public GameObject cube; // Reference to the Cube child
	public TextMesh letter; // Reference to the TextMesh
	public Vector3 rotPerSecond; // Euler rotation speed
	public float birthTime;

	private Rigidbody rigid;
	private BoundsCheck bndCheck;
	private Renderer cubeRend;

	void Awake() {
		// Find the Cube reference
		cube = transform.Find("Cube").gameObject;
		// Find the TextMesh and other components
		letter = GetComponent<TextMesh>();
		rigid = GetComponent<Rigidbody> ();
		bndCheck = GetComponent<BoundsCheck> ();
		cubeRend = cube.GetComponent<Renderer> ();

		// Set a random velocity
		Vector3 vel = Random.onUnitSphere; // Get Random XYZ velocity
		vel.z = 0; // Flatten the vel to the XY plane
		vel.Normalize(); // Makes its length 1m

		vel *= Random.Range (driftMinMax.x, driftMinMax.y); // NOTE: not coords
		rigid.velocity = vel;

		// Set the rotation of this GameObject to R:[0, 0, 0]
		transform.rotation = Quaternion.identity;
		// Quaternion.identity is equal to no rotation.

		// Set up the rotPerSecond for the Cube child using rotMinMax x & y
		rotPerSecond = new Vector3 (
			Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y)
		);

		birthTime = Time.time;
	}

	void Update() {
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);

		// Fade out the PowerUp over time
		// Given the default values, a PowerUpwil exist for 10 seconds
		// and then fade out over 4 seconds.

		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
		// u would be <= 0 for the entire lifeTime duration
		// then transition to 1 over the course of the fadeTime duration

		// Once u reaches 1, destroy this PowerUp
		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}

		// Determine alpha value of Cube & letter using u
		if (u > 0) {
			Color c = cubeRend.material.color;
			c.a = 1f - u;
			cubeRend.material.color = c;
			c = letter.color;
			c.a = 1f - (u * 0.5f);
			letter.color = c;
		}

		if (!bndCheck.isOnScreen) {
			// If the PowerUp has drifted entirely off screen, destroy it
			Destroy (gameObject);
		}
	}

	public void SetType(WeaponType wt) {
		// Grab the WeaponDefinition from Main
		WeaponDefinition def = Main.GetWeaponDefinition( wt );

		cubeRend.material.color = def.color;
		letter.color = def.color;
		letter.text = def.letter;
		type = wt;
	}

	// To be called by the HeroClass when the PowerUp is collected
	public void AbsorbedBy (GameObject target) {
		Destroy (this.gameObject);
	}
}
