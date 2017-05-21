using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		Camera.main.aspect = 1080f / 1920f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
