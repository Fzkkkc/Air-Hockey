using UnityEngine;

public class Dragging : MonoBehaviour
{
    [HideInInspector] public bool IsDragging;
    
    [SerializeField] private Aiming _aimingController;
    [SerializeField] private PuckBase _puckBaseController;
    
    private Dragging _draggingController;
    
    public void Initialize(Dragging controller)
    {
        _draggingController = controller;
    }
    
    private void Start()
    {
        _puckBaseController.Initialize(_puckBaseController);
        _aimingController.Initialize(_aimingController);
    }

    public void DraggingPuck()
    { 
        if (!IsDragging)
        {
            IsDragging = true;
            /*AddConfigurableJoint();*/
        }
        else
        {
            /*UpdateConfigurableJointAnchor();*/
            
            if (_aimingController.AimLineRenderer.enabled)
            { 
                MovePuckTowardsAimLine();
            }
        }
    }
    
    private void MovePuckTowardsAimLine()
    {
        var fingerPosition = Input.mousePosition;
        fingerPosition.z = 10f; // расстояние от камеры до экрана
        fingerPosition = Camera.main.ScreenToWorldPoint(fingerPosition);
    
        var direction = fingerPosition - transform.position;

        var newPosition = transform.position + direction.normalized * Time.deltaTime * 3f;

        newPosition.x = Mathf.Clamp(newPosition.x,
            _puckBaseController.PuckInitialPosition.x - 1f,
            _puckBaseController.PuckInitialPosition.x + 1f);

        newPosition.z = Mathf.Clamp(newPosition.z,
            _puckBaseController.PuckInitialPosition.z - 1f,
            _puckBaseController.PuckInitialPosition.z + 1f);

        // Установка новой позиции с использованием старой координаты Y
        newPosition.y = _puckBaseController.PuckInitialPosition.y;

        transform.position = newPosition;
    }


    
    public void RemoveConfigurableJointAndStopDragging()
    {
        /*var configurableJoint = GetCompondent<ConfigurableJoint>();
        Destroy(configurableJoint);*/
        IsDragging = false;
    }
    
    public void RemoveSpringJointAndRestorePreviousVelocity()
    {
        if (_puckBaseController.PuckSpringJoint != null 
            && !_puckBaseController.PuckRigidbody.isKinematic
            && _puckBaseController.PreviousPuckVelocity.sqrMagnitude > _puckBaseController.PuckRigidbody.velocity.sqrMagnitude
            && _puckBaseController.PuckSpringJoint != null)
        {
            Destroy(_puckBaseController.PuckSpringJoint);
            _puckBaseController.PuckRigidbody.velocity = _puckBaseController.PreviousPuckVelocity;
        }
    }
}