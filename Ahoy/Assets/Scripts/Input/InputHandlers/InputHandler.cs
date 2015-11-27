using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public abstract class InputHandler : MonoBehaviour {

	public abstract float ActionWaitTime{
		get;
	}

	public abstract System.Func<bool> InputAction {
		get;
	}

	public abstract bool InterruptedBySendMessage{
		get;
	}

	public abstract void OnSetHandler();

	public abstract void OnUnSetHandler();

	public abstract void HandleDragInput();

	public abstract void HandleInputUp();

	public abstract void HandleInputDown();

	public abstract void DrawInput();

	public abstract bool ShouldInvokeInputAction(float testSeconds);

}
