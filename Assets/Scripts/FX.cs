using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _goalExplosionFX;
    private ParticleSystem.MainModule _goalExplosionFXMainModule;
    public static FX Instance;

    private void Awake() =>
        Instance = this;

    private void Start()
    {
        _goalExplosionFXMainModule = _goalExplosionFX.main;
    }

    public void PlayGoalExplosionFX(Vector3 position, Color color)
    {
        _goalExplosionFXMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
        _goalExplosionFX.transform.position = position;
        _goalExplosionFX.Play();
    }
}