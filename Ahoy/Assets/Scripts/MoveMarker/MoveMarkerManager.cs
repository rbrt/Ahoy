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

	public static MoveMarker CurrentMarker {
		get {
			return instance.currentMarker;
		}
	}

	public static Vector3 PositionOnCamera(){
		return CameraManager.WorldToGameCameraPoint(instance.currentMarker.transform.position);
	}

	public static void ClearTargetMarker(){
		instance.currentMarker = instance.nullMoveMarker;
	}

	public static void ClearFiringVisualizer(){
		instance.currentMarker.SetPlayerShot(Vector3.zero);
	}

	public static void SetTargetRotation(Quaternion target){
		instance.currentMarker.SetTargetRotation(target);
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

	public static void IndicateMoveSet(){
		if (instance.currentMarker != null){
			instance.currentMarker.IndicateMoveSet();
		}
	}

	public static void SetCurrentMarker(MoveMarker marker){
		instance.currentMarker = marker;
	}

	void Awake(){
		if (instance == null){
			instance = this;
			nullMoveMarker = (new GameObject("NullMoveMarker").AddComponent<NullMoveMarker>());
			nullMoveMarker.transform.SetParent(this.transform);
			currentMarker = nullMoveMarker;
		}
		else {
			Destroy(this.gameObject);
			Debug.LogError("Destroyed duplicate instance of MoveMarkerManager");
		}
	}

}
