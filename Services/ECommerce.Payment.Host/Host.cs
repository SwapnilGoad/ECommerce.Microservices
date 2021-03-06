﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using Autofac;
using ECommerce.Common;
using ECommerce.Payment.Host.Modules;
using ECommerce.Services.Common.Configuration;
using log4net;
using MassTransit;

namespace ECommerce.Payment.Host
{
    public class Host
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Host));

        public Host()
        {
        }

        public void Run()
        {
            var waiter = new DependencyAwaiter();
            waiter.WaitForRabbit(Configuration.RabbitMqHost);

            var builder = new ContainerBuilder();

            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ConsumerModule>();

            var container = builder.Build();
            var bus = container.Resolve<IBusControl>();
            bus.Start();

            Logger.Info("Running Payment microservice.");
            Thread.Sleep(int.MaxValue);
        }
    }
}
