using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParentCounter : MonoBehaviour {

	public Player player;

	public Text textBox;

	void Start(){

		textBox = this.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		
		if (player.specie.generation != 0) {

			textBox.text = "Parent 1 : " + player.specie.parent1.generation + "_" + player.specie.parent1.specie + "  " + player.specie.parent1.fitness + "\n" +
			"Parent 2 : " + player.specie.parent2.generation + "_" + player.specie.parent2.specie + "  " + player.specie.parent2.fitness;

		}
	}
}
