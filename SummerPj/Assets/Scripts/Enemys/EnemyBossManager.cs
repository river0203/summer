using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    //페이즈 관리
    //공격 패턴
    public string _bossName;

    public UIBossHealthBar _bossHealthBar;
    EnemyStats _enemyStats;

    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemyStats._maxHealth);
    }

    public void UpdateBossHealthBar(int _currentHealth)
    {
        _bossHealthBar.SetBossCurrentHealth(_currentHealth);
    }
}
