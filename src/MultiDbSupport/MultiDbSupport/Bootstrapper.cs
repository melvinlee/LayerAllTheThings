﻿namespace MultiDbSupport
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

    using MediatR;

    using Nancy;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            //base.ConfigureApplicationContainer(container); // Lets our app do all the wiring up

            switch (ConfigurationManager.ConnectionStrings["mydb"].ProviderName.ToLower())
            {
                case "system.data.sqlclient":
                    container.Register<IDbConnectionProvider, SqlServerConnectionProvider>();
                    break;
                case "npgsql":
                    container.Register<IDbConnectionProvider, PostgresConnectionProvider>();
                    break;
                default:
                    throw new ArgumentException("Invalid ProviderName in connection string.");
            }

            container.Register<IMediator>(new Mediator(container.Resolve, container.ResolveAll));
            container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>, UserListQueryRequestHandler>();
        }
    }
}
