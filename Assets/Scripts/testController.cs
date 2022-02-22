using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class testController : MonoBehaviour
{
    [SerializeField] private Button_UI button;

    // Start is called before the first frame update
    void Start()
    {
        button.ClickFunc = () =>
        {
            print(GetComponent<IEquipable>());
        };

        
    }
}
