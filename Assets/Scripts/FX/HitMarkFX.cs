using UnityEngine;

public class HitMarkFX : MonoBehaviour
{
    [SerializeField] private float _hitMarkExistTime;
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _hitMarkExistTime)
        {
            Destroy(gameObject);
        }
    }
}
