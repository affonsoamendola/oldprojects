using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FitnessCounter : MonoBehaviour {

	public Player player;

	public Text textBox;

	void Start(){

		textBox = this.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		
		textBox.text = "Fitness : " + player.specie.fitness;
	
	}
}
