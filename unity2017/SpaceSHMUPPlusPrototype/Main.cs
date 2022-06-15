using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// For loading & reloading of scenes:
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	static public Main S; // A singleton for Main
	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

	[Header("Set in Inspector")]
	public GameObject[] prefabEnemies; // Array of Enemy prefabs
	public float enemySpawnPerSecond = 0.5f; // # Enemies/second
	public float enemyDefaultPadding = 1.5f; // Padding for position
	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] {
		WeaponType.blaster, WeaponType.blaster,
		WeaponType.spread, WeaponType.shield
	};

	private BoundsCheck bndCheck;

	public void shipDestroyed(Enemy e) {
		// Potentially generate a PowerUp
		if (Random.value <= e.powerUpDropChance) {
			int ndx = Random.Range (0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency [ndx];

			// Spawn a PowerUp
			GameObject go = Instantiate (prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();
			// Set it to the proper WeaponType
			pu.SetType (puType);

			// Set it to the position of the destroyed ship
			pu.transform.position = e.transform.position;
		}
	}

	void Awake() {
		S = this;
		bndCheck = GetComponent<BoundsCheck> ();

		// Invoke SpawnEnemy() once in 2 seconds (based on default values)
		Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);

		// A generic Dictionary with WeaponType as the key
		WEAP_DICT = new	Dictionary<WeaponType, WeaponDefinition>();
		foreach (WeaponDefinition def in weaponDefinitions) {
			WEAP_DICT [def.type] = def;
		}
	}

	public void SpawnEnemy() {
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate<GameObject> (prefabEnemies [ndx]);

		// Position the Enemy above the screen with a random x position
		float enemyPadding = enemyDefaultPadding;
		if (go.GetComponent<BoundsCheck> () != null) {
			enemyPadding = Mathf.Abs (go.GetComponent<BoundsCheck> ().radius);
		}

		// Set the initial position of the spawned Enemy
		//   making sure it is on screen horizontally
		//   and just above the screen in the y axis
		Vector3 pos = Vector3.zero;
		float xMin = -bndCheck.camWidth + enemyPadding;
		float xMax = bndCheck.camWidth - enemyPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = bndCheck.camHeight + enemyPadding;
		go.transform.position = pos;

		// Invoke SpawnEnemy() again
		// Alternatively, can use InvokeRepeating
		//   but then the invoke frequency would be fixed from start to end
		Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);
	}

	public void DelayedRestart( float delay ) {
		Invoke ("Restart", delay);
	}

	public void Restart() {
		SceneManager.LoadScene ("_Scene_0");
	}

	/// <summary>
	/// Static function that gets a WeaponDefinition from the WEAP_DICT static
	/// protected field of the Main class.
	/// </summary>
	/// <returns>The WeaponDefinition or, if there is no WeaponDefinition with
	/// the WeaponType passed in, returns a new WeaponDefinition with a
	/// WeaponType of none..</returns>
	/// <param name="wt">The WeaponType of the desired WeaponDefinition</param>
	static public WeaponDefinition GetWeaponDefinition( WeaponType wt) {
		// Need to check if key exists in WEAP_DICT
		// Attempting to retrieve a key that does not exist would throw an error
		if (WEAP_DICT.ContainsKey (wt)) {
			return (WEAP_DICT [wt]);
		}
		// This returns a WeaponDefinition with a type of WeaponType.none,
		// which means it has failed to find the right WeaponDefinition
		return (new WeaponDefinition ());
	}
}
