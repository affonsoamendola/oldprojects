using UnityEngine;
using System.Collections;

public class WorldGeneration : MonoBehaviour {

	public Player player;

	public worldPiece[] gameWorld;

	public GameObject cube;

	public int worldSize = 20;

	public struct worldPiece{

		public worldPieceType type;
		public float xPos;

		public GameObject[] pieceGameObjects;
	}

	public enum worldPieceType{
		wall,
		ground,
		hole
	};

	public worldPiece newestPiece;

	// Use this for initialization
	public void Start () {
	
		newestPiece = new worldPiece (){ xPos = -1.0f };

		gameWorld = new worldPiece[worldSize];

		for (int i = 0; i < gameWorld.GetLength(0); i++) {

			gameWorld [i] = createPiece ();

			newestPiece = gameWorld [i];
		}
	}

	worldPieceType generatePieceType(){

		float randomNumber = Random.value;

		if (randomNumber < 0.05) {

			return worldPieceType.wall;
		
		} else if (randomNumber < 0.1) {

			return worldPieceType.hole;

		} else {
			
			return worldPieceType.ground;
		
		}
	}

	worldPiece createPiece(){

		worldPiece beingCreated = new worldPiece();
		beingCreated.type = generatePieceType ();

		beingCreated.xPos = newestPiece.xPos + 1.0f;

		if (beingCreated.type == worldPieceType.ground) {

			beingCreated.pieceGameObjects = new GameObject[1];
			beingCreated.pieceGameObjects [0] = Instantiate (cube);
			beingCreated.pieceGameObjects [0].transform.position = new Vector3 (beingCreated.xPos, 0, 0);

		}else if (beingCreated.type == worldPieceType.wall) {

			beingCreated.pieceGameObjects = new GameObject[3];

			for(int i = 0; i < 3; i ++){

				beingCreated.pieceGameObjects[i] = Instantiate(cube);
				beingCreated.pieceGameObjects[i].transform.position = new Vector3 (beingCreated.xPos, i, 0);
			}

		}else if (beingCreated.type == worldPieceType.hole) {

			beingCreated.pieceGameObjects = null;

		} else {
			
			Debug.Log ("Shit Happened at create World, bitch, you suck at coding");
			beingCreated.pieceGameObjects = null;
		}

		return beingCreated;
	}

	public void destroyPiece(int index, bool generateNew){
		//destroy and generate new piece on the side of the newest one

		worldPiece toBeDestroyed = gameWorld [index];
		if (toBeDestroyed.type != worldPieceType.hole) {
			
			for (int i = 0; i < toBeDestroyed.pieceGameObjects.GetLength (0); i++) {

				Destroy (toBeDestroyed.pieceGameObjects [i]);

			}

		}

		if (generateNew == true) {
		
			gameWorld [index] = createPiece ();

			newestPiece = gameWorld [index];

		}
	}

	public void destroyWorld(){

		for (int l = 0; l < gameWorld.GetLength(0); l++) {

			destroyPiece (l, false);

		}
	}

	
	// Update is called once per frame
	void Update () {
		
		for (int i = 0; i < gameWorld.GetLength(0); i++) {

			if ((gameWorld [i].xPos - player.transform.position.x) < -2) {

				destroyPiece (i, true);

			}
		}
	}
}
