using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ShotVisualizer : MonoBehaviour {
	LineRenderer lineRenderer;
	bool flashing = false;

	void Awake(){
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		Material ownMaterial = new Material(lineRenderer.sharedMaterial);
		lineRenderer.material = ownMaterial;
	}

	public void IndicateMoveSet(){
		if (flashing){
			return;
		}
		this.StartSafeCoroutine(FlashColor());
	}

	public void SetPoints(List<Vector3> pointsToDraw){
		lineRenderer.SetVertexCount(pointsToDraw.Count);

		for (int i = 0; i < pointsToDraw.Count; i++){
			lineRenderer.SetPosition(i, pointsToDraw[i]);
		}

		lineRenderer.enabled = true;
	}

	public void ClearPoints(){
		lineRenderer.SetVertexCount(0);
		lineRenderer.enabled = false;
	}

	IEnumerator FlashColor(){
		var color = lineRenderer.sharedMaterial.GetColor("_Color");
		Color highlightColor = Color.white;
		flashing = true;
		for (float i = 0; i <= 1; i += Time.deltaTime / .2f){
			lineRenderer.sharedMaterial.SetColor("_Color", Color.Lerp(color, highlightColor, i));
			yield return null;
		}
		for (float i = 0; i <= 1; i += Time.deltaTime / .2f){
			lineRenderer.sharedMaterial.SetColor("_Color", Color.Lerp(highlightColor, color, i));
			yield return null;
		}
		lineRenderer.sharedMaterial.SetColor("_Color", color);

		flashing = false;
	}
}
