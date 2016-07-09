using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	bool isBreak;
	static public UIManager instance;
	public Button breakButton;
	public Sprite breakButtonImage;
	public Sprite buildButtonImage;
	public GameObject buttonBarContent;

	// Use this for initialization
	void Start () {
		instance = this;
		InitialTextureButton ();
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

	public void ChangeTexture()
	{
		GameManager.instance.CameraChange (false);
	}

	public void ChangeOutParts()
	{
		GameManager.instance.CameraChange (false);
	}

	public void ChangeElecDevice()
	{
		GameManager.instance.CameraChange (true);
	}

	public void ChangeInParts()
	{
		GameManager.instance.CameraChange (true);
	}

	public void ChangeOther()
	{
		GameManager.instance.CameraChange (false);
	}

	public void InitialTextureButton()
	{
		for (int i = 0; i < GameManager.instance.cunstomTextures.Count; i++) {
			GameObject obj = Instantiate (Resources.Load("UI/PartButton") as GameObject,Vector3.zero,Quaternion.identity) as GameObject;
			CustomButton btn = obj.GetComponent<CustomButton> ();
			Image btnImg = obj.GetComponent<Image> ();
			obj.transform.SetParent (buttonBarContent.transform, false);
			Texture2D img = Resources.Load (GameManager.instance.cunstomTextures[i]) as Texture2D;
			btn.ChangeImage(Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
			btn.ChangeText ("test");
		}
	}
}
