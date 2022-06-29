using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ShellEjection : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Co_Eject());
    }

    IEnumerator Co_Eject()
    {
        GetComponent<Rigidbody2D>().AddForce(Math.GetRandomDirectionV2() * 2f, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(1f, 5f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
