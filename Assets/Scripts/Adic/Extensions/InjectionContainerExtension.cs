namespace Assets.Scripts.Adic.Extensions
{
  using System;

  using MediaLab.Adic.Framework.Binding;
  using MediaLab.Adic.Framework.Container;

  public static class InjectionContainerExtension
  {
    public static IBindingFactory UpdateBind(this IInjectionContainer injectionContainer, Type type)
    {
      var listBindings = injectionContainer.GetBindingsFor(type);
      if (listBindings != null)
      {
        foreach (var binding in listBindings)
        {
          injectionContainer.Unbind(binding.Type);
        }
      }

      return injectionContainer.Bind(type);
    }

    public static IBindingFactory UpdateBind<T>(this IInjectionContainer injectionContainer)
    {
      var listBindings = injectionContainer.GetBindingsFor<T>();
      if (listBindings != null)
      {
        foreach (var binding in listBindings)
        {
          injectionContainer.Unbind(binding.Type);
        }
      }

      return injectionContainer.Bind<T>();
    }
  }
}
