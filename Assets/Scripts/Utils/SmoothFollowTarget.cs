using UnityEngine;

/// <summary>
/// Отвечает за плавное движение камеры за игроком
/// </summary>
public class SmoothFollowTarget : Singleton<SmoothFollowTarget>
{
    public GameObject target;           // Ссылка на главного героя
    public Vector3 Offset;              // Расстояние до цели

    /// <summary>
    /// Встроенный метод, который вызывается в конце каждого кадра. 
    /// Используется для перемещения камеры за героем.  
    /// </summary>
    public void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) // Если сделать else к if(target == null) поломается отдаление камер
            {
                if (GameManager.Instance.CurrentScene == Scenes.MainGame)
                {
                    transform.position = new Vector3(1.5f, 12, -7) + target.transform.position;
                    Offset = transform.position - target.transform.position;
                }
                if (GameManager.Instance.CurrentScene == Scenes.Dungeon)
                {
                    transform.position = new Vector3(6, 12, 6) + target.transform.position;
                    Offset = transform.position - target.transform.position;
                }
            }
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target.transform.position + Offset, Time.deltaTime * 5);
    }
}

