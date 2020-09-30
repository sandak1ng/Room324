using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColourChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Text theText;

	public void OnPointerEnter (PointerEventData eventData) {
		theText.color = Color.red;
	}

	public void OnPointerExit (PointerEventData eventData) {
		theText.color = Color.white;
	}
}
