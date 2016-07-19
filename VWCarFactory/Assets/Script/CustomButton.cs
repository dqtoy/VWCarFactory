using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CustomType
{
	TextureColor,
	OutsidePart,
	ElecDevice,
	InsidePart,
	Other
}

public class CustomButton : MonoBehaviour {

	public CustomType customType;
	public int thisID;
	public Text thisText;
	public Image thisImage;

	// Use this for initialization
	void Start () {
	
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

	public void ClickThisButton()
	{
		if (customType == CustomType.TextureColor) {
			GameManager.instance.ChangeCustomTexture (thisID);
		}
	}
}
