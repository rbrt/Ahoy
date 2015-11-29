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

	Vector3 initialPoint = Vector3.zero,
			currentPoint = Vector3.zero;

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
		currentPoint = PlayerController.LastInputPosition;
		MoveMarkerManager.SetTargetRotation(initialPoint, currentPoint);
	}

	public override void HandleInputUp(){
		
	}

	public override void HandleInputDown(){
		
	}

	public override void DrawInput(){
		
	}

	public override void OnSetHandler(){
		initialPoint = MoveMarkerManager.CurrentMarkerPositionOnCamera();
		initialPoint.z = 0;

		currentPoint = PlayerController.LastInputPosition;

		MoveMarkerManager.ClearRotationVisualizer();
	}

	public override void OnUnSetHandler(){
		CleanHandler();
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	void CleanHandler(){
		initialPoint = Vector3.zero;
		currentPoint = Vector3.zero;
	}

	bool SetRotation(){
		MoveMarkerManager.SetTargetRotation(initialPoint, currentPoint);
		MoveMarkerManager.IndicateRotationMoveSet();
		return releasesControlOnAction;
	}

}
