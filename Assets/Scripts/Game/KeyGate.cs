using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "Player" && KeyDoor.keyCount > 0) {
			KeyDoor.keyCount--;
			Destroy (gameObject);
		}
	}
}
