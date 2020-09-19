using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBase : ItemBase
{ 
    public enum UtilityType
    {
        grenade,
        flashbang,
    }

    public UtilityType m_utilityType = UtilityType.grenade;
    public int m_maxStack = 1;
    
    

}
