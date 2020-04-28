using System.Collections.Generic;

/// <summary>
/// Пространство имен, отвечающее за хранение игровых данных 
/// </summary>
namespace DataManager
{
    /// <summary>
    /// Содержит классы с данными о персонажах
    /// </summary>
    namespace Stats
    {
        /// <summary>
        /// Содержит характеристики Героя
        /// </summary>
        static class Player
        {
            public static readonly List<int> Health = new List<int>() { 0, 100, 125, 170, 200, 250, 300, 350, 400, 450, 500 };
            public static readonly List<int> Manapool = new List<int>() { 0, 100, 125, 150, 175, 200, 225, 250, 275, 300, 350 };
            public static readonly List<int> Experience = new List<int>() { 0, 20, 40, 60, 80, 100, 125, 150, 175, 200, 250 };
            public static readonly List<int> Damage = new List<int>() { 0, 20, 25, 30, 35, 40, 50, 60, 75, 85, 100 };
        }

        /// <summary>
        /// Содержит характеристики Гоблина
        /// </summary>
        static class Goblin
        {
            // первый элемент 0 для удобного обращения
            public static readonly List<int> Health = new List<int>() { 0, 50, 60, 70, 85, 100 };
            public static readonly List<int> Manapool = new List<int>() { 0, 100, 125, 150, 200, 250 };
            public static readonly List<int> Experience = new List<int>() { 0, 100, 125, 150, 200, 250 };
            public static readonly List<int> Damage = new List<int>() { 0, 10, 15, 20, 25, 30 };
            public static readonly float ExperienceMultiplier = 10;
        }

        /// <summary>
        /// Заглушка
        /// </summary>
        static class Default
        {
            public static readonly List<int> Health = new List<int>() { 0, 0 };
            public static readonly List<int> Manapool = new List<int>() { 0, 0 };
            public static readonly List<int> Experience = new List<int>() { 0, 0 };
            public static readonly List<int> Damage = new List<int>() { 0, 0 };
            public static readonly float ExperienceCost = 0;
        }
    }

    /// <summary>
    /// Содержит данные о сценах
    /// </summary>
    public enum Scenes
    {
        StartScene = 0,
        MainGame = 1
    }
}
