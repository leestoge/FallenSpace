using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public Vector3 m_target;
    public GameObject collisionExplosion;
    public float speed;

    public void setTarget(Vector3 target)
    {
        m_target = target;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (m_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_target, step);

            if (transform.position == m_target)
            {
                explode();
                return;
            }          
        }
    }

    void explode()
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }
}
