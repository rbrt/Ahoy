using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class PlayerController : MonoBehaviour {

	[SerializeField] protected MoveMarker moveMarkerPrefab;
	List<MoveMarker> moveMarkers;

	static PlayerController instance;

	const string inputLayerName = "PlayerInput";

	bool dragging = false,
		 firing = false,
		 turning = false;

	Vector3 lastPosition = Vector3.zero;
	Vector3 badVector;

	float timeWithoutMoving = 0,
		  timeUntilMoveSetInSeconds = 0;

	const float moveWaitTime = .5f,
				firingWaitTime = 1f,
				turningWaitTime = 1f;

	List<Vector3> movePoints;
	PathVisualizer pathVisualizer;

	public bool Dragging {
		get {
			return dragging;
		}
		set {
			dragging = value;
		}
	}

	public bool Firing {
		get {
			return firing;
		}
		set {
			firing = value;
		}
	}

	public bool Turning {
		get {
			return turning;
		}
		set {
			turning = value;
		}
	}

	public Vector3 LastPosition {
		get {
			return lastPosition;
		}
		set {
			lastPosition = value;
		}
	}

	public static PlayerController Instance {
		get {
			return instance;
		}
	}

	void Awake (){
		if (instance == null){
			instance = this;
			movePoints = new List<Vector3>();
			pathVisualizer = FindObjectOfType<PathVisualizer>();
			badVector = Vector3.one * 5000;
			moveMarkers = new List<MoveMarker>();
		}
	}

	void Update(){
		HandleInput();
		DrawPath();
	}

	List<Vector3> drawPoints;
	void DrawPath(){
		if ((!dragging && movePoints.Count == 0) ||
			turning ||
			firing)
		{
			return;
		}

		drawPoints = new List<Vector3>();
		drawPoints.Add(PlayerBoat.Instance.transform.position);
		drawPoints.AddRange(movePoints);

		if (dragging){
			drawPoints.Add(TestForHitFromScreen(lastPosition));
		}

		for (int i = 0; i < drawPoints.Count; i++){
			drawPoints[i] = drawPoints[i] + Vector3.up;
		}

		pathVisualizer.SetPoints(drawPoints);

	}

	void HandleInput(){
		System.Action inputAction = () => {};
		if (firing){
			inputAction = () => SetFiringTrajectory();
			timeUntilMoveSetInSeconds = firingWaitTime;

			if (dragging){
				PlayerBoat.Instance.FacePoint(TestForHitFromScreen(lastPosition));
			}
		}
		else if (turning){
			inputAction = () => SetRotation();
			timeUntilMoveSetInSeconds = turningWaitTime;

			if (dragging){
				PlayerBoat.Instance.FacePoint(TestForHitFromScreen(lastPosition));
			}
		}
		else{
			timeUntilMoveSetInSeconds = moveWaitTime;
			inputAction = () => SetMove(Input.mousePosition);
		}

		if (dragging){
			if (Time.time - timeWithoutMoving >= timeUntilMoveSetInSeconds){
				inputAction();
				timeWithoutMoving = Time.time - timeWithoutMoving * 2;
			}

			if (Input.mousePosition != lastPosition){
				timeWithoutMoving = Time.time;
			}
			lastPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonDown(0)){
			dragging = true;
			timeWithoutMoving = Time.time;
			SendInputAtScreenPoint(Input.mousePosition);		
		}
		if (Input.GetMouseButtonUp(0)){
			dragging = false;
		}
	}



	void SetMove(Vector3 inputPoint){
		var point = TestForHitFromScreen(inputPoint);
		if (point != badVector){
			movePoints.Add(point);
			pathVisualizer.IndicateMoveSet();
			CreateMarker(point);
		}
	}

	void SetRotation(){
		MoveMarker.SetTargetRotation(PlayerBoat.Instance.transform.rotation);
		PlayerBoat.Instance.transform.rotation = Quaternion.identity;
		timeWithoutMoving = Time.time;
		dragging = false;
		turning = false;
	}

	void SetFiringTrajectory(){
		firing = false;
	}

	Vector3 TestForHitFromScreen(Vector3 inputPoint){
		Ray fromCamera = Camera.main.ScreenPointToRay(inputPoint);
		RaycastHit info;
		if (Physics.Raycast(fromCamera, out info)){
			return info.point;
		}
		return badVector;
	}

	void SendInputAtScreenPoint(Vector3 screenPoint){
		RaycastHit info;
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		if (Physics.Raycast(ray, out info)){
			if (info.collider.GetComponent<AcceptsInput>() != null){
				info.collider.SendMessage("OnPlayerInput");
			}
		}
		else if (MoveMarkerMenu.Instance.Open){
			MoveMarkerMenu.Instance.HideMenu();
		}
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

	public void ClearShots(){
		
	}

	public void ClearTurns(){
		
	}

	public void TestRun(){
		
	}
}
