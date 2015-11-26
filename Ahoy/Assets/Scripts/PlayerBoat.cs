using UnityEngine;
using System.Collections;

public class PlayerBoat : AcceptsInput {

	static PlayerBoat instance;
	const float rotationDegreesPerSecond = 85;

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

	public void FacePoint(Vector3 convertedPoint){
		Quaternion target = Quaternion.LookRotation(convertedPoint - transform.position, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationDegreesPerSecond * Time.deltaTime);

		Debug.Log("Rotating " + transform.rotation.eulerAngles.ToString());
	}

	public override void OnPlayerInput(){
		PlayerController.Instance.Dragging = true;
	}
}
