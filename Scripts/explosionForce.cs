using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class explosionForce : MonoBehaviour
{
	public float Explosionradius = 5.0F;
	public float Explosionpower = 10.0F;
	public float destroyTimer = 3;

	void Start()
	{
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, Explosionradius);
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null)
				rb.AddExplosionForce(Explosionpower, explosionPos, Explosionradius, 3.0F);
		}
		Destroy(gameObject, destroyTimer);
	}
}