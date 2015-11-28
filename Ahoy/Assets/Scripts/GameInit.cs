using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GameInit : MonoBehaviour {

	[SerializeField] protected Camera gameCamera;
	[SerializeField] protected GameObject uiPrefab;
	[SerializeField] protected ShipGameplayConstructor shipGameplayConstructor;

	static GameInit instance;

	public static GameInit Instance {
		get {
			return instance;
		}
	}

	// Leave awake for any preconfig
	void Awake(){
		instance = this;
	}

	void Start(){
		this.StartSafeCoroutine(Initialize());
	}

	IEnumerator Initialize(){
		yield return this.StartSafeCoroutine(InitializeUI());
		yield return this.StartSafeCoroutine(InitializeShipGameplay());
	}

	IEnumerator InitializeUI(){
		GameObject ui = GameObject.Instantiate(uiPrefab.gameObject) as GameObject;

		UIController uiController = ui.GetComponent<UIController>();
		if (uiController == null){
			Debug.LogError("No UI Controller present on UI Prefab.");
		}

		ui.transform.SetParent(this.transform);
		uiController.SetCameraForCanvases(gameCamera);

		yield break;
	}

	IEnumerator InitializeShipGameplay(){
		shipGameplayConstructor.ConstructNewGame();
		yield break;
	}

}
