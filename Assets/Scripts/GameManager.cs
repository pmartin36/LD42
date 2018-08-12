using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class GameManager : Singleton<GameManager> {

	public bool Menu { get; set; }
	public Player Player;
	public Scale Scale;
	public Conveyor Conveyor;
	public SpeedDisplay SpeedDisplay;

	public int BoxesPlaced;
	public Dictionary<int, BoxStack> BoxStacks;

	public GameOver GameOverScreen;
	public BoxPlacedCounter BoxPlacedCounter;


	public void Awake() {
		BoxStacks = new Dictionary<int, BoxStack>();
	}

	public void Start () {

	}

	public void ProcessInputs(InputPackage p) {
		if(p.Quit) {
			Application.Quit();
		}

		if(Menu) {
			// do something
			if(p.Enter) {
				FindObjectOfType<Blackout>().LevelOut();
			}
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

	public void BoxDisplaced(int diff) {
		BoxesPlaced = Mathf.Max(0, BoxesPlaced + diff);
		Scale.ReevaluateWeight();
		BoxPlacedCounter.UpdateCount(BoxesPlaced);
	}

	public void PlayerLost() {
		var boxes = FindObjectsOfType<Box>();
		foreach(Box b in boxes) {
			b.Interactable = false;
			b.StopSpawn = true;
		}
		Player.CanMove = false;
		Conveyor.CanSpawn = false;

		BoxPlacedCounter.gameObject.SetActive(false);
		Menu = true;

		GameOverScreen.SetScreen();
	}
}