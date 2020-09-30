using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public GameObject gameWin;
	public GameObject gameLoss;
	public GameObject gamePause;
	public GameObject gameSettings;

	public bool isGamePaused = false;
	bool gameOver;


	// Use this for initialization
	void Start () {
		Enemy.EnemyHasSpottedPlayer += showGameLoss;
		FindObjectOfType<Controller> ().reachEnd += showGameWin;
		// exitPause = exitPause.GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			Time.timeScale = 0;
			if (Input.GetKeyDown (KeyCode.Space)) {
				KeyDoor.keyCount = 0;
				Time.timeScale = 1;
				SceneManager.LoadScene (1);
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (isGamePaused) {
				resume ();
			} else {
				pause ();
			}
		}
	}

	public void pause() {
			gamePause.SetActive (true);
			gameSettings.SetActive (false);
			Time.timeScale = 0;
			isGamePaused = true;
		AudioListener.volume = 0f;
	}

	public void resume (){
		gamePause.SetActive (false);
		Time.timeScale = 1;
		isGamePaused = false;
		AudioListener.volume = 1f;
	}

	public void muteAudio(){
		AudioListener.pause = !AudioListener.pause;
	}

	public void quitToMenu (){
		gamePause.SetActive (false);
		Time.timeScale = 1;
		isGamePaused = false;
		AudioListener.volume = 1f;
		SceneManager.LoadScene (0);
	}

	public void settings (){
		gameSettings.SetActive (true);
	}
		
	void showGameWin(){
		whenGameOver (gameWin);
	}

	void showGameLoss(){
		whenGameOver (gameLoss);
	}
		

	void whenGameOver(GameObject gameOverUI){
		gameOverUI.SetActive (true);
		gameOver = true;
		Enemy.EnemyHasSpottedPlayer -= showGameLoss;
		FindObjectOfType<Controller> ().reachEnd -= showGameWin;
	}
}
