using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSpawner : Photon.PunBehaviour {
	public GameObject shurikenPrefab;
	public GameObject shuriken = null;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnShurikenCoroutine");

		if (PhotonNetwork.isMasterClient) {
			shuriken = PhotonNetwork.Instantiate ("ShurikePowerUp", transform.position, Quaternion.identity, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnShurikenCoroutine() {
		yield return new WaitForSeconds (Random.Range(25.0f, 30.0f));

		if (PhotonNetwork.isMasterClient) {
			if (shuriken == null) {
				shuriken = PhotonNetwork.Instantiate ("ShurikePowerUp", transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnShurikenCoroutine");
	}


}
