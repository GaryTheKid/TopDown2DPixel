/* Last Edition: 01/19/2023
* Author: Chongyang Wang
* Collaborators: 
* 
* Description: 
*   The controller controls player resources
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceController : MonoBehaviour
{
    private PlayerStatsController _playerStatsController;

    private void Awake()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
    }

    public int GetCurrentGold()
    {
        return _playerStatsController.playerStats.gold;
    }

    public void GainGold(int amount)
    {
        _playerStatsController.UpdateGold(amount);
    }

    public void LoseGold(int amount)
    {
        _playerStatsController.UpdateGold(-amount);
    }
}
