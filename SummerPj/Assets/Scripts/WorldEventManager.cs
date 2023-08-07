using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    //�Ȱ� �� 

    public UIBossHealthBar _bossHealthBar;
    public EnemyBossManager _boss;

    public bool _bossFightIsActive;
    public bool _bossHasBeenAwaken; //wake ���³� �ƽ� ����
    public bool _bossHasBeanDefeated;

    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActivateBossFight()
    {
        _bossFightIsActive = true;
        _bossHasBeenAwaken = true;  
        _bossHealthBar.SetUIHealthBarToActive();
    }

    public void BossHasBeenDefeated()
    {
        _bossHasBeanDefeated = true;
        _bossFightIsActive = false;
    }
}
