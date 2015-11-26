using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class PathVisualizer : MonoBehaviour {

	LineRenderer lineRenderer;

	void Awake(){
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void SetPoints(List<Vector3> pointsToDraw){
		lineRenderer.SetVertexCount(pointsToDraw.Count);

		for (int i = 0; i < pointsToDraw.Count; i++){
			lineRenderer.SetPosition(i, pointsToDraw[i]);
		}
	}

	public void ClearPoints(){
		lineRenderer.SetVertexCount(0);
	}
}
