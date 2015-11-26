using UnityEngine;
using System.Collections;

public class PlayerBoat : MonoBehaviour {

	static PlayerBoat instance;

	public static PlayerBoat Instance{
		get {
			return instance;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
		}
	}

	public void OnPlayerInput(){
		PlayerController.Instance.Dragging = true;
	}
}
