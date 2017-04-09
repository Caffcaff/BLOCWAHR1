using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyTimer : MonoBehaviour {

public float timer =3;

	void Start () {
		Destroy(gameObject, timer);
	}
}
