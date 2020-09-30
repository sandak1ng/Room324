using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	//Event that triggers when enemy spots the player
	//System.Action represents a delegate with no parameters and a void return type
	public static event System.Action EnemyHasSpottedPlayer;
	//Speed of the enemy
	public float speed = 0;
	//Wait time of the enemy before moving to the next waypoint
	public float delay = .0f;
	//Time it takes to rotate
	public float turnSpeed = 0;
	//How long it takes to catch the player (kill)
	public float timeToKillPlayer = .4f;
	//References the spotlight
	public Light spotLight;
	//References the spotlight angle
	private float viewAngle;
	//The enemies view distance
	public float viewDistance;
	//Timer for how long is the player visible to the enemy
	private float playerVisible;
	//Obstacle layer mask used to detect whether or not the player is within line of sight
	public LayerMask viewMask;
	//Takes the original spotlight colour set in the scene
	Color ogSpotlightColour;

	public Transform wayPoint;
	Transform player;


	// Use this for initialization
	void Start () {
		//Finds any object with the "Player" tag applied
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//Makes it so that the view angle is taking the angle from the spotlight
		viewAngle = spotLight.spotAngle;
		ogSpotlightColour = spotLight.color;

		//Array of all of the positions of the waypoints in the path
		//The size of the new vector 3 is the number of children that the pathVisual holds
		Vector3[] waypoints = new Vector3[wayPoint.childCount];

		//Then I am looping through each index of my waypoints array 
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = wayPoint.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}
		//Starts the FollowPath Coroutine
		StartCoroutine (FollowPath (waypoints));
	}

	//Boolean is used to see if the player is within the view distance of the enemy
	bool abletoSee (){
		//If the distance between the enemies position and the players position is less than the view distance
		if (Vector3.Distance (transform.position, player.position) < viewDistance) {
			//I am checking if the angle between the enemies forward direction and the direction to the player is within the viewAngle
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			float angleBetweenEnemyAndPlayer = Vector3.Angle (transform.forward, directionToPlayer);
			if (angleBetweenEnemyAndPlayer < viewAngle) {
				//All that needs to be checked now is whether or not the line of sight to the player is blocked by an obstacle
				//I am casting a ray from the guards position to the players position with a layer mask (viewMask) for the obstacles
				if (!Physics.Linecast (transform.position, player.position, viewMask)) {
					//After all 3 checks, we are returning true as in the enemy CAN see the player.
					return true;
				}
			}
		}
		// If any one of the three checks fail then we return false as in the enemy CAN'T see the player.
		return false;
	}

	//A coroutine is a function that has the ability to pause execution and return control to Unity but then to continue where it left off on the following frame.
	//to declare a coroutine in C#, you use IEnumerator.

	IEnumerator FollowPath (Vector3[] waypoints) {
		//Guard starts at the position of the first waypoint
		transform.position = waypoints[0];
		//I am using this integer to keep track of the index of the waypoint that we are currently moving towards
		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints[targetWaypointIndex];

		while (true) {
			Quaternion targetRotation = Quaternion.LookRotation(targetWaypoint - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
			if (transform.rotation == targetRotation) { 
				//Updates the enemys current position to the target waypoints position at the speed of which I can declare in the inspector
				transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
				//If the enemy reaches the target waypoint, it will move onto the next waypoint.
				if (transform.position == targetWaypoint) {
					//the modulus is used because when (targetWaypointIndex + 1) = waypoints.Length it will go back to 0.
					targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
					targetWaypoint = waypoints[targetWaypointIndex];
					//the enemy will wait for an x amount of time where x I can declare the amount in the inspector.
					yield return new WaitForSeconds(delay);
				}
			} 
			//yield for 1 frame for each iteration of the while loop
			yield return null;
		}﻿
	}
	
	// Update is called once per frame
	void Update () {
		if (abletoSee ()) {
			//If the player is visible then the timer is updated 
			playerVisible += Time.deltaTime;
		}
		else {
			//If the player is not visible then that timer will count down
			playerVisible -= Time.deltaTime;
		}

		//I am clamping the value between 0 and the time to kill the player
		playerVisible = Mathf.Clamp (playerVisible, 0, timeToKillPlayer);
		//Spotlight colour changes gradually when player is inside the spotlight
		//ogSpotlightColour is the colour of the spotlight originally (which is green in the scene), then the colour changes gradually to red.
		//When the playerVisible is 0 as in the player is not within the viewDistance then the spotlight colour will be the original.
		//When it is equal to the timeToKillPlayer then it goes red.
		spotLight.color = Color.Lerp (ogSpotlightColour, Color.red, playerVisible / timeToKillPlayer);

		//playerVisible will never be greater than timeToKillPlayer however it is logically correct to declare it like this anyway.
		//But if playerVisible is equal to timeToKillPlayer then I will need to trigger an event (Defeat screen).
		if (playerVisible >= timeToKillPlayer) {
			//As long as the event is not null
			if (EnemyHasSpottedPlayer != null) {
				//I can invoke the event with just a pair of parantheses
				EnemyHasSpottedPlayer ();
			}
		}
	}
}
