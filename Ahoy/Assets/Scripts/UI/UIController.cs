using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class UIController : MonoBehaviour {

	static UIController instance;

	List<Canvas> canvases;

	public static UIController Instance {
		get {
			return instance;
		}
	} 

	public void SetCameraForCanvases(Camera camera){
		canvases.ForEach(canvas => canvas.worldCamera = camera);
	}

	void Awake(){
		if (instance == null){
			instance = this;
			canvases = GetComponentsInChildren<Canvas>().ToList();
		}
		else {
			Destroy(this.gameObject);
			Debug.LogWarning("Destroyed duplicate instance of UIController.");
		}
	}

}
