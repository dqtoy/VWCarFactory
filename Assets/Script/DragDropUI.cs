using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.ootii.Messages;

public class DragDropUI : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler 
{

	public void OnDrag(PointerEventData eventData)
	{
		GetComponent<RectTransform>().pivot.Set(0,0);
		if (transform.localPosition.y <= UIManager.instance.scrollBounds.y) {
			transform.position = new Vector3 (transform.position.x, Input.mousePosition.y, transform.position.z);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//transform.localScale=new Vector3(0.7f,0.7f,0.7f);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//transform.localPosition = new Vector3 (transform.localPosition.x, UIManager.instance.scrollBounds.y, transform.localPosition.z);
		MessageDispatcher.SendMessage (UIManager.instance.gameObject,"OnDragFinish","DFinish", 0);
		//transform.localScale=new Vector3(1f,1f,1f);
	}
}