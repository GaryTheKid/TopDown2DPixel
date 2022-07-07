/* Last Edition: 07/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The AI Brain to control the how AI Behaves.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class AIBrain : MonoBehaviour
{
    private AIMovementController _aiMovementController;
    private Vector3 startingPos;
    private Vector3 roamPosition;

    private void Awake()
    {
        _aiMovementController = GetComponent<AIMovementController>();
    }

    private void Start()
    {
        startingPos = transform.position;
        roamPosition = GetRoamingPosition();

        _aiMovementController.MoveTo(roamPosition);

        
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPos + UtilsClass.GetRandomDir() * Random.Range(10f, 70f);
    }
}
