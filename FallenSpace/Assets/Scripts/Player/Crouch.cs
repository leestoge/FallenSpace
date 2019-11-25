using Assets.Imports.Standard_Assets.Characters.FirstPersonCharacter.Scripts;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private CharacterController m_CharacterController;
    public FirstPersonController m_FPSController;
    private bool m_Crouch = false;
    private float m_originalHeight;
    [SerializeField] private float m_CrouchHeight = 0.5f;
    public KeyCode crouchKey = KeyCode.C;

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_originalHeight = m_CharacterController.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            m_Crouch = !m_Crouch;

            CheckCrouch();
        }
    }

    void CheckCrouch()
    {
        if (m_Crouch == true)
        {
            m_CharacterController.height = m_CrouchHeight;
            m_FPSController.m_WalkSpeed = m_FPSController.m_WalkSpeed / 2;
            m_CharacterController.center.Set( m_CharacterController.center.x,  m_CharacterController.center.y / 2,  m_CharacterController.center.z);
        }
        else
        {
            m_CharacterController.height = m_originalHeight;
            m_FPSController.m_WalkSpeed = m_FPSController.m_WalkSpeed * 2;
            m_CharacterController.center.Set(m_CharacterController.center.x, m_CharacterController.center.y * 2, m_CharacterController.center.z);
        }
    }
}
