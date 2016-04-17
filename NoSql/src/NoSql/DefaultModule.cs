using Autofac;
using Models;
using NoSql.Repositories;
using System;

namespace NoSql
{
    public class DefaultModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdFactory>().As<IdFactory>();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .As<IId>();
                

            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            //    .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces();

        }
    }
}
