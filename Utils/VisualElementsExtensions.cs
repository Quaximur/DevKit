using System;
using R3;
using UnityEngine.UIElements;

namespace DevKit.Utils
{
    public static class VisualElementsExtensions
    {
        public static IDisposable SubscribeCallback<TEventType>(this VisualElement element,
            EventCallback<TEventType> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback<TEventType>(callback, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType>(callback, trickleDown));
        }

        public static IDisposable SubscribeCallback<TEventType, TUserArgs>(this VisualElement element,
            EventCallback<TEventType, TUserArgs> callback, TUserArgs userArgs,
            TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback<TEventType, TUserArgs>(callback, userArgs, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType, TUserArgs>(callback, trickleDown));
        }

        public static IDisposable SubscribeCallbackOnce<TEventType>(this VisualElement element,
            EventCallback<TEventType> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallbackOnce<TEventType>(callback, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType>(callback, trickleDown));
        }

        public static IDisposable SubscribeCallbackOnce<TEventType, TUserArgs>(this VisualElement element,
            EventCallback<TEventType, TUserArgs> callback, TUserArgs userArgs,
            TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallbackOnce<TEventType, TUserArgs>(callback, userArgs, trickleDown);
            return Disposable.Create(() => element.UnregisterCallback<TEventType, TUserArgs>(callback, trickleDown));
        }

        public static VisualElement GetFirstOfTypeWithUserData<T>(this VisualElement ve)
        {
            while (ve != null)
            {
                if (ve.userData is T) return ve;
                ve = ve.parent;
            }
            return null;
        }
    }
}