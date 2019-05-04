using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EffectCamera : MonoBehaviour {

	public Material transitionMaterial;

	private float cutoff = 1f, targetCutoff = 1f;
	private float prevCutoff = 1f;
	private float cutoffPos = 0f;
	private float transitionTime = 0.5f;

	private PostProcessingBehaviour filters;
	private float chromaAmount = 0f;
	private float chromaSpeed = 0.1f;

	private float shakeAmount = 0f, shakeTime = 0f;

	private Vector3 originalPos;

	void Start() {
		filters = GetComponent<PostProcessingBehaviour>();
		originalPos = transform.position;
		Invoke ("StartFade", 0.5f);
	}

	void Update() {
		cutoffPos += Time.fixedDeltaTime / transitionTime;
		cutoffPos = (cutoffPos > 1f) ? 1f : cutoffPos;
		cutoff = Mathf.Lerp (prevCutoff, targetCutoff, cutoffPos);
		transitionMaterial.SetFloat ("_Cutoff", cutoff);

		// chromatic aberration update
		if (filters) {
			chromaAmount = Mathf.MoveTowards (chromaAmount, 0, Time.deltaTime * chromaSpeed);
			ChromaticAberrationModel.Settings g = filters.profile.chromaticAberration.settings;
			g.intensity = chromaAmount;
			filters.profile.chromaticAberration.settings = g;
		}

		if (shakeTime > 0f) {
			shakeTime -= Time.deltaTime;
			transform.position = originalPos + new Vector3 (Random.Range (-shakeAmount, shakeAmount), Random.Range (-shakeAmount, shakeAmount), 0);
		} else {
			transform.position = originalPos;
		}
	}

	void StartFade() {
		Fade (false, 0.5f);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		if (transitionMaterial) {
			Graphics.Blit (src, dst, transitionMaterial);
		}
	}

	public void Fade(bool show, float delay) {
		targetCutoff = show ? 1.1f : -0.1f;
		prevCutoff = show ? -0.1f : 1.1f;
		cutoffPos = 0f;
		transitionTime = delay;

		// AudioManager.Instance.PlayEffectAt (12, Vector3.zero, 0.2f);
	}

	public void Chromate(float amount, float speed) {
		chromaAmount = amount;
		chromaSpeed = speed;
	}

	public void Shake(float amount, float time) {
		shakeAmount = amount;
		shakeTime = time;
	}

	public void BaseEffect(float mod = 1f) {
		Shake (0.04f * mod, 0.075f * mod);
		Chromate (0.25f * mod, 0.1f * mod);
	}
}
