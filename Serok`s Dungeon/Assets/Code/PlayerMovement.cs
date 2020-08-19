using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Player player;

	public int tileSize;
	public float movSpeed;
	public float turnSpeed;

	public Vector3 startPos;
	public Vector3 futurePos;

	public Quaternion startRotation;
	public Quaternion futureRotation;

	public float timeAtBeginingOfMovement;

	public bool moving;
	public bool turning;

	public enum movementCode
	{
		Up,
		Down,
		Left,
		Right,
		TurnRight,
		TurnLeft

	};

	public movementCode nextMove;
	public bool nextMoveLoaded;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if ((Input.GetButtonUp ("Up") || (nextMoveLoaded == true && nextMove == movementCode.Up)) && moving == false) {

			futurePos = (player.position / tileSize + Quaternion.AngleAxis (90 * player.orientation, Vector3.up) * (new Vector3 (0, 0, 1))) * tileSize;
			moving = true;
			nextMoveLoaded = false;
			startPos = player.position;
			timeAtBeginingOfMovement = Time.time;

		} else if (Input.GetButtonUp ("Up") && moving == true && nextMoveLoaded == false) {

			nextMove = movementCode.Up;
			nextMoveLoaded = true;

		}

		if ((Input.GetButtonUp ("Down") || (nextMoveLoaded == true && nextMove == movementCode.Down)) && moving == false) {

			futurePos = (player.position/tileSize + Quaternion.AngleAxis (90 * player.orientation, Vector3.up) * (new Vector3 (0, 0, -1))) * tileSize;
			moving = true;
			nextMoveLoaded = false;
			startPos = player.position;
			timeAtBeginingOfMovement = Time.time;

		} else if (Input.GetButtonUp ("Down") && moving == false && nextMoveLoaded == false) {

			nextMove = movementCode.Down;
			nextMoveLoaded = true;

		}

		if ((Input.GetButtonUp ("Left") || (nextMoveLoaded == true && nextMove == movementCode.Left)) && moving == false) {

			futurePos = (player.position/tileSize + Quaternion.AngleAxis (90 * player.orientation, Vector3.up) * (new Vector3 (-1, 0, 0))) * tileSize;
			moving = true;
			nextMoveLoaded = false;
			startPos = player.position;
			timeAtBeginingOfMovement = Time.time;

		} else if (Input.GetButtonUp ("Left") && moving == false && nextMoveLoaded == false) {

			nextMove = movementCode.Left;
			nextMoveLoaded = true;

		}

		if ((Input.GetButtonUp ("Right") || (nextMoveLoaded == true && nextMove == movementCode.Right)) && moving == false) {

			futurePos = (player.position/tileSize + Quaternion.AngleAxis (90 * player.orientation, Vector3.up) * (new Vector3 (1, 0, 0))) * tileSize;
			moving = true;
			nextMoveLoaded = false;
			startPos = player.position;
			timeAtBeginingOfMovement = Time.time;

		} else if (Input.GetButtonUp ("Right") && moving == false && nextMoveLoaded == false) {

			nextMove = movementCode.Right;
			nextMoveLoaded = true;

		}

		if ((Input.GetButtonUp ("TurnRight") || (nextMoveLoaded == true && nextMove == movementCode.TurnRight)) && moving == false) {

			moving = true;
			turning = true;
			nextMoveLoaded = false;
			startPos = player.position;
			timeAtBeginingOfMovement = Time.time;

		} else if (Input.GetButtonUp ("TurnRight") && moving == false && nextMoveLoaded == false) {

			nextMove = movementCode.TurnRight;
			nextMoveLoaded = true;

		}


	}

	void FixedUpdate(){

		if (moving == true) {

			float currentLerpStep = (Time.time - timeAtBeginingOfMovement)/movSpeed;

			player.position = Vector3.Lerp (startPos, futurePos, currentLerpStep);

			if (player.position == futurePos) {
				moving = false;
			}
		}
	}
}
