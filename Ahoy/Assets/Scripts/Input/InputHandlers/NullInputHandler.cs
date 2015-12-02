using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class NullInputHandler : InputHandler {
	public override float ActionWaitTime {
		get {
			return 0;
		}
	}

	public override System.Func<bool> InputAction {
		get {
			return () => true;
		}
	}

	public override bool InterruptedBySendMessage{
		get {
			return false;
		}
	}

	public override void HandleDragInput(){}

	public override void HandleInputUp(){}

	public override void HandleInputDown(){}

	public override void DrawInput(){}

	public override void OnSetHandler(){}

	public override void OnUnSetHandler(){}

	public override bool ShouldInvokeInputAction(float testSeconds){
		return false;
	}

	public override void PassiveAction(){}
}
