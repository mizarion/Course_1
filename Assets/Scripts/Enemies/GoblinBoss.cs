/// <summary>
/// Класс, реализующий противника короля гоблинов.
/// </summary>
public class GoblinBoss : AbstractEnemy
{
    private void Awake()
    {
        InitializeProperties(DataManager.Stats.Goblin.Health, DataManager.Stats.Goblin.Manapool, DataManager.Stats.Goblin.Experience,
            DataManager.Stats.Goblin.Damage, "Goblin boss", 10, DataManager.Stats.Goblin.ExperienceMultiplier);
    }

    /// <summary>
    /// Обработчик смерти босса - победа в игре.
    /// </summary>
    public override void Die()
    {
        CanvasManager.Instance.Victory();
    }
}
