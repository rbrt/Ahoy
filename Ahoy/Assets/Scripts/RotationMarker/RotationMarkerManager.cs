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

	Dictionary<RotationMarker, Transform> rotationMarkerMapping;

	public static RotationMarkerManager Instance {
		get {
			return instance;
		}
	}

	public RealRotationMarker CreateRotationMarker(Transform targetMarker){
		Vector3 position = targetMarker.position;
		RealRotationMarker newMarker = (GameObject.Instantiate(rotationMarkerPrefab) as GameObject).GetComponent<RealRotationMarker>();
		rotationMarkerMapping[newMarker] = targetMarker;
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
		rotationMarkerMapping.Keys.ToList().ForEach(key => Destroy(rotationMarkerMapping[key].gameObject));
		rotationMarkerMapping.Clear();
	}

	void Awake(){
		if (instance == null){
			instance = this;
			rotationMarkerMapping = new Dictionary<RotationMarker, Transform>();
			shipGameplayCamera = CameraManager.Instance.ShipGameplayCamera;
			canvas = GetComponent<Canvas>();
		}
		else{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of RotationMarkerManager");
		}
	}

	void Update(){
		rotationMarkerMapping.Keys.ToList().ForEach(key => {
			var translatedPoint = shipGameplayCamera.WorldToScreenPoint(rotationMarkerMapping[key].position);
			Vector2 uiPosition = new Vector2(translatedPoint.x / canvas.scaleFactor,
											 translatedPoint.y / canvas.scaleFactor);

			key.GetComponent<RectTransform>().anchoredPosition = uiPosition;
		});
	}

}
