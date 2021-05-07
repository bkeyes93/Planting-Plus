using System.Threading;
using UnityEngine;

namespace PlantingPlus
{
    public static class ModLoader
    {
        public static GameObject Load;

        public static void Main(string[] args)
        {
            ModLoader.InitThreading();
        }

        public static void InitThreading()
        {
            new Thread(delegate ()
            {
                Thread.Sleep(10000);
                ModLoader.Init();
            }).Start();
        }

        public static void Init()
        {
            ModLoader.Load = new GameObject("PlantingPlus");
            ModLoader.Load.AddComponent<PlantingPlus>();
            UnityEngine.Object.DontDestroyOnLoad(ModLoader.Load);
        }
    }
}
