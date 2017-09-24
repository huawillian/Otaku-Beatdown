using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : Photon.PunBehaviour {
	public GameObject healthPrefab;
	public GameObject health = null;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnHealthCoroutine");

		if (PhotonNetwork.isMasterClient) {
			health = PhotonNetwork.Instantiate ("Health", transform.position, Quaternion.identity, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SpawnHealthCoroutine() {
		yield return new WaitForSeconds (Random.Range(40.0f, 60.0f));

		if (PhotonNetwork.isMasterClient) {
			if (health == null) {
				health = PhotonNetwork.Instantiate ("Health", transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnHealthCoroutine");
	}


}
