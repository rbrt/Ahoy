using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public abstract class RotationMarker : MonoBehaviour {

	public abstract void SetRotationImageFill(float fill);

	public abstract float SetDirectionImageTarget(Vector3 target);

	public abstract void IndicateRotationMoveSet();

}
