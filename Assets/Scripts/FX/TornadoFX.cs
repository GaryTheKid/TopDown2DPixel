using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoFX : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _growSpeed;
    [SerializeField] private Effector2D _effector;
    [SerializeField] private Collider2D _collider;

    private void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
    }

    private void Update()
    {
        if (transform.localScale.x < 1f)
        {
            var newScale = transform.localScale.x + Time.deltaTime * _growSpeed;
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }
        else
        {
            if (_effector.enabled == false)
            {
                _effector.enabled = true;
            }

            if (_collider.enabled == false)
            {
                _collider.enabled = true;
            }
        }
    }

    public void MoveTowards(Vector2 initPos, Vector2 targetPos)
    {
        var dir = (targetPos - initPos).normalized;
        GetComponent<Rigidbody2D>().AddForce(dir * _moveSpeed, ForceMode2D.Impulse);
    }

    public void Expire()
    {
        StartCoroutine(Co_Fade());
    }
    IEnumerator Co_Fade()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        GetComponent<Animator>().SetTrigger("Fade");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
