using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiSpawner : Photon.PunBehaviour {
	public GameObject kunaiPrefab;
	public GameObject kunai = null;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnKunaiCoroutine");

		if (PhotonNetwork.isMasterClient) {
			kunai = PhotonNetwork.Instantiate ("KunaPowerUp", transform.position, Quaternion.identity, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnKunaiCoroutine() {
		yield return new WaitForSeconds (Random.Range(20.0f, 40.0f));

		if (PhotonNetwork.isMasterClient) {
			if (kunai == null) {
				kunai = PhotonNetwork.Instantiate ("KunaPowerUp", transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnKunaiCoroutine");
	}


}
