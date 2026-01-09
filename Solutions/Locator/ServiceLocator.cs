using System;
using System.Collections.Generic;
using DevKit.Utils;

namespace DevKit.Locator
{
    public class ServiceLocator
    {
        public static ServiceLocator Current { get; } = new();

        private readonly Dictionary<Type, object> _services = new();

        public void Register<TService>(TService service) where TService : class
        {
            var type = typeof(TService);

            if (!_services.TryAdd(type, service))
                FLogger.LogWarning($"Service of type {type.FullName} is already registered");
        }

        public void Register(Type type, object service)
        {
            if (!type.IsInstanceOfType(service))
                throw new ArgumentException(
                    $"[ServiceLocator]: (Register). {nameof(service)} is not implementing the {type.FullName}");

            if (!_services.TryAdd(type, service))
                FLogger.LogWarning<ServiceLocator>($"(Register). Service of type {type.FullName} is already registered");
        }

        public void Unregister<TService>() where TService : class
        {
            var type = typeof(TService);

            if (!_services.Remove(type))
                FLogger.LogWarning<ServiceLocator>($"(Remove). Service of type {type.FullName} not registered");
        }

        public bool TryGet<TService>(out TService service) where TService : class
        {
            var type = typeof(TService);
            if (_services.TryGetValue(type, out var value))
            {
                service = (TService)value;
                return true;
            }

            FLogger.LogError<ServiceLocator>($"(TryGet). Service of type {type.FullName} not registered");
            service = null;

            return false;
        }

        public TService Get<TService>() where TService : class
        {
            var type = typeof(TService);

            if (_services.TryGetValue(type, out var service))
                return (TService)service;

            throw new ArgumentException($"[ServiceLocator]: (Get). Service of type {type.FullName} not registered");
        }
    }
}