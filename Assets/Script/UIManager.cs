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
	public Image[] typeButtons;
	public GameObject[] descriptionButtons;

	public GameObject changeBGWindow;
	public GameObject samplePhotoWindow;
	public GameObject largeSamplePhoto;
	public GameObject samplePhotoContent;
	public GameObject sampleVideoWindow;
	public GameObject shareButtonWindow;
	public MediaPlayerCtrl videoContent;
	List<GameObject> samplePhotoList;

	public IButtonInfo nowSelectedButton;

	// Use this for initialization
	void Start () {
		samplePhotoList = new List<GameObject> ();
		MessageDispatcher.AddListener ("OnDragFinish",OnBarDragFinish,true);
		instance = this;
		InitialTextureButton ();
		InitialColorButton ();
		TypeButtonChange (0);
		//ChangeDescriptionButtons (false, false, false);
		PaintBarAnimation (false);
	}

	public void BackToTitle()
	{
		GetComponent<AudioSource> ().Play ();
		Application.LoadLevel (Application.loadedLevel - 1);
	}

	public void ChangeTexture()
	{
		//GameManager.instance.CameraChange (false);
		ChangeButtonList (3,AppData.GetPaintingTemplateByName(CarStudio.Car.CarBaseModle));
		TypeButtonChange (3);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeOutParts()
	{
		//GameManager.instance.CameraChange (false);
		ChangeButtonList (0,AppData.GetCarAllParts(CarStudio.Car.CarBaseModle));
		TypeButtonChange (0);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeElecDevice()
	{
		//GameManager.instance.CameraChange (true);
		ChangeButtonCNG (2);
		TypeButtonChange (2);
		//ChangeButtonList (2,AppData.GetCarPartsByName(Scene1_UI.CarSeleted,"CNG"));
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeSpecialCar()
	{
		//GameManager.instance.CameraChange (false);
		ChangeButtonList (1,AppData.GetSpecialTemplateCarList(CarStudio.Car.CarBaseModle));
		TypeButtonChange (1);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void ChangeOther()
	{
		//GameManager.instance.CameraChange (false);
		//ChangeButtonList (4,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void InitialTextureButton()
	{
		ChangeButtonList (0,AppData.GetCarAllParts(CarStudio.Car.CarBaseModle));
	}

	public void InitialColorButton()
	{
		var button = AppData.GetCarColorsByName (CarStudio.Car.CarBaseModle);

		for (int i = 0; i < button.Count; i++) {
			GameObject objSub = Instantiate (Resources.Load ("UI/PaintButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			CustomButton btnSub = objSub.GetComponent<CustomButton> ();
			Image btnSubImg = objSub.GetComponent<Image> ();
			objSub.transform.SetParent (paintBarRoot.transform, false);
			Texture2D img;
			img = Resources.Load (button[i].Icon) as Texture2D;
			btnSub.SetThisButton (button[i]);
			btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
		}
	}

	public void ChangeButtonCNG(int id)
	{
		GameManager.instance.nowSelectedCustomType = id;
		for (int m = 0; m < GameManager.instance.allButtonIcon.Count; m++) {
			//GameManager.instance.allButtonIcon.Remove(GameManager.instance.allButtonIcon[m]);
			Destroy (GameManager.instance.allButtonIcon [m]);
		}
		if (GameManager.instance.allButtonIcon.Count > 0) {
			GameManager.instance.allButtonIcon.RemoveRange (0, GameManager.instance.allButtonIcon.Count);
		}

		IButtonInfo button = AppData.GetCngCar(Scene1_UI.CarSeleted);
		GameObject obj = Instantiate (Resources.Load ("UI/PartTag") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		GameManager.instance.allButtonIcon.Add (obj);
		CustomButton btn = obj.GetComponent<CustomButton> ();
		obj.transform.SetParent (buttonBarContent.transform, false);
		btn.ChangeText (button.Tag);

		GameObject objSub = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		GameManager.instance.allButtonIcon.Add (objSub);
		CustomButton btnSub = objSub.GetComponent<CustomButton> ();
		Image btnSubImg = objSub.GetComponent<Image> ();
		objSub.transform.SetParent (buttonBarContent.transform, false);
		Texture2D img;
		if (CarStudio.Exists (button.Name)) {
			img = Resources.Load (button.Icon + "b") as Texture2D;
		} else {
			img = Resources.Load (button.Icon) as Texture2D;
		}
		btnSub.SetThisButton (button);
		btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
		btnSub.ChangeText (button.Name);
		btnSub.ChangeCondition (id);
	}

	public void ChangeButtonList(int id,Dictionary<string,List<IButtonInfo>> parts)
	{
		GameManager.instance.nowSelectedCustomType = id;
		for (int m = 0; m < GameManager.instance.allButtonIcon.Count; m++) {
			//GameManager.instance.allButtonIcon.Remove(GameManager.instance.allButtonIcon[m]);
			Destroy (GameManager.instance.allButtonIcon [m]);
		}
		if (GameManager.instance.allButtonIcon.Count > 0) {
			GameManager.instance.allButtonIcon.RemoveRange (0, GameManager.instance.allButtonIcon.Count);
		}

//		for (int j = 0; j < AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id]).Count; j++) {
//			GameObject obj = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
//			obj.name = "Button " + j;
//			GameManager.instance.allButtonIcon.Add (obj);
//			CustomButton btn = obj.GetComponent<CustomButton> ();
//			Image btnImg = obj.GetComponent<Image> ();
//			obj.transform.SetParent (buttonBarContent.transform, false);
//			Texture2D img;
//			img = Resources.Load (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Icon) as Texture2D;
//			btn.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
//			btn.ChangeText (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Name);
//			btn.SetID (j);
//		}
		//var parts = AppData.GetCarAllParts(CarStudio.Car.CarBaseModle);
		foreach (var part in parts) {
			
			GameObject obj = Instantiate (Resources.Load ("UI/PartTag") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			GameManager.instance.allButtonIcon.Add (obj);
			CustomButton btn = obj.GetComponent<CustomButton> ();
			obj.transform.SetParent (buttonBarContent.transform, false);
			btn.ChangeText (part.Key);

			foreach (var button in parts[part.Key]) {
				GameObject objSub = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
				GameManager.instance.allButtonIcon.Add (objSub);
				CustomButton btnSub = objSub.GetComponent<CustomButton> ();
				Image btnSubImg = objSub.GetComponent<Image> ();
				objSub.transform.SetParent (buttonBarContent.transform, false);
				Texture2D img;
				if (CarStudio.Exists (button.Name)) {
					img = Resources.Load (button.Icon + "b") as Texture2D;
				} else {
					img = Resources.Load (button.Icon) as Texture2D;
				}
				btnSub.SetThisButton (button);
				btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
				btnSub.ChangeText (button.Name);
				btnSub.ChangeCondition (id);
				//btn.SetID (j);
			}
		}
		ResetContentSize (buttonBarContent.GetComponent<RectTransform>(),GameManager.instance.allButtonIcon.Count,110.0f);
	}

	public void ResetContentSize(RectTransform content,int count,float eleSize)
	{
		for (int i = 0; i < count; i++) {
			content.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal,i * eleSize);
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
			ChangeDescriptionButtons (false, false, false);
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

	public void SamplePhotoWindowShow(bool bo)
	{
		samplePhotoWindow.SetActive (bo);
		LargeSamplePhotoShow (false);
	}

	public void SampleVideoWindowShow(bool bo)
	{
		sampleVideoWindow.SetActive (bo);

	}

	public void LargeSamplePhotoShow(bool bo)
	{
		largeSamplePhoto.SetActive (bo);
	}

	public void ShareWindowShow(bool bo)
	{
		shareButtonWindow.SetActive (bo);
	}

	public void SamplePhotoRefresh()
	{
		Debug.Log (samplePhotoList.Count);
		for (int m = 0; m < samplePhotoList.Count; m++) {
			Destroy (samplePhotoList[m]);
		}
		if (samplePhotoList.Count > 0) {
			samplePhotoList.RemoveRange (0, samplePhotoList.Count);
		}
		foreach (var item in nowSelectedButton.TextureDescription) {
			GameObject obj = Instantiate (Resources.Load ("UI/CarSmaplePhotoButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			samplePhotoList.Add (obj);
			SamplePhotoButton btn = obj.GetComponent<SamplePhotoButton> ();
			obj.transform.SetParent (samplePhotoContent.transform, false);
			Texture2D img;
			Debug.Log (AppData.GetSamples (item).Asset);
			img = Resources.Load (AppData.GetSamples(item).Asset) as Texture2D;
			btn.SetSamplePhoto (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)), AppData.GetSamples(item).Name, AppData.GetSamples(item).Description);
		}
	}

	public void ChangeDescriptionButtons(bool bo0,bool bo1,bool bo2)
	{
		descriptionButtons [0].SetActive(bo0);
		descriptionButtons [1].SetActive(bo1);
		//descriptionButtons [2].SetActive(bo2);
	}

	public void TypeButtonChange(int id)
	{
		foreach (Image item in typeButtons) {
			item.color = new Color(item.color.r,item.color.g,item.color.b,0);
		}
		typeButtons[id].color =  new Color(typeButtons[id].color.r,typeButtons[id].color.g,typeButtons[id].color.b,1);
	}

	public void SaveImage()
	{
		GameManager.instance.SaveImage ();
	}


}
