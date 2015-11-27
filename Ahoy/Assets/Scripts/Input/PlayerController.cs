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
	[SerializeField] protected FiringHandler firingHandler;
	[SerializeField] protected TurningHandler turningHandler;
	[SerializeField] protected MoveHandler movingHandler;
	[SerializeField] protected NullInputHandler nullInputHandler;

	static PlayerController instance;

	const string inputLayerName = "PlayerInput";

	bool dragging = false;

	Vector3 lastPosition = Vector3.zero;
	public static Vector3 badVector = Vector3.one * 9999;

	float timeWithoutMoving = 0;

	const float moveWaitTime = .5f,
				firingWaitTime = 1f,
				turningWaitTime = 1f;

	InputHandler currentInputHandler;

	public InputHandler CurrentInputHandler{
		get {
			return currentInputHandler;
		}
		set {
			currentInputHandler = value;
		}
	}

	public static bool Dragging {
		get {
			return instance.dragging;
		}
		set {
			instance.dragging = value;
		}
	}

	public static Vector3 LastInputPosition {
		get {
			return instance.lastPosition;
		}
	}

	public bool Moving {
		get {
			return currentInputHandler == movingHandler;
		}
		set {
			if (value){
				SetInputHandler(movingHandler);
			}
		}
	}

	public bool Firing {
		get {
			return currentInputHandler == firingHandler;
		}
		set {
			if (value){
				SetInputHandler(firingHandler);
			}
		}
	}

	public bool Turning {
		get {
			return currentInputHandler == turningHandler;
		}
		set {
			if (value){
				SetInputHandler(turningHandler);
			}
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

			SetInputHandler(nullInputHandler);
		}
	}

	void Update(){
		HandleInput();
		DrawPath();
	}

	void SetInputHandler(InputHandler handler){
		currentInputHandler = handler;
		currentInputHandler.OnSetHandler();
	}

	List<Vector3> drawPoints;
	void DrawPath(){
		currentInputHandler.DrawInput();
	}

	void HandleInput(){
		if (Input.GetMouseButtonDown(0)){
			currentInputHandler.HandleInputDown();
			SendInputAtScreenPoint(Input.mousePosition);
			dragging = true;
			timeWithoutMoving = Time.time;		
		}
		if (Input.GetMouseButtonUp(0)){
			dragging = false;
			currentInputHandler.HandleInputUp();
		}

		if (dragging){
			currentInputHandler.HandleDragInput();

			if (currentInputHandler.ShouldInvokeInputAction(Time.time - timeWithoutMoving)){
				if (currentInputHandler.InputAction()){
					UnsetInputHandler();
				}
				timeWithoutMoving = Time.time + 2;
			}

			if (Input.mousePosition != lastPosition){
				timeWithoutMoving = Time.time;
			}
			lastPosition = Input.mousePosition;
		}
	}

	public void UnsetInputHandler(){
		currentInputHandler.OnUnSetHandler();
		SetInputHandler(nullInputHandler);
	}

	public static Vector3 TestForHitFromScreen(Vector3 inputPoint){
		Ray fromCamera = Camera.main.ScreenPointToRay(inputPoint);
		RaycastHit info;
		if (Physics.Raycast(fromCamera, out info)){
			return info.point;
		}
		return badVector;
	}

	public static void SendInputAtScreenPoint(Vector3 screenPoint){
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

	public void ClearShots(){
		
	}

	public void ClearTurns(){
		
	}

	public void TestRun(){
		
	}
}
