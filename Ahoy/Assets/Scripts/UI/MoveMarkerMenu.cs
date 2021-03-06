﻿using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MoveMarkerMenu : MonoBehaviour {

	static MoveMarkerMenu instance;

	[SerializeField] protected RectTransform mainPanel;

	bool transitioning = false,
		 open = false;

	float presentTime = .25f,
		  dismissalTime = .2f;

	CanvasGroup canvasGroup;
	Canvas canvas;

	Camera shipGameplayCamera;

	public static MoveMarkerMenu Instance{
		get {
			return instance;
		}
	}

	public bool Open {
		get {
			return open;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
			canvasGroup = GetComponent<CanvasGroup>();
			canvas = GetComponent<Canvas>();
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			shipGameplayCamera = CameraManager.Instance.ShipGameplayCamera;
		}
	}

	public bool ContainsPoint(Vector3 point){
		return GetComponent<RectTransform>().rect.Contains(point);
	}

	public void HideMenu(){
		if (transitioning || !open){
			return;
		}

		open = false;
		this.StartSafeCoroutine(DismissMenu());
	}

	public IEnumerator ShowMenu(Vector3 worldPoint){
		while (transitioning || open){
			yield return null;
		}

		PlayerController.Instance.UnsetInputHandler();

		open = true;
		var translatedPoint = shipGameplayCamera.WorldToScreenPoint(worldPoint);
		var cachedTransform = mainPanel.GetComponent<RectTransform>();
		Vector2 uiPosition = new Vector2(translatedPoint.x / canvas.scaleFactor,
										 translatedPoint.y / canvas.scaleFactor);

		if (uiPosition.x - cachedTransform.sizeDelta.x <= 0){
			uiPosition.x += cachedTransform.sizeDelta.x / 2;
		}
		else if (uiPosition.x + cachedTransform.sizeDelta.x > Screen.width){
			uiPosition.x -= cachedTransform.sizeDelta.x / 2;
		}

		if (uiPosition.y - cachedTransform.sizeDelta.y <= 0){
			uiPosition.y += cachedTransform.sizeDelta.y / 2;
		}
		else if (uiPosition.y + cachedTransform.sizeDelta.y > Screen.height){
			uiPosition.y -= cachedTransform.sizeDelta.y / 2;
		}

		cachedTransform.anchoredPosition = uiPosition;

		this.StartSafeCoroutine(PresentMenu());
	}

	public void EngageTurning(){
		PlayerController.Instance.Turning = true;
		HideMenu();
	}

	public void EngageAiming(){
		PlayerController.Instance.Firing = true;
		HideMenu();
	}

	IEnumerator DismissMenu(){
		transitioning = true;

		for (float i = 0; i <= 1; i += Time.deltaTime / dismissalTime){
			canvasGroup.alpha = 1 - i;
			yield return null;
		}
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;

		transitioning = false;
	}

	IEnumerator PresentMenu(){
		transitioning = true;

		for (float i = 0; i <= 1; i += Time.deltaTime / presentTime){
			canvasGroup.alpha = i;
			yield return null;
		}
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;

		transitioning = false;
	}
}
