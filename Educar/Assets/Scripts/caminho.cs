using UnityEngine;

public class caminho : MonoBehaviour
{
	public GameObject nextPoint;

	[Header("Animação de Flutuação")]
	public float floatStrength = 0.5f;
	public float floatSpeed = 2f;      

	private Vector3 startPos;

	private void Start()
	{
		startPos = transform.position;
	}

	private void Update()
	{
		transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatStrength;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.layer == LayerMask.NameToLayer("RCC_Vehicle"))
        {
            gameObject.SetActive(false);

		}

		//if (nextPoint != null)
		//{
		//    nextPoint.SetActive(true);
		//}
		//}
	}
}
