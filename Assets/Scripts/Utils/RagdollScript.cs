using UnityEngine;

/// <summary>
/// Класс, анимирующий смерть противника.
/// </summary>
public class RagdollScript : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] Rigidbody rbody;           // Ссылка на компонент Rigidbody
    [SerializeField] float forceToAdd = 5000;   // Множитель для силы отталкивания 

#pragma warning restore 649

    /// <summary>
    /// Анимирует смерть врага.
    /// </summary>
    /// <param name="force"></param>
    /// <param name="liveTime"></param>
    public void StartDeath(Vector3 force, float liveTime)
    {
        rbody.AddForce(force * forceToAdd);
        Destroy(gameObject, liveTime);
    }
}
