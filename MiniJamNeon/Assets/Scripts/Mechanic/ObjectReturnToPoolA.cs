
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturnToPoolA : MonoBehaviour
{
	public string sourceTag;

	void OnDisable() {
		ObjectPoolA.Instance.Return(sourceTag, gameObject);
	}
}