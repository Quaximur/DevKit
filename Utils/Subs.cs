using System;

namespace DevKit.Utils
{
    public static class Subs
    {
        public static void SubscribeOnce(Action action, Action<Action> subscribe, Action<Action> unsubscribe)
        {
            Action handler = null;
            handler = () =>
            {
                unsubscribe(handler);
                action();
            };
            subscribe(handler);
        }

        public static void SubscribeOnce<T>(Action<T> action, Action<Action<T>> subscribe,
            Action<Action<T>> unsubscribe)
        {
            Action<T> handler = null;
            handler = x =>
            {
                unsubscribe(handler);
                action(x);
            };
            subscribe(handler);
        }

        public static void SubscribeOnce(Action action, ref Action observable, Action<Action> unsubscribe)
        {
            Action handler = null;
            handler = () =>
            {
                unsubscribe(handler);
                action();
            };
            observable += handler;
        }
    }
}