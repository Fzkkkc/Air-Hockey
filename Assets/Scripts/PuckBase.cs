using UnityEngine;

public class PuckBase : MonoBehaviour
{
    [HideInInspector] public Rigidbody PuckRigidbody;
    [HideInInspector] public SpringJoint PuckSpringJoint;
    [HideInInspector] public Vector3 PreviousPuckVelocity;
    [HideInInspector] public Vector3 PuckInitialPosition;
    [HideInInspector] public float PuckMaxVelocity = 20f;
    
    public Transform PuckTransform;
    
    [SerializeField] private Goalkeeper _goalkeeperController;
    [SerializeField] private Dragging _draggingController;
    [SerializeField] private Aiming _aimingController;
    [SerializeField] private Shooting _shootingController;
    
    private PuckBase _puckBaseController;
    
    public void Initialize(PuckBase puckBase)
    {
        _puckBaseController = puckBase;
    }
    
    private void Awake()
    {
        PuckSpringJoint = GetComponent<SpringJoint>();
        PuckRigidbody = GetComponent<Rigidbody>();
        SetRigidbodyInterpolation();
    }

    private void Start()
    {
        GetPuckStartPosition();
        _goalkeeperController.Initialize(_goalkeeperController);
        _draggingController.Initialize(_draggingController);
        _aimingController.Initialize(_aimingController);
        _shootingController.Initialize(_shootingController);
    }

    private void Update()
    {
        if (_aimingController.ClickedOn && !_shootingController.IsPuckShot)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && _aimingController.IsAimingEnabled)
            {
                _draggingController.DraggingPuck();
            }
            else
            {
                if (_draggingController.IsDragging)
                {
                    _draggingController.RemoveConfigurableJointAndStopDragging();
                }
            }
            
            _aimingController.HandleAiming();
        }

        _draggingController.RemoveSpringJointAndRestorePreviousVelocity();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !_shootingController.IsPuckShot )
        {
            _aimingController.StartAiming();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !_shootingController.IsPuckShot)
        {
            _shootingController.ShootPuck();
        }
        
        if (_shootingController.IsPuckShot && _draggingController.IsDragging)
        {
            _draggingController.RemoveConfigurableJointAndStopDragging();
            _aimingController.IsAimingEnabled = false;
        }
    }
    
    private void SetRigidbodyInterpolation()
    {
        PuckRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void GetPuckStartPosition()
    {
        PuckInitialPosition = transform.position;
    }
    
    public void ResetPuck()
    {
        _shootingController.IsPuckShot = false;
        _aimingController.IsAimingEnabled = true;
        PuckRigidbody.velocity = Vector3.zero;
        PuckRigidbody.angularVelocity = Vector3.zero;
        transform.position = PuckInitialPosition;
        _goalkeeperController.ResetRotation();
        _goalkeeperController.GoalkeeperAnimator.SetBool("IsRunning", false);
    }
}