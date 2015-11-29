using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MoveMarkerManager : MonoBehaviour {

	[SerializeField] protected GameObject moveMarkerPrefab;

	const float proxyBoatAlpha = .6f;
	const float firingIndicationSensitivity = 6; // 4 gives a reasonable visualization on a power scale of 0 - 4.5

	static MoveMarkerManager instance;

	public static MoveMarkerManager Instance {
		get {
			return instance;
		}
	}

	MoveMarker currentMarker;
	NullMoveMarker nullMoveMarker;

	List<MoveMarker> moveMarkers;

	public static MoveMarker CurrentMarker {
		get {
			return instance.currentMarker;
		}
	}

	public static Vector3 CurrentMarkerPositionOnCamera(){
		return CameraManager.WorldToGameCameraPoint(instance.currentMarker.transform.position);
	}

	public static Vector3 CurrentMarkerPositionOnCameraForRotation(){
		Vector3 offset = instance.currentMarker.transform.forward;
		return CameraManager.WorldToGameCameraPoint(instance.currentMarker.transform.position + offset);
	}

	public static void ClearTargetMarker(){
		instance.currentMarker.OnUnselectMoveMarker();
		instance.currentMarker = instance.nullMoveMarker;
	}

	public static void ClearFiringVisualizer(){
		instance.currentMarker.SetPlayerShot(Vector3.zero);
	}

	public static void ClearRotationVisualizer(){
		instance.currentMarker.SetTargetRotation(Vector3.zero, Vector3.zero);
	}

	public static void SetTargetRotation(Vector3 initialPoint, Vector3 currentPoint){
		instance.currentMarker.SetTargetRotation(initialPoint, currentPoint);
	}

	public static void IndicateRotationMoveSet(){
		instance.currentMarker.IndicateTurningMoveSet();
	}

	public static void SetTargetFiringStrength(Vector3 shot){
		Vector3 scaledForScreen = shot;
		scaledForScreen.x /= Screen.width / firingIndicationSensitivity;
		scaledForScreen.y /= Screen.height / firingIndicationSensitivity;

		float strength = PlayerBoat.maxFiringPower;
		if (scaledForScreen.magnitude > strength){
			scaledForScreen = Vector3.ClampMagnitude(scaledForScreen, strength);
		}

		instance.currentMarker.SetPlayerShot(scaledForScreen);
	}

	public static void IndicateFiringMoveSet(){
		if (instance.currentMarker != null){
			instance.currentMarker.IndicateFiringMoveSet();
		}
	}

	public static void SetCurrentMarker(MoveMarker marker){
		instance.currentMarker = marker;
		instance.StartSafeCoroutine(MoveMarkerMenu.Instance.ShowMenu(marker.transform.position));
		marker.OnSelectMoveMarker();
	}

	public void CreateMarker(Vector3 position){
		var marker = GameObject.Instantiate(moveMarkerPrefab);
		marker.transform.position = position;
		marker.transform.rotation = Quaternion.identity;

		moveMarkers.Add(marker.GetComponent<MoveMarker>());
		marker.transform.SetParent(this.transform);
	}

	public void ClearMarkers(){
		for (int i = 0; i < moveMarkers.Count; i++){
			Destroy(moveMarkers[i].gameObject);
		}
		moveMarkers.Clear();
	}

	public void RemoveExtraMoveMarkers(ref List<Vector3> drawPoints){
		int index = moveMarkers.IndexOf(currentMarker);
		while (index < moveMarkers.Count() - 1){
			int currentIndex = moveMarkers.Count() - 1;
			Destroy(moveMarkers[currentIndex].gameObject);
			moveMarkers.RemoveAt(currentIndex);
			drawPoints.RemoveAt(currentIndex);
		}	
	}

	void Awake(){
		if (instance == null){
			instance = this;
			nullMoveMarker = (new GameObject("NullMoveMarker").AddComponent<NullMoveMarker>());
			nullMoveMarker.transform.SetParent(this.transform);
			currentMarker = nullMoveMarker;
			moveMarkers = new List<MoveMarker>();
		}
		else {
			Destroy(this.gameObject);
			Debug.LogError("Destroyed duplicate instance of MoveMarkerManager");
		}
	}

}
