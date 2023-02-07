using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeployableObject_World : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _visuals;

    private DeployableObject _deployableObject;
    private PhotonView _deployerPV;
    private Animator _animator;
    private float _dmgRatio = 1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null && !bush.GetComponent<Animator>().GetBool("Reveal"))
        {
            bush.RevealBush();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null && bush.GetComponent<Animator>().GetBool("Reveal"))
        {
            bush.HideBush();
        }
    }

    public void PerishInTime()
    {
        StartCoroutine(Co_Perish(_deployableObject.lifeTime));
    }

    private IEnumerator Co_Perish(float lifeTime)
    {
        yield return new WaitForSecondsRealtime(lifeTime);
        Destroy(gameObject);
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

    public void DisablePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }

    public void SetObjectVisible()
    {
        /*foreach (var visual in _visuals)
        {
            visual.color = new Color(visual.color.r, visual.color.g, visual.color.b, 0.5f);
        }*/
    }
}
