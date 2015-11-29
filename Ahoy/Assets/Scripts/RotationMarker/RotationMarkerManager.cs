using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RotationMarkerManager : MonoBehaviour {

	[SerializeField] protected GameObject rotationMarkerPrefab;

	Camera shipGameplayCamera;
	Canvas canvas;

	static RotationMarkerManager instance;

	List<RotationMarker> rotationMarkerList;

	public static RotationMarkerManager Instance {
		get {
			return instance;
		}
	}

	public RealRotationMarker CreateRotationMarker(Vector3 position){
		RealRotationMarker newMarker = (GameObject.Instantiate(rotationMarkerPrefab) as GameObject).GetComponent<RealRotationMarker>();
		rotationMarkerList.Add(newMarker);
		var markerTransform = newMarker.GetComponent<RectTransform>();
		markerTransform.SetParent(this.transform);
		markerTransform.localRotation = Quaternion.identity;
		markerTransform.localScale = Vector3.one;
		markerTransform.localPosition = Vector3.zero;

		var translatedPoint = shipGameplayCamera.WorldToScreenPoint(position);
		Vector2 uiPosition = new Vector2(translatedPoint.x / canvas.scaleFactor,
										 translatedPoint.y / canvas.scaleFactor);
		
		markerTransform.anchoredPosition = uiPosition;
		return newMarker;
	}

	public void ClearRotationMarkerList(){
		for (int i = 0; i < rotationMarkerList.Count; i++){
			Destroy(rotationMarkerList[i].gameObject);
		}
		rotationMarkerList.Clear();
	}

	void Awake(){
		if (instance == null){
			instance = this;
			rotationMarkerList = new List<RotationMarker>();
			shipGameplayCamera = CameraManager.Instance.ShipGameplayCamera;
			canvas = GetComponent<Canvas>();
		}
		else{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of RotationMarkerManager");
		}
	}

}
