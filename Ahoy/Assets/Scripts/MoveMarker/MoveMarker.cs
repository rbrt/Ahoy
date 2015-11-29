using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public abstract class MoveMarker : AcceptsInput {

	public abstract void IndicateFiringMoveSet();

	public abstract void IndicateTurningMoveSet();

	public abstract void SetTargetRotation(Vector3 initialPoint, Vector3 currentPoint);

	public abstract void SetPlayerShot(Vector3 playerShot);

	public abstract void OnSelectMoveMarker();

	public abstract void OnUnselectMoveMarker();

	public abstract void DestroyMarker();

}
