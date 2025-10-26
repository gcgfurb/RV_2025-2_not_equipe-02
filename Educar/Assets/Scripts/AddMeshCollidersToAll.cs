using UnityEngine;

public class AddMeshCollidersToAll : MonoBehaviour
{
    void Start()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            if (mesh.GetComponent<MeshCollider>() == null)
            {
                mesh.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}
