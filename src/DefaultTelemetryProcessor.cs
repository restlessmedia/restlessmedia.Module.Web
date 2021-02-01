using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Threading.Tasks;
using System.Web;

namespace restlessmedia.Module.Web
{
  /// <summary>
  /// Filters out unwanted insights
  /// </summary>
  public class DefaultTelemetryProcessor : ITelemetryProcessor
  {
    public DefaultTelemetryProcessor(ITelemetryProcessor next)
    {
      _next = next;
    }

    public void Process(ITelemetry item)
    {
      // To filter out an item, just return
      if (!DoProcess(item))
      {
        return;
      }

      _next.Process(item);
    }

    private bool DoProcess(ITelemetry item)
    {
      // for bots
      if (!string.IsNullOrEmpty(item.Context.Operation.SyntheticSource))
      {
        return false;
      }
      
      if (CanCastAs(item, out ExceptionTelemetry exceptionTelemetry))
      {
        // don't log httpexceptions
        if (exceptionTelemetry.Exception is HttpException)
        {
          return false;
        } 

        // don't log task cancellation exceptions
        // these are typically when a client has disconnected from an async request
        if (exceptionTelemetry.Exception is TaskCanceledException || exceptionTelemetry.Exception is OperationCanceledException)
        {
          return false;
        }
      }

      if (CanCastAs(item, out RequestTelemetry requestTelemetry))
      {
        switch (requestTelemetry.ResponseCode)
        {
          // 404 (not found) & 405 (method not allowed)
          case "401":
          case "405":
            {
              return false;
            }
        }
      }

      // allow everything else
      return true;
    }

    private static bool CanCastAs<T>(ITelemetry telemetry, out T value)
      where T : class, ITelemetry
    {
      value = telemetry as T;
      return value != null;
    }

    private readonly ITelemetryProcessor _next;
  }
}