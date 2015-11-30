using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RealRotationMarker : RotationMarker {

	[SerializeField] protected Image rotationImage;
	[SerializeField] protected Image directionImage;
	[SerializeField] protected Image baseDirectionImage;

	RectTransform directionRect;

	float highlightTime = .15f;

	public override void SetRotationImageFill(float fill){
		rotationImage.fillAmount = fill;
	}

	public override float SetDirectionImageTarget(Vector3 target){
		var adjustedPosition = CameraManager.WorldToGameCameraPoint(directionRect.position);
		target = new Vector3(target.y, 0, target.x);
		adjustedPosition = new Vector3(adjustedPosition.y, 0, adjustedPosition.x);
		Vector3 direction = adjustedPosition - target;

		directionRect.localRotation = Quaternion.LookRotation(direction);
		var temp = directionRect.localRotation.eulerAngles;
		temp.z = (temp.y + 270) % 360f;
		temp.y = 0;
		directionRect.localRotation = Quaternion.Euler(temp);
		SetRotationImageFill((360 - temp.z) / 360f);

		return temp.z;
	}

	public override void IndicateRotationMoveSet(){
		this.StartSafeCoroutine(HighlightIndicator());
	}

	void Awake(){
		directionRect = directionImage.GetComponent<RectTransform>();
	}

	IEnumerator HighlightIndicator(){
		var color = rotationImage.color;
		var highlightColor = Color.white;
		highlightColor.a = color.a;
		for(int count = 0; count < 2; count++){
			for (float i = 0; i <= 1; i += Time.deltaTime / highlightTime){
				rotationImage.color = Color.Lerp(color, highlightColor, i);
				yield return null;
			}
			for (float i = 0; i <= 1; i += Time.deltaTime / highlightTime){
				rotationImage.color = Color.Lerp(highlightColor, color, i);
				yield return null;
			}
		}

		rotationImage.color = color;
	}

}
