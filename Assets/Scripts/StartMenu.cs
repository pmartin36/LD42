using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

	[SerializeField]
	private Image Blackout;
	public Color BlackOutColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Blackout.color = BlackOutColor;

		if(Input.GetButtonDown("Enter")) {
			GetComponent<Animator>().Play("StartMenu");
		}	
		else if(Input.GetButtonDown("Exit")) {
			Application.Quit();
		}
	}

	public void LoadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}
}
