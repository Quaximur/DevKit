using System;
using Reflex.Core;
using DevKit.UI.MVVM.Bases;
using UnityEngine;

using Resolution = Reflex.Enums.Resolution;
using Lifetime = Reflex.Enums.Lifetime;
using Object = UnityEngine.Object;

namespace DevKit.UI.MVVM
{
    public static class MVVMReflexExtensions
    {
        public static ContainerBuilder RegisterMVVM<TView, TViewModel>(this ContainerBuilder builder,
            TView viewPrefab, 
            Func<Container, TViewModel> viewModelFactory = null,
            Lifetime viewModelLifetime = Lifetime.Singleton,
            Lifetime binderLifetime = Lifetime.Singleton)
            where TView : MonoBehaviour, IScreenAttach<TViewModel>, IDisposableNotifier
            where TViewModel : IScreenViewModel
        {
            return builder.RegisterMVVM<TView, TViewModel, SimpleAttachBinder<TView, TViewModel>>(
                viewPrefab, 
                viewModelFactory, 
                viewModelLifetime, 
                binderLifetime);
        }
        public static ContainerBuilder RegisterMVVM<TView, TViewModel, TBinder>(this ContainerBuilder builder,
            TView viewPrefab, 
            Func<Container, TViewModel> viewModelFactory = null,
            Lifetime viewModelLifetime = Lifetime.Singleton,
            Lifetime binderLifetime = Lifetime.Singleton)
            where TView : MonoBehaviour, IScreenAttach<TViewModel>, IDisposableNotifier
            where TViewModel : IScreenViewModel
            where TBinder : IViewBinder<TViewModel>
        {
            builder.RegisterFactory<Func<TView>>(x => () => Object.Instantiate(viewPrefab),
              Lifetime.Singleton, Resolution.Lazy);

            builder.RegisterType(typeof(TViewModel),
                viewModelLifetime, Resolution.Lazy);

            var vmFactory = viewModelFactory ?? (x => x.Resolve<TViewModel>());

            builder.RegisterFactory<Func<TViewModel>>(x => () => vmFactory(x), 
                Lifetime.Singleton, Resolution.Lazy);

            builder.RegisterType(typeof(TBinder),
                new Type[] { typeof(IViewBinder<TViewModel>) },
                binderLifetime, Resolution.Lazy);

            return builder;
        }
    }
}