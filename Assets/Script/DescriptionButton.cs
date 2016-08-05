using UnityEngine;
using System.Collections;

public enum DescriptionType
{
	PhotoButton,
	VideoButton,
	PDFButton
}

public class DescriptionButton : MonoBehaviour {

	public DescriptionType type;

	// Use this for initialization
	void Start () {
	
	}
	
	public void ClickThis()
	{
		if (type == DescriptionType.PhotoButton) {
			
		} else if (type == DescriptionType.VideoButton) {
			
		} else {
			
		}
	}
}
