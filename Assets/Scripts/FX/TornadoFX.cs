using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoFX : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _growSpeed;
    [SerializeField] private AudioSource _soundFX;

    private void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        _soundFX.Play();
    }

    private void Update()
    {
        if (transform.localScale.x < 1f)
        {
            var newScale = transform.localScale.x + Time.deltaTime * _growSpeed;
            transform.localScale = new Vector3(newScale, newScale, 1f);
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
