using MoreMountains.Tools;
using UnityEngine;

namespace Scripts
{
    public class Finder : MMSingleton<Finder>
    {
        public Transform Player { get; private set; }

        public void AddPlayerReference(Transform player)
        {
            Player = player;
        }
    }
}