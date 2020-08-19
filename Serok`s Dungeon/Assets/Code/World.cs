using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public Texture2D levelData; // RGB 24bit, any other way doesnt work.
	public GameObject[,] tileArray;

	public float sizeX = 5;
	public float sizeY = 3.5f;
	public float sizeZ = 5;

	// Use this for initialization
	void Start () 
	{
		GameObject[,] tileArray = new GameObject[128, 128];

		Color32[] colorData = levelData.GetPixels32 ();

		ArrayList walledTiles = new ArrayList();
		ArrayList toBeWalledTiles = new ArrayList();

		for (int i = 0; i < 128; i++)  //Creating Tiles
		{
			for (int j = 0; j < 128; j++) 
			{
				
				byte levelDataR = colorData [i + j * 128].r;
				byte levelDataG = colorData [i + j * 128].g;
				byte levelDataB = colorData [i + j * 128].b;

				if (levelDataR > 0 &&
				    levelDataG > 0 &&
				    levelDataB > 0) {

					GameObject currentObject = new GameObject ();

					currentObject.name = "Tile X " + i + " Z " + j;
					currentObject.transform.parent = this.transform;

					Tile currentTile = currentObject.AddComponent <Tile> ();
					currentObject.transform.position = new Vector3 (sizeX * i, 0, sizeZ * j);

					if (levelDataR == 32 &&
					    levelDataG == 32 &&
					    levelDataB == 32) {

						currentTile.floorLevel = 0;
						currentTile.ceilingLevel = 1;
					}

					if (levelDataR == 64 &&
					    levelDataG == 64 &&
					    levelDataB == 64) {

						currentTile.floorLevel = 0;
						currentTile.ceilingLevel = 2;
					
					}

					if (levelDataR == 96 &&
					    levelDataG == 96 &&
					    levelDataB == 96) {

						currentTile.floorLevel = 0;
						currentTile.ceilingLevel = 3;

					}

					currentTile.xPos = i;
					currentTile.zPos = j;

					GameObject currentFloor = Instantiate ((GameObject)Resources.Load ("FloorPrefab/FloorCave"));
					currentFloor.name = "FloorX" + i + " Z" + j;
					currentFloor.transform.parent = currentObject.transform;
					currentFloor.transform.localPosition = new Vector3 (0, currentTile.floorLevel*sizeY, 0);

					GameObject currentCeiling = Instantiate ((GameObject)Resources.Load ("FloorPrefab/CeilingCave"));
					currentCeiling.name = "CeilingX" + i + " Z" + j;
					currentCeiling.transform.parent = currentObject.transform;
					currentCeiling.transform.localPosition = new Vector3 (0, currentTile.ceilingLevel*sizeY, 0);
						
					tileArray [i, j] = currentObject;
					toBeWalledTiles.Add (currentTile);
				} 
				else 
				{
				
					tileArray [i, j] = null;
				
				}
			}
		}

		//Creating Walls
		foreach (Tile currentTile in toBeWalledTiles) 
		{
			for (int i = -1; i <= 1; i++) 
			{
				for (int j = -1; j <= 1; j++) 
				{
					if (i != j && i != -j) { //Exclude the corner tiles
						
						if ((currentTile.xPos + i) >= 0 && (currentTile.xPos + i) <= 127 && (currentTile.zPos + j) >= 0 && (currentTile.zPos + j <= 127)) {

							if (tileArray [currentTile.xPos + i, currentTile.zPos + j] != null && tileArray [currentTile.xPos + i, currentTile.zPos + j].GetComponent<Tile> () != null) {

								Tile adjTile = tileArray [currentTile.xPos + i, currentTile.zPos + j].GetComponent<Tile> ();

								if (!walledTiles.Contains (adjTile)) {

									int lowerLevel = Mathf.Min (currentTile.floorLevel, adjTile.floorLevel);
									int higherLevel = Mathf.Max (currentTile.ceilingLevel, adjTile.ceilingLevel);
									
									for (int k = lowerLevel; k < higherLevel; k++) {

										if ((k < currentTile.floorLevel && k >= adjTile.floorLevel) ||
										    (k >= currentTile.floorLevel && k < adjTile.floorLevel) ||
										    (k < currentTile.ceilingLevel && k >= adjTile.ceilingLevel) ||
										    (k >= currentTile.ceilingLevel && k < adjTile.ceilingLevel)) {

											GameObject currentWall = Instantiate ((GameObject)Resources.Load ("FloorPrefab/WallCave"));
											currentWall.transform.parent = currentTile.transform;
											currentWall.transform.localPosition = new Vector3 (i * sizeX / 2, (k * sizeY) + sizeY / 2, j * sizeZ / 2);

											float currentAngle = 0;

											if (k < currentTile.floorLevel && k >= adjTile.floorLevel) {
										
												currentAngle = Vector2.Angle (new Vector2 (i, j), new Vector2 (0, -1));
												if (i > 0) {
													currentAngle = -currentAngle;
												}
											}

											if (k >= currentTile.floorLevel && k < adjTile.floorLevel) {
												
												currentAngle = Vector2.Angle (new Vector2 (i, j), new Vector2 (0, 1));
												if (i < 0) {
													currentAngle = -currentAngle;
												}
											}

											if (k < currentTile.ceilingLevel && k >= adjTile.ceilingLevel) {

												currentAngle = Vector2.Angle (new Vector2 (i, j), new Vector2 (0, -1));
												if (i > 0) {
													currentAngle = -currentAngle;
												}
											}

											if (k >= currentTile.ceilingLevel && k < adjTile.ceilingLevel) {

												currentAngle = Vector2.Angle (new Vector2 (i, j), new Vector2 (0, 1));

												if (i < 0) {
													currentAngle = -currentAngle;
												}
											}

											currentWall.transform.localRotation = Quaternion.Euler (90, currentAngle, 0);

										}
									}
								}

							} else {

								for (int k = currentTile.floorLevel; k < currentTile.ceilingLevel; k++) {

									GameObject currentWall = Instantiate ((GameObject)Resources.Load ("FloorPrefab/WallCave"));
									currentWall.transform.parent = currentTile.transform;
									currentWall.transform.localPosition = new Vector3 (i * sizeX / 2, (k * sizeY) + sizeY / 2, j * sizeZ / 2);
									float currentAngle = Vector2.Angle (new Vector2 (i, j), new Vector2 (0, -1));
									if (i > 0) {
										currentAngle = -currentAngle;
									}
									currentWall.transform.localRotation = Quaternion.Euler (90, currentAngle, 0);

								}
							}
						}
					}
				}
			}

			walledTiles.Add (currentTile);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
