using UnityEngine;
using System.Collections;

public class PlayerBoat : AcceptsInput {

	static PlayerBoat instance;
	const float rotationDegreesPerSecond = 85;
	public const float maxFiringPower = 4.5f;

	[Range(0, maxFiringPower)]
	float boatShotStrength = 4.5f;

	public static PlayerBoat Instance{
		get {
			return instance;
		}
	}

	public float BoatShotStrength {
		get {
			return instance.boatShotStrength;
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
	}

	public override void OnPlayerInput(){
		if (!PlayerController.Instance.Firing && !PlayerController.Instance.Turning){
			PlayerController.Instance.Moving = true;
		}
	}
}
