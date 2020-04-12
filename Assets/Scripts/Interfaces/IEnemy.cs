using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    ///// <summary>
    ///// Проверяет, находится ли игрок в радиусе агра
    ///// </summary>
    ///// <param name="target"></param>
    ///// <param name="agrRadius"></param>
    ///// <returns></returns>
    //bool GetAggressive(Vector3 target, float agrRadius = 10);

    /// <summary>
    /// Реализует движение к переданной позиции.
    /// </summary>
    /// <param name="target">Координаты</param>
    /// <param name="movespeed">Скорость передвижения</param>
    ///// <param name="agrRadius">Дистанция, с которой враг замечает героя и заагривается</param>
    /// <param name="endAgrRadius">Дистанция, с которой враг теряет героя и агр спадает</param>
    void Move(Vector3 target, float movespeed = 2, /*float agrRadius = 10,*/ float endAgrRadius = 15);
}
