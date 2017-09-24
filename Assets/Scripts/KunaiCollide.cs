using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiCollide : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		GameObject.Destroy (this.gameObject);
	}
}
