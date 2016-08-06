using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SamplePhotoButton : MonoBehaviour {

	public int thisID;
	public Image photo;
	public Text photoName;
	public Text description;

	public void SetSamplePhoto(Sprite tex,string nam,string des)
	{
		photo.sprite = tex;	
		photoName.text = nam;
		description.text = des;
	}

	public void ClickThisButton()
	{
		UIManager.instance.largeSamplePhoto.GetComponent<Image> ().sprite = photo.sprite;
		UIManager.instance.LargeSamplePhotoShow (true);
	}
}
