using System;
using UnityEngine;

namespace Helicopter.Core.Services.PlayerInput
{
    public interface IPlayerInputService
    {
        void SubscribeToMove(Action<Vector2> action);
        void SubscribeToRotate(Action<float> action);
        void SubscribeToLift(Action<float> action);
        void UnsubscribeFromMove(Action<Vector2> action);
        void UnsubscribeFromRotate(Action<float> action);
        void UnsubscribeFromLift(Action<float> action);
    }
}