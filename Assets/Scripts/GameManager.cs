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
	public Scale Scale;
	public Conveyor Conveyor;

	public Dictionary<int, BoxStack> BoxStacks;

	public void Awake() {
		BoxStacks = new Dictionary<int, BoxStack>();
	}

	public void Start () {
		//create initial box stacks
		//var boxes = FindObjectsOfType<Box>();
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

	public void PlayerLost() {
		var boxes = FindObjectsOfType<Box>();
		foreach(Box b in boxes) {
			b.Interactable = false;
			b.StopSpawn = true;
		}
		Player.CanMove = false;
		Conveyor.CanSpawn = false;
	}
}