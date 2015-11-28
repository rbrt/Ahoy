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

	public abstract void IndicateMoveSet();

	public abstract void SetTargetRotation(Quaternion targetRotation);

	public abstract void SetPlayerShot(Vector3 playerShot);

}
