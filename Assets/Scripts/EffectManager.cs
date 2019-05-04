using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	public GameObject[] effects;

	// ==================

	private static EffectManager instance = null;

	public static EffectManager Instance {
		get { return instance; }
	}

	// ==================

	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
	}

	public GameObject AddEffect(int effect, Vector3 position) {
		GameObject e = Instantiate (effects[effect], transform);
		e.transform.position = position;
		return e;
	}

	public GameObject AddEffectToParent(int effect, Vector3 position, Transform parent) {
		GameObject e = Instantiate (effects[effect], parent);
		e.transform.position = position;
		return e;
	}
}
