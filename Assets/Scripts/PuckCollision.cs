using UnityEngine;

public class PuckCollision : MonoBehaviour
{
    [SerializeField] private Color _colorEnemy;
    [SerializeField] private Color _colorAlly;
    
    [SerializeField] private PuckBase _puckBaseController;
    [SerializeField] private ScenesMethods _scenesMethods;
    
    private int _goalsToEnemyCount;
    private int _goalsToYourselfCount;
    
    private void Start()
    {
        _puckBaseController.Initialize(_puckBaseController);
        _scenesMethods.Initialize(_scenesMethods);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            FX.Instance.PlayGoalExplosionFX(other.transform.position, _colorEnemy);
            Destroy(other.gameObject);
            _puckBaseController.ResetPuck();
            _goalsToEnemyCount++;
            if(_goalsToEnemyCount == 3)
                _scenesMethods.NextLevel();
        }
        
        if (other.CompareTag("GoalUr"))
        {
            FX.Instance.PlayGoalExplosionFX(other.transform.position, _colorAlly);
            Destroy(other.gameObject);
            _puckBaseController.ResetPuck();
            _goalsToYourselfCount++;
            if(_goalsToYourselfCount == 2)
                _scenesMethods.RestartLevel();
        }
        
        if (other.CompareTag("Goalkeeper"))
        {
            FX.Instance.PlayGoalExplosionFX(other.transform.position, _colorEnemy);
            _puckBaseController.ResetPuck();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != _puckBaseController.PuckTransform.gameObject)
        {
            Vector3 reflectionDirection = Vector3.Reflect(_puckBaseController.PreviousPuckVelocity.normalized, collision.contacts[0].normal);

            _puckBaseController.PuckRigidbody.velocity = reflectionDirection * Mathf.Max(_puckBaseController.PreviousPuckVelocity.magnitude, 10f) * 0.9f;

            _puckBaseController.PreviousPuckVelocity = _puckBaseController.PuckRigidbody.velocity;
        }
    }
}
