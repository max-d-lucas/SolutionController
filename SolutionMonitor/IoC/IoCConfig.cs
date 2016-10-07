namespace SolutionMonitor.IoC
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Common.Logging;
    using Helpers;

    public class IoCConfig
    {
        private static readonly ILog _Logger = LogManager.GetLogger<IoCConfig>();


        internal static readonly Dictionary<Type, Type> SingletonsRegistry;
        internal static Dictionary<Type, Type> AsSelfRegistery;
        internal static Dictionary<Type, Type> ServiceRegistry;

        static IoCConfig()
        {
            SingletonsRegistry = new Dictionary<Type, Type> { { typeof(ILogManager), typeof(LogManager) } };
            AsSelfRegistery = new Dictionary<Type, Type> { { typeof(SolutionManager), typeof(SolutionManager) } };
        }
        public static IContainer Configure()
        {
            _Logger.Trace(d => d("Processing IoC registrations..."));
            var builder = new ContainerBuilder();
            builder.PerLifetimeScopeRegistrations(ServiceRegistry, false);
            builder.PerLifetimeScopeRegistrations(AsSelfRegistery, true);
            builder.SingleInstanceRegistrations(SingletonsRegistry, false);

            var container = builder.Build();
            return container;
        }
    }
}