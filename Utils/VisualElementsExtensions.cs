using System;
using R3;
using UnityEngine.UIElements;

namespace DevKit.Solutions.Utils
{
    public static class VisualElementsExtensions
    {
        public static IDisposable SubscribeCallback<TEventType>(this VisualElement element,
            EventCallback<TEventType> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback<TEventType>(callback, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType>(callback, trickleDown));
        }

        public static IDisposable SubscribeCallback<TEventType, TUserArgs>(this VisualElement element,
            EventCallback<TEventType, TUserArgs> callback, TUserArgs userArgs, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback<TEventType, TUserArgs>(callback, userArgs, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType, TUserArgs>(callback, trickleDown));
        }
    }
}