using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;
using UnityEngine.Rendering.Universal;

public class DeployableObject_World : MonoBehaviour
{
    public bool isLocked;

    [SerializeField] private SpriteRenderer[] _visuals;
    [SerializeField] private DeployableFX _deplpyableFX;
    [SerializeField] private Light2D _pointLight;
    private Color[] _initColors;

    private PhotonView _PV;
    private DeployableObject _deployableObject;
    private PhotonView _deployerPV;
    private Animator _animator;
    private float _dmgRatio = 1f;

    private IEnumerator _co_activate;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _initColors = new Color[_visuals.Length];
        for (int i = 0; i < _visuals.Length; i++)
        {
            _initColors[i] = new Color(_visuals[i].color.r, _visuals[i].color.g, _visuals[i].color.b);
        }
    }

    private void OnEnable()
    {
        // reset deployable
        isLocked = false;
        ShowDeactivateVisual();
        if (GetDeployerPV().IsMine)
        {
            ShowDetectionVisual();
        }
        else
        {
            HideDetectionVisual();
        }
    }

    private void OnDisable()
    {
        // reset visuals
        TurnOffActivationLight();
        for (int i = 0; i < _visuals.Length; i++)
        {
            _visuals[i].color = new Color(_initColors[i].r, _initColors[i].g, _initColors[i].b, 0f);
        }
        _co_activate = null;
    }

    public void PerishInTime()
    {
        StartCoroutine(Co_Perish(_deployableObject.lifeTime));
    }

    private IEnumerator Co_Perish(float lifeTime)
    {
        yield return new WaitForSecondsRealtime(lifeTime);
        DestroySelf();
    }

    public DeployableObject GetDeployableObject()
    {
        return _deployableObject;
    }

    public void SetDeployableObject(DeployableObject deployableObject)
    {
        _deployableObject = deployableObject;
    }

    public PhotonView GetDeployerPV()
    {
        return _deployerPV;
    }

    public void SetDeployerPV(PhotonView PV)
    {
        _deployerPV = PV;
    }

    public PhotonView GetThisPV()
    {
        return _PV;
    }

    public void SetDamageRatio(float dmgRatio)
    {
        _dmgRatio = dmgRatio;
    }

    public void ShowDetectionVisual()
    {
        _animator.SetBool("ShowDetectionVisual", true);
    }

    public void HideDetectionVisual()
    {
        _animator.SetBool("ShowDetectionVisual", false);
    }

    public void ShowActivateVisual()
    {
        _animator.SetBool("isActive", true);
    }

    public void ShowDeactivateVisual()
    {
        _animator.SetBool("isActive", false);
    }

    public void Activate()
    {
        if (_co_activate == null)
        {
            _co_activate = Co_Activate();
            StartCoroutine(_co_activate);
        }
    }

    public void Deactivate()
    {
        if (_co_activate != null)
        {
            StartCoroutine(_co_activate);
            _co_activate = null;
        }
    }

    public void TurnOffActivationLight()
    {
        _pointLight.intensity = 0f;
    }

    public void DestroySelf()
    {
        isLocked = true;
        ShowDetectionVisual();
        ShowDeactivateVisual();
        TurnOffActivationLight();
        // inform object pool
        ObjectPool.objectPool.isAllLootBoxActive = false;

        gameObject.SetActive(false);
    }

    public void DisablePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }

    public bool IsDeployableDeactivatable()
    {
        return _deployableObject.isDeactivatable;
    }

    IEnumerator Co_Activate()
    {
        yield return new WaitForSecondsRealtime(_deployableObject.activationTime);

        _deplpyableFX.FireFX();
    }
}