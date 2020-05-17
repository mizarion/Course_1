/// <summary>
/// Класс, содержащий данные о гоблинах для сохранения.
/// </summary>
public class GoblinData
{
    public GoblinData(Goblin goblin)
    {
        Health = goblin.Health;
        Manapool = goblin.Manapool;
        Level = goblin.Level;
    }
    
    public float Health;     // Хранение данных о здоровье героя
    public float Manapool;   // Хранение данных о мане героя
    public int Level;        // Хранение данных об уровне героя
}
