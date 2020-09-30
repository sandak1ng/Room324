using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour {

	public static int keyCount;

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "Player") {
			keyCount += 1;
			Destroy (gameObject);
		}
	}

}
