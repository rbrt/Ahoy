using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class NullMoveMarker : MoveMarker {
	public override void IndicateFiringMoveSet(){
		
	}

	public override void IndicateTurningMoveSet(){

	}

	public override void SetTargetRotation(Vector3 initialPoint, Vector3 currentPoint){
		
	}

	public override void SetPlayerShot(Vector3 playerShot){
		
	}

	public override void OnSelectMoveMarker(){
		
	}

	public override void OnUnselectMoveMarker(){
		
	}

	public override void DestroyMarker(){
		
	}
}
