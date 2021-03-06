﻿using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class FiringHandler : InputHandler {
	
	float actionWaitTime = 1f;
	bool releasesControlOnAction = true,
		 interruptedBySendMessage = true;

	Vector3 initialPoint,
			currentPoint;

	public override float ActionWaitTime {
		get {
			return actionWaitTime;
		}
	}

	public override System.Func<bool> InputAction {
		get {
			return () => SetFiringTrajectory();
		}
	}

	public override bool InterruptedBySendMessage{
		get {
			return interruptedBySendMessage;
		}
	}

	public override void HandleDragInput(){
		currentPoint = PlayerController.LastInputPosition;
		MoveMarkerManager.SetTargetFiringStrength(GetShotVector());
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

		MoveMarkerManager.ClearFiringVisualizer();
	}

	public override void OnUnSetHandler(){
		CleanHandler();
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	public override void PassiveAction(){
		
	}

	Vector3 GetShotVector(){
		return currentPoint - initialPoint;
	}

	bool SetFiringTrajectory(){
		MoveMarkerManager.IndicateFiringMoveSet();
		MoveMarkerManager.SetTargetFiringStrength(GetShotVector());
		return releasesControlOnAction;
	}

	void Awake(){
		CleanHandler();
	}

	void CleanHandler(){
		initialPoint = Vector3.zero;
		currentPoint = Vector3.zero;
	}

}
