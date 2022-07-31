using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpellMeteorRain : MonoBehaviour
{
    [Tooltip("Range of the effect")]
    [SerializeField] private int _strikeRange;
    [Tooltip("How many waves of meteor")]
    [SerializeField] private int _strikeWaveCount;
    [Tooltip("The CD between each wave")]
    [SerializeField] private float _strikeWaveCD;
    [Tooltip("Strike CD add a random variation between 0 and this value")]
    [SerializeField] private float _strikeWaveCDVariation;
    [Tooltip("The min number of meteor that each wave spawn")]
    [SerializeField] private int _MeteorEachWave_Min;
    [Tooltip("The max number of meteor that each wave spawn")]
    [SerializeField] private int _MeteorEachWave_Max;
    [Tooltip("CD between each meteor in 1 wave")]
    [SerializeField] private float _summonOneMeteorCD;
    [Tooltip("CD variation between each meteor")]
    [SerializeField] private float _summonOneMeteorCDVariation;
    [SerializeField] private AudioSource _soundFX;
    [SerializeField] private Transform _pfMeteor;
    private PhotonView _attackPV;
    private int _strikeTimes;
    private float _timer;
    private float _cd;

    private void Start()
    {
        _soundFX.Play();
        _cd = _strikeWaveCD + Random.Range(0f, _strikeWaveCDVariation);
    }

    private void Update()
    {
        // strike wave
        if (_strikeTimes >= _strikeWaveCount)
        {
            DestroySelf();
            return;
        }
            

        // timer
        _timer += Time.deltaTime;
        if (_timer >= _cd)
        {
            _cd = _strikeWaveCD + Random.Range(0f, _strikeWaveCDVariation);
            _timer = 0f;
            _strikeTimes++;

            // summon 1 wave of meteor
            SummonOneWaveOfMeteors();
        }
    }

    public void SetAttackerPV(PhotonView PV)
    {
        _attackPV = PV;
    }

    public void SummonOneWaveOfMeteors()
    {
        StartCoroutine(Co_SummonOneWaveOfMeteors());
    }
    IEnumerator Co_SummonOneWaveOfMeteors()
    {
        // strike times
        var meteorCounter = 0;
        var totalMeteor = Random.Range(_MeteorEachWave_Min, _MeteorEachWave_Max + 1);
        while (meteorCounter < totalMeteor)
        {
            var timer = 0f;
            var cd = _summonOneMeteorCD + Random.Range(0f, _strikeWaveCDVariation);
            while (timer < cd)
            {
                // update timer
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            // summon 1 meteor
            SummonOneMeteor();
            meteorCounter++;
        }
    }

    public void SummonOneMeteor()
    {
        // TODO: make this networkd
        Vector2 myPos = transform.position;
        Vector2 randMeteorSpawnPos = myPos + Random.insideUnitCircle * _strikeRange;
        Transform meteor = Instantiate(_pfMeteor, randMeteorSpawnPos, Quaternion.identity, GameManager.gameManager.FXParent);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
