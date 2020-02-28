using UnityEngine;

public class DestroyAfter : MonoBehaviour 
{	
    public float destroyAfter = 15.0f;

    void Awake() 
	{
        Destroy(gameObject, destroyAfter);
    }
}
