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

	float targetFirstAngleSlope = 15;

	public static ShipPathVisualizer Instance {
		get {
			return instance;
		}	
	}

	public void VisualizePathForPoints(List<Vector3> points){
		if (lineRenderers.Length != points.Count){
			InitializeLineRenderers(points.Count);
		}

		for (int i = 1; i < points.Count; i++){
			//VisualizeCurve(points[i-1], points[i], i-1, initializationVector);
		}

		curveDegree = points.Count - 1;
		VisualizeBSpline(points);
	}

	public int curveDegree = 1; // Degree of the curve
	private Vector3[] cachedControlPoints; // cached control points
	private int[] nV; // Node vector

	void Start(){
		cachedControlPoints = new Vector3[0];
		List<Vector3> points = new List<Vector3>();
		CacheControlPoints(ref points);
		nV = new int[5];
		createNodeVector();
	}

	void VisualizeBSpline(List<Vector3> controlPoints){
		if(controlPoints.Count <= 0){
			return;	
		}

		cachedControlPoints = new Vector3[controlPoints.Count];

		// Cached the control points
		CacheControlPoints(ref controlPoints);

		if(cachedControlPoints.Length <= 0){
			return;
		}

		// Initialize node vector.
		nV = new int[cachedControlPoints.Length + curveDegree + 1];
		createNodeVector();

		Vector3 start = cachedControlPoints[0];
		Vector3 end = Vector3.zero;

		float increment = .01f;

		lineRenderers[0].SetVertexCount((int)(1 / increment) + 2);
		int index = 0;
		for(float i = 0.0f; i < nV[curveDegree + cachedControlPoints.Length]; i += increment){
			for(int j = 0; j < cachedControlPoints.Length; j++){
				if(i >= j){
					end = deBoor(curveDegree, j, i);
				}
			}
			lineRenderers[0].SetPosition(index, start);
			index++;
			start = end;
		}
		lineRenderers[0].SetPosition(index, start);
	}

	// Recursive deBoor algorithm.
	public Vector3 deBoor(int r, int i, float u){
		if(r == 0){
			return cachedControlPoints[i];
		}
		else{
			float pre = (u - nV[i + r]) / (nV[i + curveDegree + 1] - nV[i + r]);
			return ((deBoor(r - 1, i, u) * (1 - pre)) + (deBoor(r - 1, i + 1, u) * (pre)));
		}
	}

	public void createNodeVector(){
		int knoten = 0;
		// n+m+1 = nr of nodes
		for(int i = 0; i < (curveDegree + cachedControlPoints.Length + 1); i++){
			if(i > curveDegree){
				if(i <= cachedControlPoints.Length){
					nV[i] = ++knoten;
				}
				else{
					nV[i] = knoten;
				}
			}
			else {
				nV[i] = knoten;
			}
		}
	}

	private void CacheControlPoints(ref List<Vector3> controlPoints){
		for(int i = 0; i < controlPoints.Count; i++){
			cachedControlPoints[i] = controlPoints[i];
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

		p2 = midpoint1 + Vector3.Cross(midpoint1 - p1, Vector3.up);	

		p3 = midpoint2 + Vector3.Cross(p4 - midpoint2, Vector3.up);
		p2.y = p1.y;
		p3.y = p1.y;

		Debug.DrawLine(midpoint1, p2, Color.gray);
		Debug.DrawLine(midpoint, midpoint + Vector3.up, Color.gray);

		var firstPoint = Mathx.InterpolatedPointOnBezierCurve(p1, p2, p3, p4, .3f);
		var comparison = firstPoint - p1;
		comparison.y = firstPoint.y;

		float firstAngleSteepness = Vector3.Angle(initializationVector, comparison);
		Vector3 p2Offset = (p2 - midpoint1) * .05f;
		p2Offset.y = initializationVector.y;

		float temp = firstAngleSteepness;
		int iterations = 0;
		bool itHappened = false;
		float boost = 1;
		for (float i = 0; firstAngleSteepness > targetFirstAngleSlope; i += .3f){
			itHappened = true;
			iterations++;
			p2 += p2Offset;
			p2.y = p1.y;

			firstPoint = Mathx.InterpolatedPointOnBezierCurve(p1, p2, p3, p4, .1f);
			comparison = firstPoint - p1;
			comparison.y = p1.y;

			firstAngleSteepness = Vector3.Angle(initializationVector, comparison);

			if (iterations >= 50){
				break;
			}

			boost = i;
		}

		initializationVector *= 100;
		initializationVector.y = p1.y;
		comparison *= 100;
		comparison.y = p1.y;

		Debug.DrawLine(p1, (p1 + initializationVector), Color.red);
		Debug.DrawLine(p1, (p1 + comparison), Color.yellow);

		Debug.DrawLine(midpoint1, p2, Color.cyan);
		Debug.DrawLine(midpoint2, p3, Color.cyan);

		if (itHappened){
			Debug.Log("Final Steepness: " + firstAngleSteepness + " initial angle: " + temp);
		}
		else{
			Debug.Log("Angle: " + firstAngleSteepness);
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
