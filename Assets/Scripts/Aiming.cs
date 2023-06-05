using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private Dragging _draggingController;
    [SerializeField] private PuckBase _puckBaseController;
    [SerializeField] private Shooting _shootingController;
    
    private Aiming _aimingController;
    
    [HideInInspector] public bool IsAimingEnabled;
    [HideInInspector] public bool ClickedOn;
    
    [HideInInspector] public Vector3 MouseDownPoint;
    [HideInInspector] public Vector3 MouseUpPoint;
    
    public LineRenderer AimLineRenderer;
    public Transform AimTarget;
    
    private float _maxXOffsetLine = 3f;
    private float _maxZOffsetLine = 3f;
    
    public void Initialize(Aiming controller)
    {
        _aimingController = controller;
    }
    
    private void Start()
    {
        LineStartConfig();
        _puckBaseController.Initialize(_puckBaseController);
        _draggingController.Initialize(_draggingController);
        _shootingController.Initialize(_shootingController);
    }

    private void LineStartConfig()
    {
        AimLineRenderer.enabled = false;
        AimLineRenderer.startWidth = 0.3f;
        AimLineRenderer.endWidth = 0.3f;
    }
    
    public void StartAiming()
    {
        IsAimingEnabled = true;
        ClickedOn = true;
        _shootingController.IsPuckShot = false;
        _draggingController.IsDragging = false;
        MouseDownPoint = GetMouseWorldPosition();
        
        if(_puckBaseController.PuckSpringJoint != null)
            _puckBaseController.PuckSpringJoint.spring = 0f;
        
        AimLineRenderer.enabled = true;
        AimLineRenderer.SetPosition(0, transform.position);
        AimLineRenderer.SetPosition(1, transform.position);
        _puckBaseController.PreviousPuckVelocity = _puckBaseController.PuckRigidbody.velocity;
        IsAimingEnabled = true;
    }
    
    public void HandleAiming()
    {
        if (IsAimingEnabled)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            AimTarget.position = mousePos;

            Vector3 aimTargetPos = AimTarget.position;
            aimTargetPos.x = Mathf.Clamp(aimTargetPos.x,
                _puckBaseController.PuckInitialPosition.x - _maxXOffsetLine,
                _puckBaseController.PuckInitialPosition.x + _maxXOffsetLine);
            aimTargetPos.z = Mathf.Clamp(aimTargetPos.z,
                _puckBaseController.PuckInitialPosition.z - _maxZOffsetLine,
                _puckBaseController.PuckInitialPosition.z + _maxZOffsetLine);
            AimTarget.position = aimTargetPos;
            
            AimLineRenderer.SetPosition(0, transform.position);
            AimLineRenderer.SetPosition(1, transform.position - (AimTarget.position - transform.position)
                                           + new Vector3(0, AimTarget.position.y - transform.position.y, 0));
        }
    }
    
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.GetTouch(0).position;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(worldPosition, Vector3.zero);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                Vector3 hitPoint = hit.point;
                Vector3 direction = (hitPoint - worldPosition).normalized;
                float distance = Vector3.Distance(hitPoint, worldPosition);

                worldPosition = hitPoint + direction * (distance + 0.01f);
            }
        }

        return new Vector3(worldPosition.x, 0f, worldPosition.z);
    }
}