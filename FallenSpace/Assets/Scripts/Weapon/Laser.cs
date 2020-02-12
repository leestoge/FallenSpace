using UnityEngine;

public class Laser : MonoBehaviour
{
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;

    RaycastHit hit;
    float range = 1000.0f;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time > m_shootRateTimeStamp)
            {
                shootRay();
                m_shootRateTimeStamp = Time.time + shootRate;
            }
        }
    }

    public void shootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            GameObject laser = Instantiate(m_shotPrefab, transform.position, transform.rotation);
            laser.GetComponent<LaserBehaviour>().setTarget(hit.point);
            Destroy(laser, 2f);
        }
    }
}
