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

	[SerializeField] protected ShotVisualizer shotVisualizer;
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

	public float FiringStrength {
		get {
			return playerShot.magnitude / 1f;
		}
	}

	public override void OnPlayerInput(){
		if (tappedMarker != null && tappedMarker != this){
			tappedMarker.UnselectMoveMarker();
		}

		this.StartSafeCoroutine(MoveMarkerMenu.Instance.ShowMenu(transform.position));
		tappedMarker = this;
		this.StartSafeCoroutine(FadeBoat(true, .25f));
	}

	void Awake(){
		this.StartSafeCoroutine(AnimateMarker());
		playerShot = Vector3.zero;

		var proxyRenderer = proxyBoat.GetComponent<Renderer>();
		Material boatMaterial = new Material(proxyRenderer.sharedMaterial);
		proxyRenderer.material = boatMaterial;

		this.StartSafeCoroutine(FadeBoat(false, 0));
	}

	public static void SetTargetRotation(Quaternion target){
		if (tappedMarker != null){
			tappedMarker.targetRotation = target;
		}
	}

	public static void SetTargetFiringStrength(Vector3 shot){
		if (tappedMarker != null){
			Vector3 scaledForScreen = shot;
			scaledForScreen.x /= Screen.width / 2f;
			scaledForScreen.y /= Screen.height / 2f;

			tappedMarker.playerShot = scaledForScreen;
		}
	}

	public static void ClearTargetMarker(){
		tappedMarker = null;
	}

	public static void ClearFiringVisualizer(){
		tappedMarker.playerShot = Vector3.zero;
	}

	public static void IndicateMoveSet(){
		if (tappedMarker != null){
			tappedMarker.shotVisualizer.IndicateMoveSet();
		}
	}

	public void UnselectMoveMarker(){
		this.StartSafeCoroutine(FadeBoat(false, .25f));
	}

	Vector3 GetShotVector(){
		return proxyBoat.transform.position + proxyBoat.transform.right * FiringStrength * PlayerBoat.Instance.BoatShotStrength;
	}

	void DrawFiringStrength(){
		List<Vector3> shotPoints = new List<Vector3>();
		shotPoints.Add(proxyBoat.transform.position);
		shotPoints.Add(GetShotVector());

		Vector3 adjusted;
		for (int i = 0; i < shotPoints.Count; i++){
			adjusted = shotPoints[i];
			adjusted.y = 1;
			shotPoints[i] = adjusted;
		}

		shotVisualizer.SetPoints(shotPoints);
	}

	void ClearFiringStrength(){
		
	}

	void Update(){
		DrawFiringStrength();
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
