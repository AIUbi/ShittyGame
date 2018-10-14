using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLogic
{
    public CollectibleData m_CollectibleData;

    public CollectibleLogic(CollectibleData InData)
    {
        m_CollectibleData = InData;
    }

    public void Animate()
    {
        //Colored animation
    }
}

public class Collectible : MonoBehaviour
{
    public CollectibleLogic m_CollectibleLogic { get; private set; }

    private void Awake()
    {
        int RandomValue = Random.Range(0, 30);
        CollectibleType RandomType = (CollectibleType)Random.Range(0, 3);

        m_CollectibleLogic = new CollectibleLogic(new CollectibleData(RandomType, RandomValue));
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
