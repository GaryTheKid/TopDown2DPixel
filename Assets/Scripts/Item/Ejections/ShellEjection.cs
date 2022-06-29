using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ShellEjection : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(Co_Eject());
    }

    IEnumerator Co_Eject()
    {
        GetComponent<AudioSource>().pitch = Random.Range(1f, 1.2f);
        _rb.AddForce(Math.GetRandomDirectionV2() * 2f, ForceMode2D.Impulse);
        _rb.AddTorque(Random.Range(1f, 5f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        _rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
