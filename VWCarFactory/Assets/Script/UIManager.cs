using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	bool isBreak;
	public Button breakButton;
	public Sprite breakButtonImage;
	public Sprite buildButtonImage;

	// Use this for initialization
	void Start () {
		
	}

	public void BreakButton()
	{
		CarControl.instance.ChangeBodyPartsButton ();
		if (breakButton.image.sprite == breakButtonImage) {
			breakButton.image.sprite = buildButtonImage;
		} else {
			breakButton.image.sprite = breakButtonImage;
		}
		GetComponent<AudioSource> ().Play ();
	}

	public void BackToTitle()
	{
		GetComponent<AudioSource> ().Play ();
		Application.LoadLevel (Application.loadedLevel - 1);
	}
}
