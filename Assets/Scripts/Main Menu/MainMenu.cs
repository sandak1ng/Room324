using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button playText;
	public Button exitText;
	public Button startText;
	public Button exitStoryText;
	public GameObject storyboard;

	void Start (){
		startText = startText.GetComponent<Button> ();
		playText = playText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		exitStoryText = exitStoryText.GetComponent<Button> ();
	}

	public void play(){
		SceneManager.LoadScene ("Room324");
	}

	public void start(){
		storyboard.SetActive (true);
	}

	public void exitStory(){
		storyboard.SetActive (false);
	}

	public void muteAudio(){
		AudioListener.pause = !AudioListener.pause;
	}

	public void exit(){
		Application.Quit ();
	}
}
