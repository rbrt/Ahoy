using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RealMoveMarker : MoveMarker {
	const float proxyBoatAlpha = .6f;
	const float firingIndicationSensitivity = 6; // 4 gives a reasonable visualization on a power scale of 0 - 4.5

	[SerializeField] protected ShotVisualizer shotVisualizer;
	[SerializeField] protected GameObject proxyBoat;

	Quaternion targetRotation;
	Vector3 playerShot;

	bool fadingBoat = false;

	public Quaternion TargetRotation {
		get {
			return targetRotation;
		}
	}

	public float FiringStrength {
		get {
			return playerShot.magnitude;
		}
	}

	public override void OnPlayerInput(){
		if (MoveMarkerManager.CurrentMarker != this){
			MoveMarkerManager.SetCurrentMarker(this);
		}
		else {
			PlayerController.Instance.Moving = true;
			MoveMarkerMenu.Instance.HideMenu();
			this.StartSafeCoroutine(SmoothFadeBoat(false, .25f));
		}
	}

	void Awake(){
		this.StartSafeCoroutine(AnimateMarker());
		playerShot = Vector3.zero;

		var proxyRenderer = proxyBoat.GetComponent<Renderer>();
		Material boatMaterial = new Material(proxyRenderer.material);
		proxyRenderer.material = boatMaterial;

		this.StartSafeCoroutine(SmoothFadeBoat(false, 0));
	}

	public override void IndicateMoveSet(){
		shotVisualizer.IndicateMoveSet();
	}

	public override void SetTargetRotation(Quaternion targetRotation){
		this.targetRotation = targetRotation;
	}

	public override void SetPlayerShot(Vector3 playerShot){
		this.playerShot = playerShot;
	}

	public override void OnSelectMoveMarker(){
		this.StartSafeCoroutine(SmoothFadeBoat(true, .25f));
	}

	public override void OnUnselectMoveMarker(){
		this.StartSafeCoroutine(SmoothFadeBoat(false, .25f));
	}

	public override void DestroyMarker(){
		Destroy(this.gameObject);
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
		List<Vector3> shotPoints = new List<Vector3>();
		shotPoints.Add(Vector3.zero);
		shotPoints.Add(Vector3.zero);
		shotVisualizer.SetPoints(shotPoints);
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

	IEnumerator SmoothFadeBoat(bool appear, float time){
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
