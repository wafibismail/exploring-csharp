using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner3 : MonoBehaviour {
	public GameObject cubePrefabVar;
	public List<GameObject> gameObjectList; // to hold all the cubes
	public float scalingFactor = 0.95f;

	public int numCubes = 0;

	// Use this for initialization
	void Start () {
		gameObjectList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		numCubes++;

		GameObject gObj = Instantiate<GameObject> (cubePrefabVar);

		gObj.name = "Cube " + numCubes;
		Color c = new Color(Random.value, Random.value, Random.value);
		gObj.GetComponent<Renderer>().material.color = c;
		// Gets the Renderer component of gObj & gives gObj a random color
		gObj.transform.position = Random.insideUnitSphere;

		gameObjectList.Add (gObj);
	}
}
