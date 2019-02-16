using BimManufact.WebApi.Models;
using Microsoft.Practices.Unity;
using System;

namespace BimManufact.WebApi
{
    public static class UnityConfiguration
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            container.RegisterType<IBimManufactWebApiContext, BimManufactWebApiContext>(new HierarchicalLifetimeManager());

            return container;
        });

        public static IUnityContainer Instance => Container.Value;
    }
}