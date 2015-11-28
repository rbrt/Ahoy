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

	float actionWaitTime = 1f;
	bool releasesControlOnAction = false,
		 interruptedBySendMessage = true;

	[SerializeField] protected GameObject moveMarkerPrefab;
	[SerializeField] protected PathVisualizer pathVisualizer;

	List<MoveMarker> moveMarkers;
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
			drawPoints.Add(PlayerController.TestForHitFromScreen(PlayerController.LastInputPosition));
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
		SetMoveAtPoint(PlayerBoat.Instance.transform.position);
	}

	public override void OnUnSetHandler(){
		DrawInput();
	}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return testSeconds > actionWaitTime;
	}

	public List<MoveMarker> MoveMarkers{
		get {
			return moveMarkers;
		}
	}

	void Awake(){
		moveMarkers = new List<MoveMarker>();
		movePoints = new List<Vector3>();
	}

	bool SetMove(){
		var point = PlayerController.TestForHitFromScreen(PlayerController.LastInputPosition);
		if (point != PlayerController.badVector){
			SetMoveAtPoint(point);
		}

		return releasesControlOnAction;
	}

	void SetMoveAtPoint(Vector3 point){
		movePoints.Add(point);
		pathVisualizer.IndicateMoveSet();
		CreateMarker(point);
	}

	void CreateMarker(Vector3 position){
		var marker = GameObject.Instantiate(moveMarkerPrefab);
		marker.transform.position = position;
		marker.transform.rotation = Quaternion.identity;

		moveMarkers.Add(marker.GetComponent<MoveMarker>());
	}

	void ClearMarkers(){
		for (int i = 0; i < moveMarkers.Count; i++){
			Destroy(moveMarkers[i].gameObject);
		}
		moveMarkers.Clear();
	}

	public void ClearMoves(){
		movePoints.Clear();
		ClearMarkers();
		pathVisualizer.ClearPoints();
	}
}
