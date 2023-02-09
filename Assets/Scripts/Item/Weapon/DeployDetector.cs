using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployDetector : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController _playerWeaponController;
    [SerializeField] private SpriteRenderer _deployIndicator;

    private void Start()
    {
        _playerWeaponController.isDeployable = true;
        _deployIndicator.color = Color.white;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null &&
            (collision.gameObject.CompareTag("EnemyPlayer") ||
            collision.gameObject.CompareTag("Portal") ||
            collision.gameObject.CompareTag("Well") ||
            collision.gameObject.CompareTag("Deployable_Activation") ||
            collision.gameObject.CompareTag("Mine") ||
            collision.gameObject.CompareTag("Merchant") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Default") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Character") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Map_Wall") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Deco") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Water") ||
            collision.gameObject.layer == LayerMask.NameToLayer("EnemyAI")))
        {
            _playerWeaponController.isDeployable = false;
            _deployIndicator.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null &&
            (collision.gameObject.CompareTag("EnemyPlayer") ||
            collision.gameObject.CompareTag("Portal") ||
            collision.gameObject.CompareTag("Well") ||
            collision.gameObject.CompareTag("Deployable_Activation") ||
            collision.gameObject.CompareTag("Mine") ||
            collision.gameObject.CompareTag("Merchant") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Default") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Character") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Map_Wall") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Deco") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Water") ||
            collision.gameObject.layer == LayerMask.NameToLayer("EnemyAI")))
        {
            _playerWeaponController.isDeployable = true;
            _deployIndicator.color = Color.white;
        }
    }
}
