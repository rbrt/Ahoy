using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ShipPathVisualizer : MonoBehaviour {

	[SerializeField] protected LineRenderer lineRenderer;

	static ShipPathVisualizer instance;

	LineRenderer[] lineRenderers;

	int curvePointDensity = 50;
	Vector3 lastDirection = Vector3.zero;

	float targetFirstAngleSlope = 50;

	public static ShipPathVisualizer Instance {
		get {
			return instance;
		}	
	}

	public void VisualizePathForPoints(List<Vector3> points){
		if (lineRenderers.Length != points.Count){
			InitializeLineRenderers(points.Count);
		}

		Vector3 initializationVector = (points[0] + PlayerBoat.Instance.transform.forward) - points[0];
		initializationVector.y = points[0].y;

		for (int i = 1; i < points.Count; i++){
			VisualizeCurve(points[i-1], points[i], i-1, initializationVector);
		}
	}

	Vector3 midpoint = Vector3.zero;
	Vector3 midpoint1 = Vector3.zero;
	Vector3 midpoint2 = Vector3.zero;
	Vector3 p2 = Vector3.zero;
	Vector3 p3 = Vector3.zero;

	void VisualizeCurve(Vector3 p1, Vector3 p4, int rendererIndex, Vector3 initializationVector){
		midpoint = Vector3.Lerp(p1, p4, .5f);
		midpoint1 = Vector3.Lerp(p1, p4, .15f);
		midpoint2 = Vector3.Lerp(p1, p4, .85f);

		if (rendererIndex == 0){
			p2 = midpoint1 + Vector3.Cross(midpoint1 - p1, Vector3.up);	
		}

		p3 = midpoint2 + Vector3.Cross(p4 - midpoint2, Vector3.up);
		p2.y = p1.y;
		p3.y = p1.y;

		Debug.DrawLine(p1, p4, Color.gray);
		Debug.DrawLine(midpoint, midpoint + Vector3.up, Color.gray);
		Debug.DrawLine(p2, midpoint1, Color.cyan);
		Debug.DrawLine(p3, midpoint2, Color.cyan);

		float firstAngleSteepness = Vector3.Angle(initializationVector - p1, p2 - p1);
		Debug.Log(initializationVector + " " + p1 + " " + p2);
		Vector3 p2Offset = initializationVector * 20;
		p2Offset.y = initializationVector.y;


		float temp = firstAngleSteepness;
		int iterations = 0;
		bool itHappened = false;
		float boost = 1;
		for (float i = 1 ; firstAngleSteepness < targetFirstAngleSlope; i += .01f){
			itHappened = true;
			iterations++;
			p2 = midpoint1 + Vector3.Cross(midpoint1 - p1, Vector3.up);
			p2 += p2Offset * i;
			if (rendererIndex == 0){
				firstAngleSteepness = Vector3.Angle(initializationVector - p1, p2 - p1);
			}
			else {
				firstAngleSteepness = Vector3.Angle(initializationVector - p1, p2 - p1);
				//firstAngleSteepness = Vector3.Angle(midpoint1 - p1, p2 - p1);	
			}
			Debug.DrawLine(p1, midpoint1, Color.red);
			Debug.DrawLine(p1, p2, Color.green);

			if (iterations >= 10){
				firstAngleSteepness = targetFirstAngleSlope;
			}

			boost = i;
		}
		//Debug.Log("Final Steepness: " + firstAngleSteepness + " initial angle: " + temp);
		if (itHappened){
			Debug.DrawLine(midpoint1 + Vector3.Cross(midpoint1 - p1, Vector3.up), 20 * (p2 - (midpoint1 + Vector3.Cross(midpoint1 - p1, Vector3.up))));
		}

		p3 += (p3 - midpoint2).normalized * boost * 5;
//		float secondAngleSteepness = Vector3.Angle(midpoint2 - p4, p3 - p4);
//		for (float i = 1 ; firstAngleSteepness < 10; i += .1f){
//			p3 = Vector3.Cross(p4 - midpoint2, Vector3.up);
//			secondAngleSteepness = Vector3.Angle(midpoint2 - p4, p3 - p4);
//		}

		lineRenderers[rendererIndex].SetVertexCount(curvePointDensity);
		for (int i = 0; i < curvePointDensity; i++){
			lineRenderers[rendererIndex].SetPosition(i, Mathx.InterpolatedPointOnBezierCurve(p1, p2, p3, p4, i/(float)curvePointDensity));
		}

		lastDirection = Mathx.InterpolatedPointOnBezierCurve(p1, p2, p3, p4, (float)curvePointDensity * .99f) - 
						Mathx.InterpolatedPointOnBezierCurve(p1, p2, p3, p4, (float)curvePointDensity * .99f);
	}

	void Awake(){
		if (instance == null){
			instance = this;
			lineRenderers = new LineRenderer[0];
			InitializeLineRenderers(1);
		}
		else{
			Destroy(this.gameObject);
			Debug.LogError("Destroyed duplicate instance of ShipPathVisualizer");
		}
	}

	void InitializeLineRenderers(int count){
		for (int i = 0; i < lineRenderers.Length; i++){
			Destroy(lineRenderers[i].gameObject);
		}

		lineRenderers = new LineRenderer[count];

		for (int i = 0; i < count; i++){
			var newRenderer = GameObject.Instantiate(lineRenderer.gameObject).GetComponent<LineRenderer>();
			newRenderer.SetVertexCount(0);
			lineRenderers[i] = newRenderer;
		}

	}

}
