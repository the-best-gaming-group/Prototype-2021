using System;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an door.
    /// </summary>
    /// <typeparam name="doorCollision"></typeparam>
    public class PlayerDoorCollision : Simulation.Event<PlayerDoorCollision>
    {
        public DoorController door;
        public GhostController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            door.invokable.Invoke();
        }
        
    }
}