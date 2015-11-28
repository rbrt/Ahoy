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

	const float proxyBoatAlpha = .6f;

	[SerializeField] protected ShotVisualizer ShotVisualizer;
	[SerializeField] protected GameObject proxyBoat;

	Quaternion targetRotation;
	Vector3 playerShot;

	bool fadingBoat = false;

	static MoveMarker tappedMarker;

	public Quaternion TargetRotation {
		get {
			return targetRotation;
		}
	}

	public override void OnPlayerInput(){
		MoveMarkerMenu.Instance.ShowMenu(transform.position);
		tappedMarker = this;
		this.StartCoroutine(FadeBoat(true, .25f));
	}

	void Awake(){
		this.StartCoroutine(AnimateMarker());
		playerShot = Vector3.zero;
		this.StartCoroutine(FadeBoat(false, 0));
	}

	public static void SetTargetRotation(Quaternion target){
		if (tappedMarker != null){
			tappedMarker.targetRotation = target;
		}
	}

	public static void SetTargetFiringStrength(Vector3 shot){
		if (tappedMarker != null){
			tappedMarker.playerShot = shot;
		}
	}

	public static void ClearTargetMarker(){
		tappedMarker = null;
	}

	public void UpdatePlayerShot(Vector3 playerShot){
		
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

	IEnumerator FadeBoat(bool appear, float time){
		if (fadingBoat){
			yield break;
		}

		fadingBoat = true;
		var renderers = proxyBoat.GetComponentsInChildren<Renderer>().ToList();
		for (float i = 0; i <= 1; i += Time.deltaTime / time){
			renderers.ForEach(x => {
				Color color = x.sharedMaterial.color;
				color.a = (appear ? i : (1 - i)) * proxyBoatAlpha;
				x.sharedMaterial.color = color;
			});
			yield return null;
		}

		renderers.ForEach(x => {
			Color color = x.sharedMaterial.color;
			color.a = (appear ? 1 : 0) * proxyBoatAlpha;
			x.sharedMaterial.color = color;
		});

		fadingBoat = false;
	}
}
