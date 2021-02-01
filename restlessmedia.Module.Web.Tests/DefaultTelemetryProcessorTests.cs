using FakeItEasy;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace restlessmedia.Module.Web.Tests
{
  public class DefaultTelemetryProcessorTests
  {
    public DefaultTelemetryProcessorTests()
    {
      _telemetryProcessor = A.Fake<ITelemetryProcessor>();
      _defaultTelemetryProcessor = new DefaultTelemetryProcessor(_telemetryProcessor);
    }

    [Fact]
    public void exceptions_are_logged()
    {
      ITelemetry telemetry = new ExceptionTelemetry
      {
        Exception = new Exception()
      };

      // call
      _defaultTelemetryProcessor.Process(telemetry);

      // assert
      A.CallTo(() => _telemetryProcessor.Process(telemetry))
        .MustHaveHappened();
    }

    [Fact]
    public void specific_exceptions_are_not_logged()
    {
      TestNoProcess(new ExceptionTelemetry
      {
        Exception = new HttpException()
      });

      TestNoProcess(new RequestTelemetry
      {
        ResponseCode = "401"
      });

      TestNoProcess(new RequestTelemetry
      {
        ResponseCode = "405"
      });
    }

    private void TestNoProcess<T>(T telemetry)
      where T : ITelemetry
    {
      // call
      _defaultTelemetryProcessor.Process(telemetry);

      // assert
      A.CallTo(() => _telemetryProcessor.Process(telemetry))
        .MustNotHaveHappened();
    }

    private class TestException : TaskCanceledException
    {
      public TestException(string stackTrace)
      {
        _stackTrace = stackTrace;
      }

      public override string StackTrace => _stackTrace;

      private readonly string _stackTrace;
    }

    private readonly ITelemetryProcessor _telemetryProcessor;

    private readonly DefaultTelemetryProcessor _defaultTelemetryProcessor;
  }
}
