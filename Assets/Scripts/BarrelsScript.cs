﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelsScript : MonoBehaviour {

	public GameObject barrelPrefab;
	public Canvas canvas;
	public Material redColorBarrelMaterial;

	private List<float> angle;
	private List<string> orderOfSelection;
	
	private MeshRenderer meshRenderer;

	private GameObject[] barrels = new GameObject[12];
	private GameObject selectedBarrel;
	private ClickBarrel[] clickedBarrel;
	private angleScript angScript;

	// Use this for initialization
	void Start () {
		randomPlacement (12f, 12f, 250f, 250f, 0, 3);
		randomPlacement (255f, 12f, 485f, 250f, 3, 6);
		randomPlacement (12f, 255f, 250f, 485f, 6, 9);
		randomPlacement (255f, 255f, 485f, 485f, 9, 12);
		canvas.enabled = false;

		clickedBarrel = gameObject.GetComponentsInChildren<ClickBarrel> ();
		angScript = canvas.GetComponentInChildren<angleScript> ();
	
	}


	private void randomPlacement(float lowX,float lowZ,float highX,float highZ,int indexLow, int indexHigh){
		Collider[] colliders;
		Vector3 position;
		for (int i = indexLow; i < indexHigh; i++) {
			position = new Vector3 (Random.Range (lowX, highX), 0.0f, Random.Range (lowZ, highZ));
			colliders = Physics.OverlapSphere (position, 1.0F);
			while (colliders.Length != 0) {
				Debug.Log ("that place is taken!");
				position = new Vector3 (Random.Range (lowX, highX), 7.0f, Random.Range (lowZ, highZ));
				colliders = Physics.OverlapSphere (position, 1.0F);
			}
			barrels [i] = Instantiate (barrelPrefab, position - new Vector3(0.0F,7.0F,0.0F), Quaternion.identity) as GameObject;
			barrels[i].name = i.ToString();
//			barrels[i].layer = 9;
		}
	}

	void MoveToLayer(Transform root, int layer) {
		root.gameObject.layer = layer;
		foreach(Transform child in root)
			MoveToLayer(child, layer);
	}

	// Update is called once per frame
	void Update () {
		int selectedBarrelIndex;
		foreach (ClickBarrel clicked in clickedBarrel) {
			if(clicked.barrelFound == 1){
				selectedBarrel = clicked.gameObject;
				canvas.enabled = true;
				clicked.barrelFound = 0;
				float clickAngle = new float();
				clickAngle = angScript.angle;
				if (clickAngle != 9999f){
					Debug.Log("Copying angle from angleScripts");
					angle.Add (clickAngle);
					orderOfSelection.Add(clicked.gameObject.name);
				}
				else 
					Debug.Log("Error with angleScript.cs!");

				Debug.Log("Changing layer of selected barrel from barrel to environment");
				MoveToLayer(clicked.gameObject.transform,8);
				clicked.barrelFound = 0;
				meshRenderer = clicked.gameObject.GetComponentInChildren<MeshRenderer>();
				meshRenderer.material = redColorBarrelMaterial;
				break;
			}
		}
	}
}