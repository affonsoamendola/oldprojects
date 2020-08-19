using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Evolution : MonoBehaviour {

	public int currentGeneration = 0;
	public int currentSpecies = 0;

	public int generationSize = 10;

	public int DNASize;

	public Specie[] speciesList;

	public Specie[] bestList;

	public Dictionary<int, Specie[]> generationList; 

	public class Specie{

		public int generation;
		public int specie;

		public Specie parent1;
		public Specie parent2;
	
		public float[] DNA;
		public float fitness;

	}

	void Start(){

		generationList = new Dictionary<int, Specie[]> ();
		generationList [currentGeneration] = new Specie[generationSize];

		bestList = new Specie[10];

	}

	void Update(){

	}

	public float maxFitness;
	public float[] chances;

	public Specie getParent(){

		maxFitness = bestList [0].fitness;
		chances = new float[bestList.GetLength (0)];

		for (int i = 0; i < bestList.GetLength (0); i++) {

			if (i == 0) {

				chances [i] = bestList [i].fitness / maxFitness;

			} else {

				chances [i] = chances [i-1] + bestList [i].fitness / maxFitness;

			}
		}

		float randomNumber = Random.Range (0, chances [bestList.GetLength (0) - 1]);

		for (int i = 0; i < bestList.GetLength (0); i++) {

			if (i == 0) {

				if (randomNumber > 0 && randomNumber <= chances [i]) {

					return bestList [i];

				}

			} else {

				if (randomNumber > chances[i-1] && randomNumber <= chances [i]) {

					return bestList [i];

				}
			}
		}
		return null;
	}
}
