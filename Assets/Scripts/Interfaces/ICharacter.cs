using System.Collections;
using UnityEngine;

/// <summary>
/// Интерфейс, отвечающий за основную механику персонажей.
/// </summary>
public interface ICharacter
{
    /// <summary>
    /// Получиет урон в размере damage.
    /// </summary>
    /// <param name="damage">Получаемый урон</param>
    void GetDamage(float damage);

    /// <summary>
    /// Атакует выбранную цель.
    /// </summary>
    /// <param name="character">Цель атаки</param>
    void Attack(ICharacter character);

    /// <summary>
    /// Обрабатывает смерть персонажа.
    /// </summary>
    void Die();

    /// <summary>
    /// Восполняет здоровье и ману персонажа.
    /// </summary>
    /// <param name="healRecovery">Скорость восстановления здоровья</param>
    /// <param name="manaRecovery">Скорость восстановления маны</param>
    /// <param name="delay">Задержка между лечениями</param>
    /// <returns></returns>
    IEnumerator Recovery(float healRecovery = 5, float manaRecovery = 5, float delay = 1);
}

