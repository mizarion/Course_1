using System.Collections.Generic;

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
            public static readonly List<int> Health = new List<int>() { 0, 100, 200, 250, 350, 400 };
            public static readonly List<int> Manapool = new List<int>() { 0, 100, 125, 150, 200, 250 };
            public static readonly List<int> Experience = new List<int>() { 0, 100, 125, 150, 200, 250 };
        }

        /// <summary>
        /// Содержит характеристики Гоблина
        /// </summary>
        static class Goblin
        {
            // первый элемент 0 для удобного обращения
            public static readonly List<int> Health = new List<int>() { 0, 100, 200, 250, 350, 400 };
            public static readonly List<int> Manapool = new List<int>() { 0, 100, 125, 150, 200, 250 };
            public static readonly List<int> Experience = new List<int>() { 0, 100, 125, 150, 200, 250 };
            public static readonly float ExperienceCost = 10;
        }

        /// <summary>
        /// Заглушка
        /// </summary>
        static class Default
        {
            public static readonly List<int> Health = new List<int>() { 0, 0 };
            public static readonly List<int> Manapool = new List<int>() { 0, 0 };
            public static readonly List<int> Experience = new List<int>() { 0, 0 };
        }
    }

    /// <summary>
    /// Содержит данные о сценах
    /// </summary>
    static class Scenes
    {
        public static readonly int startScene = 0;
        public static readonly int MainGame = 1;
    }
}
