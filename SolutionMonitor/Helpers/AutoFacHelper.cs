namespace SolutionMonitor.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Autofac;
    using Common.Logging;

    public static class AutoFacHelper
    {
        public static void SingleInstanceRegistrations(this ContainerBuilder builder, Dictionary<Type, Type> registry, bool asSelf)
        {
            if (registry == null)
            {
                return;
            }
            var logger = AutoFacHelper.LoggerGet();
            foreach (var key in registry.Keys)
            {
                if(asSelf)
                {
                    logger.Trace(t => t("Registering \"{0}\" as AsSelf.SingleInstance", key));
                    builder.RegisterType(registry[key])
                           .AsSelf()
                           .SingleInstance();
                }
                else
                {
                    logger.Trace(t => t("Registering \"{0}\" as SingleInstance", key));
                    builder.RegisterType(registry[key])
                           .As(key)
                           .SingleInstance();
                }
            }
        }

        public static void PerRequestRegistrations(this ContainerBuilder builder, Dictionary<Type, Type> registry, bool asSelf)
        {
            if (registry == null)
            {
                return;
            }
            var logger = AutoFacHelper.LoggerGet();
            foreach (var key in registry.Keys)
            {
                if(asSelf)
                {
                    logger.Trace(t => t("Registering \"{0}\" as AsSelf.InstancePerRequest", key));
                    builder.RegisterType(registry[key])
                           .AsSelf()
                           .InstancePerRequest();
                }
                else
                {
                    logger.Trace(t => t("Registering \"{0}\" as InstancePerRequest", key));
                    builder.RegisterType(registry[key])
                           .As(key)
                           .InstancePerRequest();
                }
            }
        }

        public static void PerLifetimeScopeRegistrations(this ContainerBuilder builder, Dictionary<Type, Type> registry,
                                                         bool asSelf)
        {
            if (registry == null)
            {
                return;
            }
            var logger = AutoFacHelper.LoggerGet();
            foreach (var key in registry.Keys)
            {
                if(asSelf)
                {
                    logger.Trace(t => t("Registering \"{0}\" as AsSelf.InstancePerLifetimeScope", key));
                    builder.RegisterType(registry[key])
                           .AsSelf()
                           .InstancePerLifetimeScope();
                }
                else
                {
                    logger.Trace(t => t("Registering \"{0}\" as InstancePerLifetimeScope", key));
                    builder.RegisterType(registry[key])
                           .As(key)
                           .InstancePerLifetimeScope();
                }
            }
        }

        public static void PerDependencyRegistrations(this ContainerBuilder builder, Dictionary<Type, Type> registry, bool asSelf)
        {
            if (registry == null)
            {
                return;
            }
            var logger = AutoFacHelper.LoggerGet();
            foreach (var key in registry.Keys)
            {
                if(asSelf)
                {
                    logger.Trace(t => t("Registering \"{0}\" as AsSelf", key));
                    builder.RegisterType(registry[key])
                           .AsSelf();
                }
                else
                {
                    logger.Trace(t => t("Registering \"{0}\" as InstancePerLifetimeScope", key));
                    builder.RegisterType(registry[key])
                           .As(key);
                }
            }
        }

        private static ILog LoggerGet()
        {
            var callingClass = new StackFrame(2).GetMethod().ReflectedType;
            var logger = LogManager.GetLogger(callingClass);
            return logger;
        }
    }
}