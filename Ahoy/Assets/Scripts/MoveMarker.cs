using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MoveMarker : AcceptsInput {

	Quaternion targetRotation;

	static MoveMarker tappedMarker;

	public Quaternion TargetRotation {
		get {
			return targetRotation;
		}
	}

	public override void OnPlayerInput(){
		MoveMarkerMenu.Instance.ShowMenu(transform.position);
		tappedMarker = this;
	}

	void Awake(){
		this.StartCoroutine(AnimateMarker());
	}

	public static void SetTargetRotation(Quaternion target){
		if (tappedMarker != null){
			tappedMarker.targetRotation = target;
		}
	}

	public static void ClearTargetMarker(){
		tappedMarker = null;
	}

	IEnumerator AnimateMarker(){
		var renderer = GetComponent<Renderer>();
		var color = renderer.sharedMaterial.GetColor("_Color");
		Color highlightColor = Color.green;

		while (true && this != null){
			for (float i = 0; i <= 1; i += Time.deltaTime / .75f){
				renderer.sharedMaterial.SetColor("_Color", Color.Lerp(color, highlightColor, i * i));
				yield return null;
			}
			for (float i = 0; i <= 1; i += Time.deltaTime / .75f){
				renderer.sharedMaterial.SetColor("_Color", Color.Lerp(highlightColor, color, i * i));
				yield return null;
			}
			renderer.sharedMaterial.SetColor("_Color", color);
			yield return null;
		}
	}
}
