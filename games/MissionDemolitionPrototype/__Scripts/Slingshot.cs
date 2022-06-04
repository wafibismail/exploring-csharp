using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour {
	static private Slingshot S;

	// Code in [] is a compiler attribute which gives either
	//   Unity or the compiler a specific instruction.
	// In this case, it instructs unity to create a header
	//   in the Inspector view of this script

	[Header("Set in Inspector")]
	public GameObject prefabProjectile;
	public float velocityMult = 12f;

	[Header("Set Dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	private Rigidbody projectileRigidbody;

	static public Vector3 LAUNCH_POS {
		get {
			if (S == null)
				return Vector3.zero;
			return S.launchPos;
		}
	}

	void Awake() {
		S = this;
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		// Tells the game whether to ignore them
		// i.e. render on screen, and 
		//      receive any calls e.g. Update, OnCollisionEnter
		launchPoint.SetActive (false);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter() {
		//print ("Slingshot:OnMouseEnter()");
		launchPoint.SetActive (true);
	}

	void OnMouseExit() {
		//print ("Slingshot:OnMouseExit()");
		launchPoint.SetActive (false);
	}

	void OnMouseDown() {
		aimingMode = true;
		projectile = Instantiate (prefabProjectile) as GameObject;
		projectile.transform.position = launchPos;
		projectileRigidbody = projectile.GetComponent<Rigidbody> ();
		projectileRigidbody.isKinematic = true;
	}

	void Update() {
		// If Slingshot is not in aimingMode, don't run this code
		if (!aimingMode) return;

		// Get the current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

		// Find the delta from the launchPos to the mousePos3D
		Vector3 mouseDelta = mousePos3D - launchPos;
		// Limit mouseDelta to the radius of the Slingshot SphereCollider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize ();
			mouseDelta *= maxMagnitude;
		}

		// Move the projectile to this new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		// true only on the first frame that the 0th mouse button is released
		if (Input.GetMouseButtonUp (0)) {
			aimingMode = false;
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = -mouseDelta * velocityMult;
			// Open the field projectile to be filled by another instance
			FollowCam.POI = projectile;
			projectile = null;
		}
	}
}
