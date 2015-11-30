using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Mathx {

	public static Vector3 InterpolatedPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		return (1 - t)*(1 - t)*(1 - t) * p0 +
			   3 * (1 - t)*(1 - t) * t * p1 +
			   3 * (1 - t) * t * t * p2 +
			   t * t * t * p3;
	}

}
