using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class FiringHandler : InputHandler {

	[SerializeField] protected ShotVisualizer shotVisualizer;

	float actionWaitTime = 1f;
	bool releasesControlOnAction = true,
		 interruptedBySendMessage = true;

	List<Vector3> shotPoints;

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
		shotPoints.Clear();
		shotPoints.Add(PlayerBoat.Instance.transform.position);
		shotPoints.Add(PlayerController.TestForHitFromScreen(PlayerController.LastInputPosition));
	}

	public override void HandleInputUp(){

	}

	public override void HandleInputDown(){

	}

	public override void DrawInput(){
		if (shotPoints.Count == 0){
			return;
		}

		List<Vector3> drawPoints = new List<Vector3>();
		drawPoints.AddRange(shotPoints);

		Vector3 temp = Vector3.one;
		for (int i = 0; i < drawPoints.Count; i++){
			temp = drawPoints[i];
			temp.y = 1;
			drawPoints[i] = temp;
		}

		shotVisualizer.SetPoints(drawPoints);
	}

	public override void OnSetHandler(){
		
	}

	public override void OnUnSetHandler(){

	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	Vector3 GetShotVector(){
		return PlayerBoat.Instance.transform.position - PlayerController.TestForHitFromScreen(PlayerController.LastInputPosition);
	}

	bool SetFiringTrajectory(){
		shotVisualizer.IndicateMoveSet();
		MoveMarker.SetTargetFiringStrength(GetShotVector());
		return releasesControlOnAction;
	}

	void Awake(){
		shotPoints = new List<Vector3>();
	}

}
