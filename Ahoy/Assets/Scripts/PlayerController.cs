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

	static PlayerController instance;

	bool dragging = false,
		 holding = false;
	Vector3 lastPosition = Vector3.zero;
	Vector3 badVector;

	float timeWithoutMoving = 0,
		  timeUntilMoveSetInSeconds = 2;

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
		}
	}

	void Update(){
		HandleInput();
		DrawPath();
	}

	List<Vector3> drawPoints;
	void DrawPath(){
		if (!dragging && movePoints.Count == 0){
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
		if (dragging){
			//Debug.Log(Time.time - timeWithoutMoving);

			if (Time.time - timeWithoutMoving >= timeUntilMoveSetInSeconds){
				SetMove(Input.mousePosition);
				timeWithoutMoving = Time.time;
			}

			if (Input.mousePosition != lastPosition){
				timeWithoutMoving = Time.time;
			}
			lastPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonDown(0)){
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
		}
	}

	Vector3 TestForHitFromScreen(Vector3 inputPoint){
		Ray fromCamera = Camera.main.ScreenPointToRay(inputPoint);
		RaycastHit info;
		if (Physics.Raycast(fromCamera, out info)){
			return info.point;
		}
		return badVector;
	}

	public void ClearMoves(){
		movePoints.Clear();
		pathVisualizer.ClearPoints();
	}

	void SendInputAtScreenPoint(Vector3 screenPoint){
		RaycastHit info;
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		if (Physics.Raycast(ray, out info)){
			info.collider.SendMessage("OnPlayerInput");
		}
	}

}
