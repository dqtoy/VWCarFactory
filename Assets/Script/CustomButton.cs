using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum CustomType
{
	OutsidePart,
	TextureColor,
	SpecialCar,
	CNG,
	PaintingCar
}

public class CustomButton : MonoBehaviour {

	public CustomType customType;
	public int thisID;
	public Text thisText;
	public Image thisImage;
	public bool isTag;
	public bool isPreview;
	public bool isVideo;
	public bool isPaint;
	public IButtonInfo thisButton;
	IButtonInfo descriptionButton;

	// Use this for initialization
	void Start () {
		//descriptionButton = AppData.GetCarPartsByName (GameManager.instance.carType.ToString());
	}

	public void SetID(int id)
	{
		thisID = id;
	}

	public void ChangeImage(Sprite img)
	{
		thisImage.sprite = img;
	}

	public void ChangeText(string txt)
	{
		thisText.text = txt;
	}

	public void ChangeCondition(int cond)
	{
		switch (cond) {
		case 0:
			customType = CustomType.OutsidePart;
			break;
		case 1:
			customType = CustomType.SpecialCar;
			break;
		case 2:
			customType = CustomType.CNG;
			break;
		case 3:
			customType = CustomType.PaintingCar;
			break;
		default:
			break;
		}

		if (thisButton.Name == "变色模块") {
			customType = CustomType.TextureColor;
		}
	}

	public void SetThisButton(IButtonInfo button)
	{
		thisButton = button;
	}

	public void ClickThisButton()
	{
		if (!isTag) {
			GameManager.instance.isPartButtonClick = true;
			descriptionButton = thisButton;
			Texture2D img;
			if (customType == CustomType.TextureColor && !UIManager.instance.isPaintBarOut) {
				UIManager.instance.PaintBarAnimation (true);
				img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;
				thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
				GameManager.instance.CameraGoBack ();
				//GameManager.instance.ChangeCustomTexture (thisID);
			}
			else{
				if (UIManager.instance.isPaintBarOut && !isPaint) {
					UIManager.instance.PaintBarAnimation (false);
				}

				if (!isPaint) {
					UIManager.instance.ChangeDescriptionButtons(descriptionButton.TextureDescription!=null,descriptionButton.MovieDescription!=null,string.IsNullOrEmpty(descriptionButton.PdfDescription));
					UIManager.instance.nowSelectedButton = thisButton;
					if (CarStudio.Exists (descriptionButton.Name)) {
						img = Resources.Load (descriptionButton.Icon) as Texture2D;
					} else {
						img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;

					}
					//CameraGoBack ();
					thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
				} 


				if (customType == CustomType.OutsidePart) {
					

					if (thisButton.Name != "电动踏板") {
						GameManager.instance.CloseDoor ();
					}
					if (descriptionButton.Type == null){// || GameManager.instance.inCameraPosition) {
						ChangePart ();
					} else {
						CameraGoto (int.Parse(descriptionButton.Type));
					}
				}
				else if (customType == CustomType.SpecialCar) {
					CarStudio.LoadTemplate (thisButton.Description);
					GameManager.instance.CameraGoBack ();
					return;
				}
				else if (customType == CustomType.CNG) {
//					CarData carData = AppData.GetCarAllParts().;
//					CarStudio.LoadTemplate (carData.CNG);
					CameraGoto (2);
					return;
				}
				else if (customType == CustomType.PaintingCar) {
					CarStudio.LoadTemplate (thisButton.Description);
					GameManager.instance.CameraGoBack ();
					return;
				}
			}

		}
	}

	void ChangePart()
	{
		if (customType == CustomType.CNG) {
			CarStudio.LoadTemplate(AppData.GetCarDataByName(Scene1_UI.CarSeleted).CNG);
		} else {
			
			if (CarStudio.Exists(thisButton.Name)) {
				StartSettingAnimation ("_playback");
				if (thisButton.Name != "电动踏板" && thisButton.Name != "后盖开启") {
					UIManager.instance.HideFloatWindow ();
					UIManager.instance.float3DButton.gameObject.SetActive (false);
					CarStudio.RemovePart (thisButton.Name);
				}

				if (thisButton.Name == "导航仪") {
					GameManager.instance.ShowIphone (false, GameManager.instance.selectedCarID);
				} else {
					GameManager.instance.iphoneHideComplete ();
				}
//				else if (thisButton.Name == "电动踏板" && !GameManager.instance.epDown) {
//					CarStudio.RemovePart (thisButton.Name);
//				}
			} else {
				CarStudio.AddPart (thisButton.Name);
				StartSettingAnimation ("_play");
				ShowFloatButton ();
				if (thisButton.Name == "导航仪") {
					GameManager.instance.ShowIphone (true, GameManager.instance.selectedCarID);
				}
				else
				{
					GameManager.instance.iphoneHideComplete ();
				}
			}
		}
		CarStudio.SaveCustumUserCar(Scene1_UI.CarSeleted + "save");
	}

	void StartSettingAnimation(string str)
	{
		GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
		foreach (GameObject obj in parts) {
			obj.GetComponent<PartAnimation> ().SettingAnimation (thisButton.Name + str);
		}
	}

	void CameraGoto(int id)
	{
		GameManager.instance.inCameraPosition = true;
		GameManager.instance.nowCamPositionID = id;
		CarControl.instance.camNowPosX = 0;
		CarControl.instance.camNowRotateY = Camera.main.transform.rotation.eulerAngles.y;
		Camera.main.transform.DOMove (GameManager.instance.allCamPosition [id].position, GameManager.instance.cameraMoveTime).SetEase(GameManager.instance.cameraMoveEase).OnComplete(CameraGotoFinish);
		Camera.main.transform.DORotate (GameManager.instance.allCamPosition [id].rotation.eulerAngles, GameManager.instance.cameraMoveTime).SetEase (GameManager.instance.cameraMoveEase);
	}

	void CameraGotoFinish()
	{
		ChangePart ();
		if (GameManager.instance.inCameraPosition) {
			GameManager.instance.cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
			CarControl.instance.camNowRotateY = Camera.main.transform.rotation.eulerAngles.y;

		}
	}

	void ShowFloatButton()
	{
		if (thisButton.Description != null) {
			UIManager.instance.float3DButton.gameObject.SetActive (true);
			UIManager.instance.float3DButton.SetThisTarget (GameObject.Find(AppData.GetSamples (thisButton.Description).Asset),thisButton);
			//UIManager.instance.float3DButton.SetThisTarget (CarStudio.objects[AppData.GetCarPartData(Scene1_UI.CarSeleted,thisButton.Name)]. thisButton.,thisButton);
		}
	}
}
