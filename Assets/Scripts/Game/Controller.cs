using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	Vector3 movement;

	public event System.Action reachEnd;
	public float movementSpeed = 10f;
	public float rotationSpeed = .3f;
	private Animator animation;

	void Start() {
		animation = GetComponent<Animator> ();
	}

	void Update() {
		float horizontalMovement = Input.GetAxisRaw("Horizontal");
		float verticalMovement = Input.GetAxisRaw("Vertical");
		movement = new Vector3 (horizontalMovement, 0, verticalMovement);
		if (horizontalMovement != 0f || verticalMovement != 0f) {
			animation.SetBool ("Walking", true);
		} else
			animation.SetBool ("Walking", false);
	}


	public void FixedUpdate() {
		Move(movement);
		Turn(movement, rotationSpeed);
	}


	public void Move(Vector3 movement) {
		transform.Translate(movement.normalized * movementSpeed * Time.deltaTime, Space.World);
	}


	public void Turn(Vector3 movement, float rotationSpeed) {
		if (movement.x != 0 || movement.z != 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
		}
	}

	void OnTriggerEnter(Collider hitCollider){
		if (hitCollider.tag == "Finish") {
			if (reachEnd != null) {
				reachEnd ();
			}
		}
	}
}