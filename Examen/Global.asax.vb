Imports System.Web.Optimization
Imports Autofac
Imports Autofac.Integration.Mvc
Imports Examen.DAL

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()

        Dim builder As New ContainerBuilder()

        builder.RegisterControllers(GetType(MvcApplication).Assembly)

        builder.RegisterType(Of ClienteRepository)().As(Of IClienteRepository)() _
               .WithParameter(New TypedParameter(GetType(String), ApplicationDbContext.ConnectionString()))

        builder.RegisterType(Of ProductoRepository)().As(Of IProductoRepository)() _
               .WithParameter(New TypedParameter(GetType(String), ApplicationDbContext.ConnectionString()))

        builder.RegisterType(Of VentaRepository)().As(Of IVentaRepository)() _
               .WithParameter(New TypedParameter(GetType(String), ApplicationDbContext.ConnectionString()))

        Dim container As IContainer = builder.Build()

        DependencyResolver.SetResolver(New AutofacDependencyResolver(container))

        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub
End Class
