using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public class PlayerPositionUpdateEvent : VSGameEvent
    {
        public Vector3 PlayerPosition;

        public PlayerPositionUpdateEvent(Vector3 pos)
        {
            PlayerPosition = pos;
        }
    }
}
