using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public CharacterController controller;
	public WorldGeneration worldGen;

	public bool jumped = false;
	public bool movingRight = false;
	public bool movingLeft = false;

	public bool isGrounded;

	public float movementForce = 0.1f;
	public float jumpSpeed = 0.2f;
	public float gravity = -0.1f;
	public float drag = -0.2f;
	public float maxVelocity = 0.5f;

	public Evolution evolutionController;

	public Evolution.Specie specie;

	public Transform[] detectorArray = new Transform[16];
	public float[] detectorCollisionDistance = new float[16];

	public int[] hiddenLayerSizes; 

	public hiddenLayer[] hiddenLayerArray;

	public float deathCounter = 0;
	public float dieAtCounter = 10f;

	public struct neuron{

		public float activatedValue;		
		public float weightedValue;
		public float[] weights;

	}

	public struct hiddenLayer{

		public neuron[] neuronArray; 

	}

	// Use this for initialization
	void Start () {

		hiddenLayerArray = new hiddenLayer[hiddenLayerSizes.GetLength(0)];

		int currentGene = 0; 

		specie = new Evolution.Specie ();

		specie.generation = evolutionController.currentGeneration;
		specie.specie = evolutionController.currentSpecies;

		if (evolutionController.currentGeneration == 0) {

			specie.DNA = generateRandomDNA ();

		} else {

			specie.parent1 = evolutionController.getParent ();
			specie.parent2 = evolutionController.getParent ();
			specie.DNA = generateDNAfromParents (specie.parent1, specie.parent2);

		}
			
		for (int i = 0; i < hiddenLayerSizes.GetLength(0); i++) {

			hiddenLayerArray [i].neuronArray = new neuron[hiddenLayerSizes [i]];

			for (int j = 0; j < hiddenLayerArray [i].neuronArray.GetLength (0); j++) {

				if (i != 0) {
					
					hiddenLayerArray [i].neuronArray [j].weights = new float[hiddenLayerArray [i - 1].neuronArray.GetLength (0)];
				
				} else {

					hiddenLayerArray [i].neuronArray [j].weights = new float[16];

				}

				for (int k = 0; k < hiddenLayerArray [i].neuronArray [j].weights.GetLength (0); k++) {
						
					hiddenLayerArray [i].neuronArray [j].weights [k] = specie.DNA [currentGene];
					currentGene += 1;
					
				}
			}
		}
	}

	float[] generateRandomDNA(){

		float[] currentDNA = new float[evolutionController.DNASize];

		for (int i = 0; i < currentDNA.GetLength(0); i++) {

			currentDNA[i] = Random.Range (-10.0f, 10.0f);

		}

		return currentDNA;
	}

	float[] generateDNAfromParents (Evolution.Specie parent1, Evolution.Specie parent2){

		float[] currentDNA = new float[evolutionController.DNASize];

		for (int i = 0; i < currentDNA.GetLength(0); i++) {

			if (Random.Range (0f, 1f) > 0.5f) {

				currentDNA [i] = parent1.DNA [i];

				if (Random.Range (0f, 100f) < 0.5f) {

					currentDNA [i] = Random.Range (-10.0f, 10.0f);

				}

			}else{
				
				currentDNA [i] = parent2.DNA [i];

				if (Random.Range (0f, 100f) < 0.5f) {

					currentDNA [i] = Random.Range (-10.0f, 10.0f);

				}
			}
		}

		return currentDNA;
	}
		
	// Update is called once per frame
	void Update () {

		//Debug.ClearDeveloperConsole ();

		for(int i = 0; i < detectorArray.GetLength(0);i++){

			RaycastHit hit;
			Physics.Linecast (this.transform.position, detectorArray [i].transform.position, out hit);
			detectorCollisionDistance[i] = hit.distance;

		}

		for (int i = 0; i < hiddenLayerSizes.GetLength(0); i++) {

			for (int j = 0; j < hiddenLayerArray [i].neuronArray.GetLength (0); j++) {
				
				hiddenLayerArray [i].neuronArray[j].weightedValue = 0;

				for (int k = 0; k < hiddenLayerArray [i].neuronArray [j].weights.GetLength (0); k++) {

					hiddenLayerArray [i].neuronArray[j].weightedValue += hiddenLayerArray [i].neuronArray[j].weights[k] * detectorCollisionDistance[k];

				}

				//Debug.Log (hiddenLayerArray [i].neuronArray [j].weightedValue);

				hiddenLayerArray [i].neuronArray[j].activatedValue = 1 / (1 + Mathf.Exp (hiddenLayerArray [i].neuronArray[j].weightedValue));

				//Debug.Log (hiddenLayerArray [i].neuronArray [j].activatedValue);

			}

			if (controller.velocity.x < 0.1f) {

				deathCounter += 0.1f;

				if (deathCounter > dieAtCounter) {

					die ();

				}

			} else {

				deathCounter = 0;

			}

			if (this.transform.position.y < -2.0f) {

				die ();

			}
		}
			
		isGrounded = controller.isGrounded;
		/*
		if (Input.GetKeyDown (KeyCode.UpArrow) && isGrounded && !jumped) {

			jumped = true;

		}

		if (Input.GetKey (KeyCode.RightArrow) && movingLeft == false) {

			movingRight = true;
		
		} else {

			movingRight = false;

		}

		if (Input.GetKey (KeyCode.LeftArrow) && movingRight == false) {

			movingLeft = true;

		} else {

			movingLeft = false;

		}*/

		if (hiddenLayerArray [hiddenLayerArray.GetLength (0) - 1].neuronArray [0].activatedValue > 0.7f) {

			movingRight = true;

		} else {
			
			movingRight = false;
		
		}


		if (hiddenLayerArray [hiddenLayerArray.GetLength (0) - 1].neuronArray [1].activatedValue > 0.7f) {

			movingLeft = true;

		} else {

			movingLeft = false;

		}

		if (hiddenLayerArray [hiddenLayerArray.GetLength (0) - 1].neuronArray [2].activatedValue > 0.7f && isGrounded == true && !jumped) {

			jumped = true;

		} else {

			jumped = false;

		}

		specie.fitness = this.transform.position.x;

	}

	public void die(){

		deathCounter = 0;

		if (evolutionController.currentSpecies < evolutionController.generationSize) {

			evolutionController.generationList [evolutionController.currentGeneration][evolutionController.currentSpecies] = this.specie;
			evolutionController.currentSpecies += 1;
		
		} else {

			evolutionController.currentSpecies = 0;
			evolutionController.currentGeneration += 1;
			evolutionController.generationList [evolutionController.currentGeneration] = new Evolution.Specie[evolutionController.generationSize] ;

		}

		for (int i = 0; i < evolutionController.bestList.GetLength (0); i++) {

			if (evolutionController.bestList [i] == null || evolutionController.bestList[i].fitness < this.specie.fitness) {
				
				for (int j = evolutionController.bestList.GetLength (0) - 2; j > i; j--) {

					evolutionController.bestList [j] = evolutionController.bestList [j - 1];

				}

				evolutionController.bestList [i] = this.specie;

				break;
			}
		}

		this.transform.position = new Vector3 (0, 2, 0);

		worldGen.destroyWorld ();

		worldGen.Start ();

		Start ();
	
	}

	public Vector3 velocity = new Vector3(0,0,0);

	void FixedUpdate () {

		if (jumped) {
			
			velocity.y = jumpSpeed;
			jumped = false;

		} else {

			velocity.y = velocity.y + gravity;

			if (controller.isGrounded) {

				velocity.y = velocity.y - gravity;

			}
		}

		if (movingRight) {

			if (velocity.x < maxVelocity) {

				velocity.x = velocity.x + movementForce;

			}
		}

		if (movingLeft) {
			
			if (velocity.x > -maxVelocity) {

				velocity.x = velocity.x - movementForce;

			}
		}

		if (movingLeft == false && movingRight == false) {

			if (velocity.x > 0) {
				
				velocity.x = velocity.x + drag;

				if (velocity.x < 0) {

					velocity.x = 0;

				}
			} else if (velocity.x < 0) {

				velocity.x = velocity.x - drag;

				if (velocity.x > 0) {

					velocity.x = 0;

				}
			}
		}

		controller.Move (velocity);
	}
}
