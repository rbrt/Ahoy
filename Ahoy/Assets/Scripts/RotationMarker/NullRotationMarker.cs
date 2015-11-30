using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class NullRotationMarker : RotationMarker {

	public override void SetRotationImageFill(float fill){
		
	}

	public override float SetDirectionImageTarget(Vector3 target){
		return -1;
	}

	public override void IndicateRotationMoveSet(){
		
	}

}
