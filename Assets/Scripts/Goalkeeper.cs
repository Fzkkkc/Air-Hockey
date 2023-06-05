using UnityEngine;

public class Goalkeeper : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    
    [SerializeField] private PuckBase _puckBaseController;
    [SerializeField] private Shooting _shootingController;
    
    private Goalkeeper _goalkeeperController;
    
    private float _xMin = -2.6f; 
    private float _xMax = 2.6f;
    
    public Animator GoalkeeperAnimator;
    
    public void Initialize(Goalkeeper controller)
    {
        _goalkeeperController = controller;
    }

    private void Start()
    {
        _puckBaseController.Initialize(_puckBaseController);
        _shootingController.Initialize(_shootingController);
    }

    private void Update()
    {
        PlayAnimation();
        GoalkeeperFollowPuck();
    }

    private void GoalkeeperFollowPuck()
    {
        Vector3 currentPosition = transform.position;
        
        Vector3 puckPosition = _puckBaseController.PuckTransform.position;

        float targetX = Mathf.Clamp(puckPosition.x, _xMin, _xMax);
        Vector3 targetPosition = new Vector3(targetX, currentPosition.y, currentPosition.z);

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);

        Vector3 direction = (puckPosition - currentPosition).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void PlayAnimation()
    {
        GoalkeeperAnimator.SetBool("IsRunning", _shootingController.IsPuckShot);
    }
    
    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
