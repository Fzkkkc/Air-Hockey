using UnityEngine;

public class Shooting : MonoBehaviour
{
    [HideInInspector] public bool IsPuckShot = false;
    
    [SerializeField] private PuckBase _puckBaseController;
    [SerializeField] private Dragging _draggingController;
    [SerializeField] private Aiming _aimingController;
    
    private Shooting _shootingController;
    
    private float _shootPowerMultiplier = 10f;
    private float _minShootPower = 2f;
    private float _maxShootPower = 12f;
    
    public void Initialize(Shooting controller)
    {
        _shootingController = controller;
    }

    private void Start()
    {
        _aimingController.Initialize(_aimingController);
        _draggingController.Initialize(_draggingController);
        _puckBaseController.Initialize(_puckBaseController);
    }
    
    public void ShootPuck()
    {
        IsPuckShot = true;
        _aimingController.IsAimingEnabled = false;
        _aimingController.ClickedOn = false;
        
        _aimingController.MouseUpPoint = _aimingController.GetMouseWorldPosition();

        _aimingController.AimLineRenderer.enabled = false;

        Vector3 direction = (transform.position - _aimingController.MouseUpPoint).normalized;

        float distance = Vector3.Distance(_aimingController.MouseDownPoint, _aimingController.MouseUpPoint);
        float power = Mathf.Clamp(distance * _shootPowerMultiplier, _minShootPower, _maxShootPower);
        _puckBaseController.PuckRigidbody.velocity = Vector3.ClampMagnitude(_puckBaseController.PuckRigidbody.velocity,
                                                                            _puckBaseController.PuckMaxVelocity);
        Vector3 velocity = direction * power;
        _puckBaseController.PuckRigidbody.velocity = velocity;

        _puckBaseController.PreviousPuckVelocity = _puckBaseController.PuckRigidbody.velocity;
        
        if(_puckBaseController.PuckSpringJoint != null)
            Destroy(_puckBaseController.PuckSpringJoint);
    }
}