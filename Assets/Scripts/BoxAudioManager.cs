using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxAudioManager : MonoBehaviour {

	AudioSource boxPlaced;
	AudioSource broke;

	// Use this for initialization
	void Start () {
		var audios = GetComponents<AudioSource>();
		boxPlaced = audios.First(a => a.clip.name == "box_placed");
		broke = audios.First(a => a.clip.name == "fragile_better");
	}
	
	public void PlayBroke() {
		broke.Play();
	}

	public void PlayBoxPlaced() {
		boxPlaced.Play();
	}

}
