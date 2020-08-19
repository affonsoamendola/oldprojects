using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenerationCounter : MonoBehaviour {

	public Evolution evolutionController;

	public Text textBox;

	void Start(){

		textBox = this.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {

		textBox.text = "Generation : " + evolutionController.currentGeneration;

	}
}
