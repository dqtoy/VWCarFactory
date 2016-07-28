using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropUI : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler 
{
	public void OnDrag(PointerEventData eventData)
	{
		GetComponent<RectTransform>().pivot.Set(0,0);
		transform.position = new Vector3(transform.position.x,Input.mousePosition.y,transform.position.z);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//transform.localScale=new Vector3(0.7f,0.7f,0.7f);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//transform.localScale=new Vector3(1f,1f,1f);
	}
}