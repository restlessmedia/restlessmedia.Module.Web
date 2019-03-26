using restlessmedia.Module.Web.Api.Attributes;
using System;
using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public class TestimonialController : ApiControllerBase
  {
    public TestimonialController(IUIContext context, ITestimonialService testimonialService)
      : base(context)
    {
      _testimonialService = testimonialService ?? throw new ArgumentNullException(nameof(testimonialService));
    }

    [HttpGet]
    public ModelCollection<TestimonialEntity> List(int page)
    {
      return _testimonialService.List(page, 5);
    }

    [HttpGet]
    [ClientCache(1)]
    [Route("api/testimonial/random")]
    public TestimonialEntity Random()
    {
      return _testimonialService.Random();
    }

    private readonly ITestimonialService _testimonialService;
  }
}