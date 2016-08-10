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
			descriptionButton = thisButton;

			if (customType == CustomType.TextureColor && !UIManager.instance.isPaintBarOut) {
				UIManager.instance.PaintBarAnimation (true);
				CameraGoBack ();
				//GameManager.instance.ChangeCustomTexture (thisID);
			}
			else{
				if (UIManager.instance.isPaintBarOut && !isPaint) {
					UIManager.instance.PaintBarAnimation (false);
				}

				if (!isPaint) {
					UIManager.instance.ChangeDescriptionButtons(descriptionButton.TextureDescription!=null,descriptionButton.MovieDescription!=null,string.IsNullOrEmpty(descriptionButton.PdfDescription));
					UIManager.instance.nowSelectedButton = thisButton;

				}

				if (customType == CustomType.OutsidePart) {

					if (descriptionButton.Type == null) {
						ChangePart ();
					} else {
						CameraGoto (int.Parse(descriptionButton.Type));
					}
				}
				else if (customType == CustomType.SpecialCar) {
					CarStudio.LoadTemplate (thisButton.Description);
					CameraGoBack ();
				}
				else if (customType == CustomType.CNG) {
//					CarData carData = AppData.GetCarAllParts().;
//					CarStudio.LoadTemplate (carData.CNG);
					CameraGoto (2);
				}
				else if (customType == CustomType.PaintingCar) {
					CarStudio.LoadTemplate (thisButton.Description);
					CameraGoBack ();
				}

				CarStudio.SaveCustumUserCar("save");
				Texture2D img;
				if (CarStudio.Exists (descriptionButton.Name)) {
					img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;
				} else {
					img = Resources.Load (descriptionButton.Icon) as Texture2D;
				}
				thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));

				Debug.Log (descriptionButton.Type);
			}
		}
	}

	void ChangePart()
	{
		if (customType == CustomType.CNG) {
			CarStudio.LoadTemplate(AppData.GetCarDataByName(Scene1_UI.CarSeleted).CNG);
		} else {
			if (CarStudio.Exists(thisButton.Name)) {
				CarStudio.RemovePart (thisButton.Name);
			} else {
				CarStudio.AddPart (thisButton.Name);
			}
		}
	}

	void CameraGoto(int id)
	{
		GameManager.instance.inCameraPosition = true;
		Camera.main.transform.DOMove (GameManager.instance.allCamPosition [id].position, GameManager.instance.cameraMoveTime).SetEase(GameManager.instance.cameraMoveEase).OnComplete(CameraGotoFinish);
		Camera.main.transform.DORotate (GameManager.instance.allCamPosition [id].rotation.eulerAngles, GameManager.instance.cameraMoveTime).SetEase (GameManager.instance.cameraMoveEase);
	}

	void CameraGoBack()
	{
		if (GameManager.instance.inCameraPosition) {
			GameManager.instance.inCameraPosition = false;
			Camera.main.transform.DOMove (GameManager.instance.cameraInitPosition, GameManager.instance.cameraMoveTime).SetEase(GameManager.instance.cameraMoveEase);
		}
	}

	void CameraGotoFinish()
	{
		ChangePart ();
	}
}
