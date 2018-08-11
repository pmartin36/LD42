using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class GameManager : Singleton<GameManager> {

	public bool Paused { get; set; }
	public Player Player;

	public void Awake() {

	}

	public void ProcessInputs(InputPackage p) {
		if(Paused) {
			// do something
		}
		else {
			Player.ProcessInputs(p);
		}
	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ToggleSoundOn() {
		
	}


}