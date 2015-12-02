using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GameplayCameraHandler : InputHandler {

	Camera gameplayCamera;
	Vector3 lastFramePosition;
	Vector3 passiveOffset;
	Vector2 frameThreshold;

	public override float ActionWaitTime {
		get {
			return 0;
		}
	}

	public override System.Func<bool> InputAction {
		get {
			return () => true;
		}
	}

	public override bool InterruptedBySendMessage{
		get {
			return false;
		}
	}

	public override void HandleDragInput(){

		Vector3 inputVector = lastFramePosition - PlayerController.LastInputPosition;
		lastFramePosition = PlayerController.LastInputPosition;

		Debug.Log(inputVector + " " + (inputVector.magnitude * Time.deltaTime));
		inputVector = new Vector3(inputVector.x, 0, inputVector.y);
		gameplayCamera.transform.position += inputVector * Time.deltaTime * .25f * inputVector.magnitude;
	}

	public override void HandleInputUp(){
		PlayerController.Instance.UnsetInputHandler();
	}

	public override void HandleInputDown(){}

	public override void DrawInput(){}

	public override void OnSetHandler(){
		lastFramePosition = PlayerController.LastInputPosition;
	}

	public override void OnUnSetHandler(){
		lastFramePosition = Vector3.zero;
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return false;
	}

	public override void PassiveAction(){
		lastFramePosition = PlayerController.LastInputPosition;
		passiveOffset = Vector3.zero;

		if (lastFramePosition.x < frameThreshold.x || lastFramePosition.x > gameplayCamera.pixelWidth - frameThreshold.x) {
			passiveOffset.x = lastFramePosition.x  - (gameplayCamera.pixelWidth / 2f);
		}
		if (lastFramePosition.y < frameThreshold.y || lastFramePosition.y > gameplayCamera.pixelHeight - frameThreshold.y){
			passiveOffset.z = lastFramePosition.y - (gameplayCamera.pixelHeight / 2f);
		}

		gameplayCamera.transform.position += passiveOffset * Time.deltaTime * .25f;
	}

	void Start(){
		gameplayCamera = CameraManager.Instance.ShipGameplayCamera;
		frameThreshold = new Vector2(gameplayCamera.pixelWidth * .1f, gameplayCamera.pixelHeight * .1f);
	}

}
