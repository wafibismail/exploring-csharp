using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour {
	// The static point of interest
	//   that the camera should follow
	// Having it as static i.e. accessible anywhere
	//   as FollowCam.POI makes it easy for Slingshot
	//   code to tell _MainCamera which Projectile to follow
	static public GameObject POI;

	[Header("Set in Inspector")]
	public float easing = 0.05f;
	public Vector2 minXY = Vector2.zero;

	[Header("Set Dynamically")]
	public float camZ; // The desired z pos of the camera

	void Awake() {
		camZ = this.transform.position.z;
	}

	// Use FixedUpdate() instead of Update() as the projectile is moved by
	//   the PhysX engine which in turn updates in sync with FixedUpdate
	void FixedUpdate() {
		if (POI == null)
			return;

		// Get the position of the poi
		Vector3 destination = POI.transform.position;
		// Limit the X & Y to minimum values
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);
		// Interpolate from the current Camera position toward destination
		destination = Vector3.Lerp(transform.position, destination, easing);
		// Force destination.z to be camZ to keep the camera far enough away
		//   i.e. not too close to POI that POI fills the entire frame
		destination.z = camZ;
		// Set the camera to the destination
		transform.position = destination;
		// Set the orthographicSize of the Camera to keep Ground in view
		//  This works due to destination.y never being below 0
		//  So the orthographicSize is always >= 10
		//    keeping the Ground which is at y=-10 in view
		Camera.main.orthographicSize = destination.y + 10;
	}
}
