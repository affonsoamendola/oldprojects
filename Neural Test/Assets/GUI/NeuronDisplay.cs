using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NeuronDisplay : MonoBehaviour {

	public GameObject node;
	public Player player;

	public Material lineMaterial;
	public Material nodeMaterial;

	public Vector3 initialPosition;
	public float yOffset;
	public float xOffset;

	public struct Layer{

		public Node[] nodeArray;

	}

	public struct Node{

		public GameObject[] weightLines;
		public GameObject nodeObject;

	}

	public Layer[] layerArray;

	public void Start(){

		layerArray = new Layer[1 + player.hiddenLayerArray.GetLength(0)];

		layerArray [0].nodeArray = new Node[player.detectorArray.GetLength (0)];

		for (int i = 0; i < layerArray [0].nodeArray.GetLength (0); i++) {

			layerArray [0].nodeArray [i].nodeObject = Instantiate (node);
			layerArray [0].nodeArray [i].nodeObject.transform.parent = this.transform;
			layerArray [0].nodeArray [i].nodeObject.transform.localPosition = new Vector3 (initialPosition.x, initialPosition.y + yOffset*i, initialPosition.z);

		}

		for (int i = 1; i < layerArray.GetLength (0); i++) {

			layerArray [i].nodeArray = new Node[player.hiddenLayerSizes [i - 1]];

			for (int j = 0; j < player.hiddenLayerSizes [i - 1]; j++) {
				
				layerArray [i].nodeArray [j].nodeObject = Instantiate (node);
				layerArray [i].nodeArray [j].nodeObject.transform.parent = this.transform;
				layerArray [i].nodeArray [j].nodeObject.transform.localPosition = new Vector3 (initialPosition.x + xOffset*i, initialPosition.y + yOffset*j, initialPosition.z);

				layerArray [i].nodeArray [j].weightLines = new GameObject[layerArray [i - 1].nodeArray.GetLength (0)];

				for (int k = 0; k < layerArray [i - 1].nodeArray.GetLength (0); k++) {

					layerArray [i].nodeArray [j].weightLines [k] = new GameObject ();
					layerArray [i].nodeArray [j].weightLines [k].transform.parent = layerArray [i].nodeArray [j].nodeObject.transform;

					LineRenderer lineRenderer = layerArray [i].nodeArray [j].weightLines [k].AddComponent<LineRenderer> ();
					lineRenderer.useWorldSpace = false;
					lineRenderer.SetWidth (0.01f, 0.01f);
					lineRenderer.material = lineMaterial; 

					lineRenderer.SetPosition (0, new Vector3(layerArray [i].nodeArray [j].nodeObject.transform.position.x, layerArray [i].nodeArray [j].nodeObject.transform.position.y, layerArray [i].nodeArray [j].nodeObject.transform.position.z + 2.0f));
					lineRenderer.SetPosition (1, new Vector3(layerArray [i-1].nodeArray [k].nodeObject.transform.position.x, layerArray [i-1].nodeArray [k].nodeObject.transform.position.y, layerArray [i-1].nodeArray [k].nodeObject.transform.position.z + 2.0f));
				}
			}
		}
	}
		
	public void Update(){
		
		for (int i = 0; i < layerArray.GetLength (0); i++) {

			if (i == 0) {

				for (int j = 0; j < player.detectorArray.GetLength (0); j++) {

					MeshRenderer meshRenderer = layerArray [i].nodeArray [j].nodeObject.GetComponent<MeshRenderer> (); 
					Color currentColor = Color.Lerp (Color.green, Color.red, player.detectorCollisionDistance [j] / 7.0f);
					meshRenderer.material = nodeMaterial;
					meshRenderer.material.color = currentColor;
				}

			} else {
			
				for (int j = 0; j < player.hiddenLayerSizes [i - 1]; j++) {

					MeshRenderer meshRenderer = layerArray [i].nodeArray [j].nodeObject.GetComponent<MeshRenderer> (); 
					Color currentColor = Color.Lerp (Color.red, Color.green, player.hiddenLayerArray [i-1].neuronArray[j].activatedValue);
					meshRenderer.material = nodeMaterial;
					meshRenderer.material.color = currentColor;
				
					for (int k = 0; k < layerArray [i - 1].nodeArray.GetLength (0); k++) {
					
						LineRenderer lineRenderer = layerArray [i].nodeArray [j].weightLines [k].GetComponent<LineRenderer> ();
						lineRenderer.SetColors (Color.Lerp (Color.red, Color.green, player.hiddenLayerArray [i - 1].neuronArray [j].weights [k] / 10.0f), Color.Lerp (Color.red, Color.green, player.hiddenLayerArray [i - 1].neuronArray [j].weights [k] / 10.0f));

					}
				}
			}
		}
	}
}
