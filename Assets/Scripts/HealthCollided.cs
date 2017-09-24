using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollided : Photon.PunBehaviour {

	void OnTriggerEnter(Collider other) {
		if (!PhotonNetwork.isMasterClient)
			return;

		if (other.name.Contains ("Player")) {
			PhotonNetwork.Destroy (this.gameObject);
		}
	}
}
