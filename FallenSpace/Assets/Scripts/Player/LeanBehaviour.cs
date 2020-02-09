using UnityEngine;

public class LeanBehaviour : MonoBehaviour
{

    public Transform _Pivot;
    public KeyCode leanLeftKey = KeyCode.Q;
    public KeyCode leanRightKey = KeyCode.E;

    public float speed = 100f;
    public float maxAngle = 20f;

    float curAngle = 0f;
    private Vector3 _InitPos;

    private bool IsLeaningLeft = false;
    private bool IsLeaningRight = false;

    // Use this for initialization
    void Awake()
    {
        if (_Pivot == null && transform.parent != null)
        {
            _Pivot = transform.parent;
        }

        _InitPos = _Pivot.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(leanLeftKey))
        {
            IsLeaningLeft = true;
        }
        else
        {
            IsLeaningLeft = false;
        }

        if (Input.GetKey(leanRightKey))
        {
            IsLeaningRight = true;
        }
        else
        {
            IsLeaningRight = false;
        }

        CheckCanLeanLeft();
        CheckCanLeanRight();
        CheckLeaning();       
    }

    void CheckCanLeanLeft()
    {
        RaycastHit hit;

        if (Physics.Raycast(_Pivot.position, _Pivot.TransformDirection(Vector3.left * 0.5f), out hit, 0.5f))
        {
            IsLeaningLeft = false;
        }
    }

    void CheckCanLeanRight()
    {
        RaycastHit hit;

        if (Physics.Raycast(_Pivot.position, _Pivot.TransformDirection(Vector3.right * 0.5f), out hit, 0.5f))
        {
            IsLeaningRight = false;
        }
    }

    void CheckLeaning()
    {
        if (IsLeaningLeft) // lean left
        {
            curAngle = Mathf.LerpAngle(curAngle, maxAngle, Time.deltaTime * speed);

            Vector3 newPos = new Vector3(_InitPos.x - 0.5f, _InitPos.y, _InitPos.z);
            _Pivot.localPosition = Vector3.LerpUnclamped(_Pivot.localPosition, newPos, Time.deltaTime * speed);
        }
        else if (IsLeaningRight) // lean right
        {
            curAngle = Mathf.LerpAngle(curAngle, -maxAngle, Time.deltaTime * speed);

            Vector3 newPos = new Vector3(_InitPos.x + 0.5f, _InitPos.y, _InitPos.z);
            _Pivot.localPosition = Vector3.LerpUnclamped(_Pivot.localPosition, newPos, Time.deltaTime * speed);
        }
        else // reset lean
        {
            curAngle = Mathf.LerpAngle(curAngle, 0f, Time.deltaTime * speed);

            _Pivot.localPosition = Vector3.LerpUnclamped(_Pivot.localPosition, _InitPos, Time.deltaTime * speed);
        }

        _Pivot.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
    }
}
