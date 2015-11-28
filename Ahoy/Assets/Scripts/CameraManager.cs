using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class CameraManager : MonoBehaviour {

	[SerializeField] protected Camera shipGameplayCamera;

	static CameraManager instance;

	public static Vector3 badVector = Vector3.one * 9999;

	public static CameraManager Instance {
		get {
			return instance;
		}
	}

	public static Vector3 TestForHitFromScreen(Vector3 inputPoint){
		Ray fromCamera = instance.shipGameplayCamera.ScreenPointToRay(inputPoint);
		RaycastHit info;
		if (Physics.Raycast(fromCamera, out info)){
			return info.point;
		}
		return badVector;
	}

	public static void SendInputAtScreenPoint(Vector3 screenPoint){
		RaycastHit info;
		Ray ray = instance.shipGameplayCamera.ScreenPointToRay(screenPoint);
		if (Physics.Raycast(ray, out info)){
			if (info.collider.GetComponent<AcceptsInput>() != null){
				info.collider.SendMessage("OnPlayerInput");
			}
		}
	}

	public Camera ShipGameplayCamera {
		get {	
			return shipGameplayCamera;
		}
	}

	void Awake(){
		instance = this;
	}
}
