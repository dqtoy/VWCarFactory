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
	
	public void ClickThis()
	{
		if (type == DescriptionType.PhotoButton) {
            UIManager.instance.samplePhotoContent.transform.position = new Vector3(UIManager.instance.samplePhotoContent.transform.position.x,0, UIManager.instance.samplePhotoContent.transform.position.z);

            UIManager.instance.SamplePhotoWindowShow (true);
			UIManager.instance.SamplePhotoRefresh ();
		} else if (type == DescriptionType.VideoButton) {
			UIManager.instance.SampleVideoWindowShow (true);
		}
	}
}
