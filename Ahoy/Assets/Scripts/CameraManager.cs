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

	public static CameraManager Instance {
		get {
			return instance;
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
