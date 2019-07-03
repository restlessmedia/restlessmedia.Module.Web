using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace restlessmedia.Module.Web
{
  [Obsolete("This should be moved to api module.")]
  internal class AssembliesResolver : IAssembliesResolver
  {
    public AssembliesResolver(ICollection<Assembly> assemblies = null)
    {
      _assemblies = assemblies ?? new List<Assembly>(0);
    }

    public ICollection<Assembly> GetAssemblies()
    {
      return _assemblies;
    }

    public void RegisterAssembly(Assembly assembly)
    {
      _assemblies.Add(assembly);
    }

    private readonly ICollection<Assembly> _assemblies;
  }
}
