using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ConvertedToEntityHolder : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        GetComponentInParent<AIMovementController>().entity = entity;
        GetComponentInParent<AIMovementController>().entityManager = dstManager;
        transform.parent.gameObject.SetActive(false);
    }
}