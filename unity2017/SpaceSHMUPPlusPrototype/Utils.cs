using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	//========MATERIALS FUNCTIONS========\\

	// Returns a list of all Materials on this GameObject and its children
	static public Material[] GetAllMaterials( GameObject go ) {
		                   // this iterates over go + its children
		Renderer[] rends = go.GetComponentsInChildren<Renderer> ();

		List<Material> mats = new List<Material> ();
		foreach (Renderer rend in rends) {
			mats.Add (rend.material);
		}

		return (mats.ToArray ());
	}
}
