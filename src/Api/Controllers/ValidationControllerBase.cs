using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public class ValidationController : ApiControllerBase
  {
    public ValidationController(IUIContext context)
      : base(context) { }

    /// <summary>
    /// Wraps the telephone number attribute routine
    /// </summary>
    /// <param name="TelephoneNumber"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/validate/phonenumber")]
    public virtual bool ValidatePhoneNumber(string number)
    {
      return ValidationHelper.IsValidTelephoneNumber(number);
    }

    /// <summary>
    /// Wraps the postcode attribute routine
    /// </summary>
    /// <param name="PostCode"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/validate/postcode")]
    public virtual bool ValidatePostCode(string postCode)
    {
      return ValidationHelper.IsValidPostCode(postCode);
    }
  }
}