using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    UIBossHealthBar _bossHealthBar;

    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActivateBossFight()
    {
        _bossHealthBar.SetUIHealthBarToActive();
    }
}
