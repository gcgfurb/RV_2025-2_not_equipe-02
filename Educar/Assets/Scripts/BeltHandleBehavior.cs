using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BeltHandleBehavior : MonoBehaviour
{
	private XRGrabInteractable grabInteractable;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private bool encaixado = false;

	private Collider[] objectColliders;
	private Collider[] handColliders;

	[Header("Configurações")]
	public Transform pontoEncaixe;
	public float distanciaEncaixe = 0.1f;
	public float velocidadeVolta = 2f;

	[Header("Feedback Visual")]
	public Renderer slotRenderer;
	public Color corNormal = Color.gray;
	public Color corProxima = Color.green;
	public Color corEncaixado = Color.blue;

	[Header("Colisões")]
	public Collider[] slotColliders; // arraste o(s) collider(s) do Belt Slot

	[Header("Sumir ao encaixar?")]
	public bool sumirQuandoEncaixar = true;  // 🔥 Toggle no inspetor

	void Start()
	{
		grabInteractable = GetComponent<XRGrabInteractable>();
		objectColliders = GetComponentsInChildren<Collider>(true);

		initialPosition = transform.position;
		initialRotation = transform.rotation;

		grabInteractable.selectEntered.AddListener(OnGrab);
		grabInteractable.selectExited.AddListener(OnRelease);

		if (slotRenderer != null)
			slotRenderer.material.color = corNormal;

		// ignora colisão com tudo exceto slot e mão
		IgnoreAllCollisionsExceptHandsAndSlot();
	}

	// =========================================
	// IGNORA COLISÕES COM TUDO MENOS SLOT E MÃO
	// =========================================
	void IgnoreAllCollisionsExceptHandsAndSlot()
	{
		Collider[] allColliders = FindObjectsOfType<Collider>();

		foreach (var objCol in objectColliders)
		{
			foreach (var otherCol in allColliders)
			{
				bool isSlot = System.Array.IndexOf(slotColliders, otherCol) != -1;

				if (isSlot)
					continue;

				Physics.IgnoreCollision(objCol, otherCol, true);
			}
		}
	}

	void OnGrab(SelectEnterEventArgs args)
	{
		handColliders = args.interactorObject.transform.GetComponentsInChildren<Collider>();

		Collider[] allColliders = FindObjectsOfType<Collider>();

		foreach (var objCol in objectColliders)
		{
			foreach (var otherCol in allColliders)
			{
				bool isHand = System.Array.IndexOf(handColliders, otherCol) != -1;
				bool isSlot = System.Array.IndexOf(slotColliders, otherCol) != -1;

				if (isHand || isSlot)
					continue;

				Physics.IgnoreCollision(objCol, otherCol, true);
			}
		}
	}

	void RestoreCollisions()
	{
		Collider[] allColliders = FindObjectsOfType<Collider>();

		foreach (var objCol in objectColliders)
		{
			foreach (var otherCol in allColliders)
			{
				Physics.IgnoreCollision(objCol, otherCol, false);
			}
		}

		IgnoreAllCollisionsExceptHandsAndSlot();
	}

	void OnRelease(SelectExitEventArgs args)
	{
		RestoreCollisions();

		if (encaixado) return;

		float distancia = Vector3.Distance(transform.position, pontoEncaixe.position);

		if (distancia <= distanciaEncaixe)
		{
			encaixado = true;

			// encaixa no ponto
			transform.position = pontoEncaixe.position;
			transform.rotation = pontoEncaixe.rotation;

			// muda cor do slot
			if (slotRenderer != null)
				slotRenderer.material.color = corEncaixado;

			// impede que seja pego novamente
			grabInteractable.enabled = false;

			// 🔥🔥🔥 FAZER O CINTO SUMIR
			if (sumirQuandoEncaixar)
				SumirAposEncaixe();

			return;
		}
		else
		{
			StartCoroutine(VoltarPosicao());
		}
	}

	// =========================================
	// FAZ O OBJETO SUMIR QUANDO ENCAIXADO
	// =========================================
	void SumirAposEncaixe()
	{
		// desativa render
		foreach (var r in GetComponentsInChildren<MeshRenderer>())
			r.enabled = false;

		// desativa colisão
		foreach (var c in GetComponentsInChildren<Collider>())
			c.enabled = false;

		// opcional: desativar completamente o objeto
		// gameObject.SetActive(false);
	}
	// =========================================

	System.Collections.IEnumerator VoltarPosicao()
	{
		float t = 0;
		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

		while (t < 1f)
		{
			t += Time.deltaTime * velocidadeVolta;
			transform.position = Vector3.Lerp(startPos, initialPosition, t);
			transform.rotation = Quaternion.Lerp(startRot, initialRotation, t);
			yield return null;
		}

		transform.position = initialPosition;
		transform.rotation = initialRotation;
	}

	void Update()
	{
		if (pontoEncaixe == null || encaixado) return;

		float distancia = Vector3.Distance(transform.position, pontoEncaixe.position);

		if (slotRenderer != null)
			slotRenderer.material.color = distancia <= distanciaEncaixe ? corProxima : corNormal;
	}
}
