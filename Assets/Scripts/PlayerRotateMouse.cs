using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateMouse : Photon.PunBehaviour {
	public float offset = 130;

	void Update () {
		if (!photonView.isMine)
			return;
		/*
		float h = Input.mousePosition.x - Screen.width / 2;
		float v = Input.mousePosition.y - Screen.height / 2;
		float angle = -Mathf.Atan2(v,h) * Mathf.Rad2Deg;

		if (GetComponent<PlayerMovementMouse> ().isGrounded && !GetComponent<PlayerMovementMouse> ().lockCtrl) {
			transform.rotation = Quaternion.Euler (0, angle + offset, 0);
		}*/
	}
}
