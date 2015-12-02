using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class OceanInput : AcceptsInput {

	public override void OnPlayerInput(){
		PlayerController.Instance.Scrolling = true;
	}

}
