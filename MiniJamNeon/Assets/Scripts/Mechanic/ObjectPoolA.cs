using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPoolA : MonoBehaviour
{
	[System.Serializable]
	public class Pool {
		public string tag;
		public GameObject prefab;
		public int defaultCount;
	}

	public static ObjectPoolA Instance;

	[SerializeField] int expansionConstant;
	[SerializeField] List<Pool> pools = new List<Pool>();

	Dictionary<string, Queue<GameObject>> objectQueues = new Dictionary<string, Queue<GameObject>>();
	Dictionary<string, int> tagToPoolIndex = new Dictionary<string, int>();
	

	void Awake() {
		Instance = this;
		for (int i = 0; i < pools.Count; i++) {
			tagToPoolIndex[pools[i].tag] = i;
			objectQueues[pools[i].tag] = new Queue<GameObject>();
			Expand(pools[i], pools[i].defaultCount);
		}
	}

	void Expand(Pool pool, int count) {
		while (count --> 0) {
			GameObject obj = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
			obj.SetActive(false);
			obj.AddComponent(typeof(ObjectReturnToPoolA));
			obj.transform.parent = transform;

			ObjectReturnToPoolA returnScript = obj.GetComponent<ObjectReturnToPoolA>();
			returnScript.sourceTag = pool.tag;

			objectQueues[pool.tag].Enqueue(obj);

		}
	}

	public GameObject Instantiate(string tag) {
		return Instantiate(tag, Vector3.zero, Quaternion.identity);
	}

	public GameObject Instantiate(string tag, Vector3 position, Quaternion rotation) {
		Assert.IsTrue(objectQueues.ContainsKey(tag));

		if (objectQueues[tag].Count == 0)
			Expand(pools[tagToPoolIndex[tag]], expansionConstant);

		GameObject obj = objectQueues[tag].Dequeue();
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		obj.SetActive(true);

		return obj;
	}

	public void Return(string tag, GameObject obj) {
		Assert.IsTrue(objectQueues.ContainsKey(tag));

		objectQueues[tag].Enqueue(obj);
	}
}