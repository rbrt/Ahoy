using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TurningHandler : InputHandler {

	float actionWaitTime = 1f;
	bool releasesControlOnAction = true,
		 interruptedBySendMessage = true;

	public override float ActionWaitTime {
		get {
			return actionWaitTime;
		}
	}

	public override System.Func<bool> InputAction {
		get {
			return () => SetRotation();
		}
	}

	public override bool InterruptedBySendMessage{
		get {
			return interruptedBySendMessage;
		}
	}

	public override void HandleDragInput(){
		if (PlayerController.Dragging){
			PlayerBoat.Instance.FacePoint(CameraManager.TestForHitFromScreen(PlayerController.LastInputPosition));
		}
	}

	public override void HandleInputUp(){
		
	}

	public override void HandleInputDown(){
		
	}

	public override void DrawInput(){
		
	}

	public override void OnSetHandler(){
		
	}

	public override void OnUnSetHandler(){
		
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	bool SetRotation(){
		MoveMarker.SetTargetRotation(PlayerBoat.Instance.transform.rotation);
		PlayerBoat.Instance.transform.rotation = Quaternion.identity;
		return releasesControlOnAction;
	}

}
