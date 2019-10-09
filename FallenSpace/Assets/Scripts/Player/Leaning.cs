using Assets.Imports.Standard_Assets.Characters.FirstPersonCharacter.Scripts;
using UnityEngine;

public class Leaning : MonoBehaviour
{
    public KeyCode leanLeftKey = KeyCode.Q;
    public KeyCode leanRightKey = KeyCode.E;

    [SerializeField] private float m_Amount = 10f;
    [SerializeField] private float m_LeaningSpeed = 2f;

    private FirstPersonController m_FPSController;
    public Transform m_CameraTransform;
    private Vector3 m_InitPos;
    private Quaternion m_InitRot;

    private bool m_IsLeaningLeft = false;
    private bool m_IsLeaningRight = false;
    
    void Awake()
    {
        m_FPSController = GetComponent<FirstPersonController>();

        m_InitPos = m_CameraTransform.localPosition;
        m_InitRot = m_CameraTransform.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(leanLeftKey))
        {
            m_IsLeaningLeft = true;
        }
        else
        {
            m_IsLeaningLeft = false;
        }

        if (Input.GetKey(leanRightKey))
        {
            m_IsLeaningRight = true;
        }
        else
        {
            m_IsLeaningRight = false;
        }

        CheckCanLeanLeft();
        CheckCanLeanRight();
        CheckLeaning();
    }

    void CheckCanLeanLeft()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.TransformDirection(Vector3.left * 0.5f), out hit, 0.5f))
        {
            m_IsLeaningLeft = false;
        }
    }

    void CheckCanLeanRight()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.TransformDirection(Vector3.right * 0.5f), out hit, 0.5f))
        {
            m_IsLeaningRight = false;
        }
    }

    void CheckLeaning()
    {
        if (m_IsLeaningLeft)
        {
            m_FPSController.SetRotateZ(m_Amount);

            Vector3 newPos = new Vector3(m_InitPos.x - 0.5f, m_InitPos.y, m_InitPos.z);
            m_CameraTransform.localPosition = Vector3.Lerp(m_CameraTransform.localPosition, newPos, Time.deltaTime * m_LeaningSpeed);
        }
        else if (m_IsLeaningRight)
        {
            m_FPSController.SetRotateZ(-m_Amount);

            Vector3 newPos = new Vector3(m_InitPos.x + 0.5f, m_InitPos.y, m_InitPos.z);
            m_CameraTransform.localPosition = Vector3.Lerp(m_CameraTransform.localPosition, newPos, Time.deltaTime * m_LeaningSpeed);
        }
        else
        {
            m_FPSController.SetRotateZ(m_InitRot.eulerAngles.z);

            m_CameraTransform.localPosition = Vector3.Lerp(m_CameraTransform.localPosition, m_InitPos, Time.deltaTime * m_LeaningSpeed);
        }
    }
}
