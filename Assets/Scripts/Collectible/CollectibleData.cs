using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    CT_Damage,
    CT_Health,
    CT_Score
}

public class CollectibleData
{
    [SerializeField]
    public CollectibleType m_Type;

    [SerializeField]
    public int m_Value;

    public CollectibleData(CollectibleType InType, int InValue)
    {
        m_Type = InType;
        m_Value = InValue;
    }
}