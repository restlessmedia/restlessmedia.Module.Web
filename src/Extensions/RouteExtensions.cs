namespace System.Web.Http.Routing
{
  public static class RouteExtensions
  {
    public static void MapHttpRoute<T>(this HttpRouteCollection routes, string name, string action, string routeTemplate)
      where T : ApiController
    {
      Type controllerType = typeof(T);
      string controller = controllerType.Name; // TODO: remove the Api?
      routes.MapHttpRoute(
          name: name,
          routeTemplate: routeTemplate,
          defaults: new { controller, action }
      );
    }
  }
}