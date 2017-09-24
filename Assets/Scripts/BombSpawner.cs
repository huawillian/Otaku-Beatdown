using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : Photon.PunBehaviour {
	public GameObject bombPrefab;
	public GameObject bomb = null;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnBombCoroutine");

		if (PhotonNetwork.isMasterClient) {
			bomb = PhotonNetwork.Instantiate ("BombPowerUp", transform.position, Quaternion.identity, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnBombCoroutine() {
		yield return new WaitForSeconds (Random.Range(30.0f, 40.0f));

		if (PhotonNetwork.isMasterClient) {
			if (bomb == null) {
				bomb = PhotonNetwork.Instantiate ("BombPowerUp", transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnBombCoroutine");
	}


}
