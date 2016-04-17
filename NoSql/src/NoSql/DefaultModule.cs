using Autofac;

namespace NoSql
{
    public class DefaultModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            //    .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces();

        }
    }
}
