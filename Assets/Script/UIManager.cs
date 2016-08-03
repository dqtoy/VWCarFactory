using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using com.ootii.Messages;

public class UIManager : MonoBehaviour {

	bool isBreak;
	static public UIManager instance;
	public Button breakButton;
	public Sprite breakButtonImage;
	public Sprite buildButtonImage;
	public GameObject buttonBarContent;
	public string[] menuName;
	public GameObject animationScrollBar;
	public Vector2 scrollBounds;
	bool isBarOpen;
	public bool isPaintBarOut;
	public GameObject paintBarRoot;
	public Vector2 paintBarBound;
	public bool isBarDraging;

	public GameObject changeBGWindow;

	// Use this for initialization
	void Start () {
		MessageDispatcher.AddListener ("OnDragFinish",OnBarDragFinish,true);
		instance = this;
		InitialTextureButton ();
		PaintBarAnimation (false);
	}

	public void BackToTitle()
	{
		GetComponent<AudioSource> ().Play ();
		Application.LoadLevel (Application.loadedLevel - 1);
	}

	public void ChangeTexture()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (3,true);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeOutParts()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (0,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeElecDevice()
	{
		GameManager.instance.CameraChange (true);
		ChangeButtonList (2,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeSpecialCar()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (1,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeOther()
	{
		GameManager.instance.CameraChange (false);
		ChangeButtonList (4,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
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
			
		for (int j = 0; j < AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id]).Count; j++) {
			GameObject obj = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			obj.name = "Button " + j;
			GameManager.instance.allButtonIcon.Add (obj);
			CustomButton btn = obj.GetComponent<CustomButton> ();
			Image btnImg = obj.GetComponent<Image> ();
			obj.transform.SetParent (buttonBarContent.transform, false);
			Texture2D img;
			img = Resources.Load (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Icon) as Texture2D;
			btn.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
			btn.ChangeText (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Name);
			btn.SetID (j);
		}
	}

	public void ChangeScrollBar(bool bo)
	{
		if (bo) {
			animationScrollBar.transform.DOLocalMoveY (scrollBounds.y, 0.5f).SetEase (Ease.InOutExpo);
			isBarOpen = true;
		} else {
			animationScrollBar.transform.DOLocalMoveY (scrollBounds.x, 0.5f).SetEase (Ease.InOutExpo);
			isBarOpen = false;
		}
	}

	public void OnBarDragFinish(IMessage rMessage)
	{
		//Debug.Log((scrollBounds.y - scrollBounds.x)/2);
		if (animationScrollBar.transform.localPosition.y > scrollBounds.y - (scrollBounds.y - scrollBounds.x)/2) {
			ChangeScrollBar (true);
		} else {
			ChangeScrollBar (false);
		}
		isBarDraging = false;
	}

	public void PaintBarAnimation(bool bo)
	{
		if (bo) {
			isPaintBarOut = true;
			paintBarRoot.transform.DOLocalMoveX (paintBarBound.y, 0.5f).SetEase (Ease.InOutExpo);
		} else {
			isPaintBarOut = false;
			paintBarRoot.transform.DOLocalMoveX (paintBarBound.x, 0.5f).SetEase (Ease.InOutExpo);
		}
	}

	public void BGWinodowShow(bool bo)
	{
		changeBGWindow.SetActive (bo);
	}

	public void ChangeBG(int id)
	{
		GameManager.instance.ChangeBGFunc (id);
	}

	public void SaveImage()
	{
		GameManager.instance.SaveImage ();
	}
}
