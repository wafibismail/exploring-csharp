using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Basket : MonoBehaviour {
	[Header("Set Dynamically")]
	public Text scoreGT;

	void Start() {
		// Find a reference to the ScoreCounter GameObject
		GameObject scoreGO = GameObject.Find("ScoreCounter");
		// Get the Text Component of that GameObject
		scoreGT = scoreGO.GetComponent<Text>();
		// Set the starting number of points to 0
		scoreGT.text = "0";
	}
	
	void Update () {
		// Get the current screen position of the mouse from Input
		//  - which is in screen coordinates (from the top left corner)
		//  - z is always 0
		Vector3 mousePos2D = Input.mousePosition;

		// The Camera's z position sets how far to push the mouse into the 3D
		mousePos2D.z = -Camera.main.transform.position.z;

		// Convert the point from 2D screen space into 3D game world space
		// * If mousePos2D's z were 0, the resulting mousePos3D's z would
		//   be the same as Main Camera's z e.g. -10.
		// * Setting it as the negative value of Main Camera's z therefore
		//   results in mousePos3D's to be 0 i.e. 10 units away from the
		//   Main Camera
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		// Move the x position of this Basket to the x position of the Mouse
		Vector3 pos = this.transform.position;
		pos.x = mousePos3D.x;
		this.transform.position = pos;
	}

	void OnCollisionEnter( Collision coll) {
		// Find out what hit this basket
		GameObject collidedWith = coll.gameObject;
		if (collidedWith.tag == "Apple") {
			Destroy (collidedWith);

			// Parse the text of the scoreGT into an int
			int score = int.Parse(scoreGT.text);
			// Add points for catching the apple
			score += 100;
			// Convert the score back to a string and display it
			scoreGT.text = score.ToString();

			// Track the high score
			if (score > HighScore.score) {
				HighScore.score = score;
			}
		}
	}
}
