using ServerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class MyPlayer
    {
        public Int64 ID { get; set; }
        public Player playerObject { get; set; }


        public MyPlayer(PlayerDto serverPlayer, GameObject prefab)
        {
            ID = serverPlayer.ID;
            playerObject = Player.Init(prefab, new Vector3(serverPlayer.X, 3, serverPlayer.Z));
        }


        public void Move()
        {
            playerObject.Move();
        }

        public void Rotate(Vector3 direction)
        {
            //playerObject.Rotate(direction);
        }

        public void Destroy()
        {
            playerObject.DestroyThis();
        }
    }
}
