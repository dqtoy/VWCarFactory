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
	public string[] menuName;

	// Use this for initialization
	void Start () {
		instance = this;
		InitialTextureButton ();
	}

	public void BackToTitle()
	{
		GetComponent<AudioSource> ().Play ();
		Application.LoadLevel (Application.loadedLevel - 1);
	}

	public void ChangeTexture()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (0,true);
	}

	public void ChangeOutParts()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (1,false);
	}

	public void ChangeElecDevice()
	{
		GameManager.instance.CameraChange (true);
		ChangeButtonList (2,false);
	}

	public void ChangeInParts()
	{
		GameManager.instance.CameraChange (true);
		ChangeButtonList (3,false);
	}

	public void ChangeOther()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (4,false);
	}

	public void InitialTextureButton()
	{
		ChangeButtonList (0,true);
	}

	public void ChangeButtonList(int id,bool isTexture)
	{
		GameManager.instance.nowSelectedCustomType = id;
		for (int m = 0; m < GameManager.instance.allButtonIcon.Count; m++) {
			//GameManager.instance.allButtonIcon.Remove(GameManager.instance.allButtonIcon[m]);
			Destroy (GameManager.instance.allButtonIcon [m]);
		}
		if (GameManager.instance.allButtonIcon.Count > 0) {
			GameManager.instance.allButtonIcon.RemoveRange (0, GameManager.instance.allButtonIcon.Count);
		}

		for (int i = 0; i < GameManager.instance.customTexturesBtn.Count; i++) {
			GameObject obj = Instantiate (Resources.Load("UI/PartButton") as GameObject,Vector3.zero,Quaternion.identity) as GameObject;
			obj.name = "Button " + i;
			GameManager.instance.allButtonIcon.Add (obj);
			CustomButton btn = obj.GetComponent<CustomButton> ();
			Image btnImg = obj.GetComponent<Image> ();
			obj.transform.SetParent (buttonBarContent.transform, false);
			Texture2D img;
			img = Resources.Load (GameManager.instance.customTexturesBtn[i]) as Texture2D;
			btn.ChangeImage(Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
			btn.ChangeText ("test");
			btn.SetID (i);
		}
	}
}
