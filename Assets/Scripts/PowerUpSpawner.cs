using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : Photon.PunBehaviour {
	public GameObject powerup = null;
	public string powerupresourse;
	public float spawnCooldown;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnPowerUpCoroutine");

		if (PhotonNetwork.isMasterClient) {
			powerup = PhotonNetwork.Instantiate (powerupresourse, transform.position, Quaternion.identity, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnPowerUpCoroutine() {
		yield return new WaitForSeconds (spawnCooldown);

		if (PhotonNetwork.isMasterClient) {
			if (powerup == null) {
				powerup = PhotonNetwork.Instantiate (powerupresourse, transform.position, Quaternion.identity, 0);
			}
		}

		StartCoroutine ("SpawnPowerUpCoroutine");
	}


}
