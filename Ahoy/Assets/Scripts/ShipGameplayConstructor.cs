using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ShipGameplayConstructor : MonoBehaviour {

	[SerializeField] protected GameObject playerControllerPrefab;
	[SerializeField] protected GameObject playerBoatPrefab;
	[SerializeField] protected GameObject oceanPrefab;
	[SerializeField] protected GameObject moveMarkerManagerPrefab;

	Vector3 defaultStartingPosition = new Vector3(-10, 0, -10);

	public void ConstructNewGame(){
		var playerController = GameObject.Instantiate(playerControllerPrefab) as GameObject;
		playerController.transform.SetParent(this.transform);

		var ocean = GameObject.Instantiate(oceanPrefab) as GameObject;
		ocean.transform.localPosition = Vector3.zero;
		ocean.transform.localRotation = Quaternion.identity;

		var playerBoat = GameObject.Instantiate(playerBoatPrefab) as GameObject;
		playerBoat.transform.localPosition = defaultStartingPosition;
		playerBoat.transform.localRotation = Quaternion.identity;

		var moveMarkerManager = GameObject.Instantiate(moveMarkerManagerPrefab) as GameObject;
		moveMarkerManager.transform.SetParent(this.transform);
		moveMarkerManager.transform.localPosition = Vector3.zero;
		moveMarkerManager.transform.localRotation = Quaternion.identity;
	}

}
