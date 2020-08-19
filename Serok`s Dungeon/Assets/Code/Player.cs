using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Vector3 position;
	public int orientation; //0-N (+z), 1-E (+x), 2-S (-z), 3-W (-x)

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		this.transform.position = position;
		this.transform.rotation = Quaternion.Euler (new Vector3 (0, orientation * 90));
	
	}
}
