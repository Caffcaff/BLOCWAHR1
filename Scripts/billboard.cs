using UnityEngine;
using System.Collections;

public class billboard: MonoBehaviour
{
	public Camera m_camera;

	void Start (){
	if (m_camera == null){
			m_camera = Camera.main;
		}
	}

	void Update()
	{
		transform.LookAt(transform.position + m_camera.transform.rotation * Vector3.forward,
		m_camera.transform.rotation * Vector3.up);
	}
}