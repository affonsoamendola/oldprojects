using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int currentXPos;
	public int currentYPos;

	public int nextXPos;
	public int nextYPos;

	// Use this for initialization
	void Start () {

		currentXPos = 0;
		currentYPos = 0;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (nextXPos == currentXPos && nextYPos == currentYPos) {

			if (Input.GetAxis ("Vertical") > 0.5) {

				nextYPos = currentYPos + 1;

			}

			if (Input.GetAxis ("Vertical") < -0.5) {

				nextYPos = currentYPos - 1;

			}

			if (Input.GetAxis ("Horizontal") > 0.5) {

				nextXPos = currentXPos + 1;

			}

			if (Input.GetAxis ("Horizontal") < -0.5) {

				nextXPos = currentXPos - 1;

			}
		}

		if ((nextXPos != currentXPos) && Input.GetAxis("Horizontal") == 0) {

			currentXPos = nextXPos;
		
		}

		if ((nextYPos != currentYPos) && Input.GetAxis("Vertical") == 0) {

			currentYPos = nextYPos;

		}

		this.transform.position = new Vector3 (currentXPos, currentYPos);
	
	}
}
