using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

	public TMP_Text amtStacked;
	public TMP_Text score;
	public TMP_Text directions;
	public TMP_Text lossReason;

	public Image background;


	// Use this for initialization
	void Start () {
		GameManager.Instance.GameOverScreen = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetScreen(string lossstring) {
		int boxesPlaced = GameManager.Instance.BoxesPlaced;
		amtStacked.text = $"Stacked\n{boxesPlaced} boxes";

		if( boxesPlaced < 25 ) {
			score.text = "Subpar Stacker";
		}
		else if(boxesPlaced < 75) {
			score.text = "Standard Stacker";
		}
		else if(boxesPlaced < 125) {
			score.text = "Super Stacker";
		}
		else {
			score.text = "Spectacular Stacker";
		}

		lossReason.text = lossstring;

		GetComponent<Animator>().Play("GameOver");
	}
}
