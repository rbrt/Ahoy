using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class AcceptsInput : MonoBehaviour {

	public virtual void OnPlayerInput(){
		throw new System.NotImplementedException();
	}

}
