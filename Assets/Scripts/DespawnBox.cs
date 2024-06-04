using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBox : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision detected between " + gameObject.name + " and " + collision.gameObject.name);

		if (collision.gameObject.CompareTag("BlueFruit") || collision.gameObject.CompareTag("RedFruit"))
		{
			Destroy(collision.gameObject);
			GameManager.instance.DecrementScore();
		}
	}
}
