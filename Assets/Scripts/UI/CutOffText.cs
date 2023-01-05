/* Last Edition: 05/22/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * Reference: CodeMonkey
 * 
 * Description: 
 *   Used for creating inverse mask UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutOffText : Text
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
