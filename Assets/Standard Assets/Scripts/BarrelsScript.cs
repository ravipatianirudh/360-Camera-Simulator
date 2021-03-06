﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelsScript : MonoBehaviour {

	public GameObject barrelPrefab;
	public Canvas canvas;
	public Material redColorBarrelMaterial;


	private List<float> estimatedAngles = new List<float>();
	private List<float> actualAngles = new List<float> ();
	private List<string> orderOfSelection = new List<string>();
	private static string cameraName;
	private static int barrelsFound = 0;
	
	private GameObject[] barrels = new GameObject[12];

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

		cameraName = gameObject.name;
	
	}


	private void randomPlacement(float lowX,float lowZ,float highX,float highZ,int indexLow, int indexHigh){
		Collider[] colliders;
		Vector3 position;
		for (int i = indexLow; i < indexHigh; i++) {
			position = new Vector3 (Random.Range (lowX, highX), 0.0f, Random.Range (lowZ, highZ));
			colliders = Physics.OverlapSphere (position, 1.0F);
			while (colliders.Length != 0) {
				position = new Vector3 (Random.Range (lowX, highX), 7.0f, Random.Range (lowZ, highZ));
				colliders = Physics.OverlapSphere (position, 1.0F);
			}
			barrels [i] = Instantiate (barrelPrefab, position - new Vector3(0.0F,7.0F,0.0F), Quaternion.identity) as GameObject;
			barrels[i].name = i.ToString();
		}
	}

	void MoveToLayer(Transform root, int layer) {
		Debug.Log ("I'm in the layer function!");
		root.gameObject.layer = layer;
		foreach(Transform child in root)
			MoveToLayer(child, layer);
	}
	
	public GameObject[] returnBarrelsArray(){
		return barrels;	
	}

	public static int returnFoundBarrelsCount(){
		return barrelsFound;
	}
	public void returnLists(out List<float> estimation, out List<float> actual, out List<string> order){
		estimation = estimatedAngles;
		actual = actualAngles;
		order = orderOfSelection;
	}

	public static string returnCameraName(){
		return cameraName;
	}

	// Update is called once per frame
	void FixedUpdate () {
		float estimation = new float();
		float estimate;
		foreach (ClickBarrel clicked in clickedBarrel) {
			if(clicked.barrelFound == 1){
				canvas.enabled = true;
				if(angScript.angle != 9999f){
					estimation = angScript.angle;
					estimatedAngles.Add(estimation);
					orderOfSelection.Add(clicked.returnBarrel().gameObject.name);
					estimate = Vector3.Angle ( (new Vector3(0f,0f,1f) - new Vector3(0f,0f,0f)) , (clicked.returnBarrel().gameObject.transform.position - gameObject.transform.position) );
					if(clicked.returnBarrel().gameObject.transform.position.x < gameObject.transform.position.x )
						estimate = -estimate;
					actualAngles.Add( estimate );

					angScript.angle = 9999f;

					clicked.returnBarrel().layer = 8;
					clicked.returnBarrel().gameObject.GetComponentInChildren<Transform>().gameObject.layer = 8;
					clicked.returnBarrel().gameObject.GetComponentInChildren<MeshRenderer>().material = redColorBarrelMaterial;

					canvas.enabled = false;
					clicked.barrelFound = 0;
					barrelsFound++;
				}
				break;
			}
		}
		if (barrelsFound == 12 || Input.GetKeyDown(KeyCode.P)) {
			LogScript.setStatus(1);
//			Application.LoadLevel(2);
		}

	}
}