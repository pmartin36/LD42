using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.Instance.Menu = false;
		LevelIn();
	}
	
	public void LevelOut() {
		GetComponent<Animator>().Play("Blackout_LevelOut");
	}

	public void LevelIn() {
		GetComponent<Animator>().Play("Blackout_LevelIn");
	}

	public void ReloadLevel() {
		GameManager.Instance.ReloadLevel();
	}
}
