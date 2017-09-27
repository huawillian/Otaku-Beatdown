using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpawner : Photon.PunBehaviour {
	public GameObject ultimatePrefab;
	public GameObject ultimate = null;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnUltimateCoroutine");

		if (PhotonNetwork.isMasterClient) {
			ultimate = PhotonNetwork.Instantiate ("UltimatePowerUp", transform.position, Quaternion.identity, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnUltimateCoroutine() {
		yield return new WaitForSeconds (Random.Range(50.0f, 60.0f));

		if (PhotonNetwork.isMasterClient) {
			if (ultimate == null) {
				ultimate = PhotonNetwork.Instantiate ("UltimatePowerUp", transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnUltimateCoroutine");
	}


}
