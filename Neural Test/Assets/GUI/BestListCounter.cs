using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestListCounter : MonoBehaviour {

	public Player player;
	public Evolution evolutionController;

	public Text textBox;

	void Start(){

		textBox = this.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		
		textBox.text = "Best List \n" + "--------------------------------------\n";

		for (int i = 0; i < evolutionController.bestList.GetLength (0); i++) {

			if (evolutionController.bestList [i] != null) {

				textBox.text = textBox.text + (i + 1) + ". \t" + evolutionController.bestList [i].generation + "_" + evolutionController.bestList [i].specie + "              " + evolutionController.bestList [i].fitness + "\n";

			} else {

				textBox.text = textBox.text + (i + 1) + ". \t" + "null" + "\n";

			}
		}
	}
}
