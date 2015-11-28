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
	public override void IndicateMoveSet(){
		
	}

	public override void SetTargetRotation(Quaternion targetRotation){
		
	}

	public override void SetPlayerShot(Vector3 playerShot){
		
	}
}
