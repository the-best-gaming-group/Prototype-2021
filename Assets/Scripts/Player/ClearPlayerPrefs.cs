using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerPres : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod]
	static void ClearPrefsOnLoad()
	{
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
#endif
	}
}
