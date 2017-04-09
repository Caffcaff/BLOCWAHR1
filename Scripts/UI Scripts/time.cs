using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class time : MonoBehaviour {

	DateTime now = DateTime.Now;
	public Text timeNow;
	public float rate =1;

	void Start (){
	timeNow = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rate < 0) {
			DateTime now = DateTime.Now;
			timeNow.text = now.ToString ("HH:MM:ss");
			rate = 1;
			}
		rate -= Time.deltaTime;
		}
	}
