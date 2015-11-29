using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MoveHandler : InputHandler {

	float actionWaitTime = .5f;
	bool releasesControlOnAction = false,
		 interruptedBySendMessage = true;

	[SerializeField] protected PathVisualizer pathVisualizer;

	List<Vector3> movePoints;
	List<Vector3> drawPoints;

	public override float ActionWaitTime {
		get {
			return actionWaitTime;
		}
	}

	public override System.Func<bool> InputAction {
		get {
			return () => SetMove();
		}
	}

	public override bool InterruptedBySendMessage{
		get {
			return interruptedBySendMessage;
		}
	}

	public override void HandleDragInput() {
		
	}

	public override void HandleInputUp(){
		PlayerController.Instance.UnsetInputHandler();
	}

	public override void HandleInputDown(){

	}

	public override void DrawInput(){
		if (movePoints.Count == 0){
			return;
		}

		drawPoints = new List<Vector3>();
		drawPoints.AddRange(movePoints);

		if (PlayerController.Dragging){
			drawPoints.Add(CameraManager.TestForHitFromScreen(PlayerController.LastInputPosition));
		}

		Vector3 temp = Vector3.one;
		for (int i = 0; i < drawPoints.Count; i++){
			temp = drawPoints[i];
			temp.y = 1;
			drawPoints[i] = temp;
		}

		pathVisualizer.SetPoints(drawPoints);
	}

	public override void OnSetHandler(){
		if (movePoints.Count == 0){
			SetMoveAtPoint(PlayerBoat.Instance.transform.position);
		}
		else {
			MoveMarkerManager.Instance.RemoveExtraMoveMarkers(ref movePoints);
		}
	}

	public override void OnUnSetHandler(){
		DrawInput();
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	void Awake(){
		movePoints = new List<Vector3>();
	}

	bool SetMove(){
		var point = CameraManager.TestForHitFromScreen(PlayerController.LastInputPosition);
		if (point != CameraManager.badVector){
			SetMoveAtPoint(point);
		}

		return releasesControlOnAction;
	}

	void SetMoveAtPoint(Vector3 point){
		movePoints.Add(point);
		pathVisualizer.IndicateMoveSet();
		MoveMarkerManager.Instance.CreateMarker(point);
	}

	public void ClearMoves(){
		movePoints.Clear();
		MoveMarkerManager.Instance.ClearMarkers();
		pathVisualizer.ClearPoints();
	}
}
