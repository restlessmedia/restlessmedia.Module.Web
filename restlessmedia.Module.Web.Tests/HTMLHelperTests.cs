using Microsoft.VisualStudio.TestTools.UnitTesting;
using restlessmedia.Test;

namespace restlessmedia.Module.Web.Tests
{
  [TestClass]
  public class HTMLHelperTests
  {
    [TestMethod]
    public void HasText_returns_true_when_html_contains_text_elements()
    {
      string html = "<span>d</span>";
      HTMLHelper.ContainsInnerText(html).MustBeTrue();
    }

    [TestMethod]
    public void HasText_returns_false_when_html_does_not_contain_text_elements()
    {
      string html = "<span></span>";
      HTMLHelper.ContainsInnerText(html).MustBeFalse();
    }

    [TestMethod]
    public void HasText_returns_true_when_nested_html_contains_text_elements()
    {
      string html = "<span><span>d</span></span>";
      HTMLHelper.ContainsInnerText(html).MustBeTrue();
    }

    [TestMethod]
    public void HasText_returns_true_when_html_with_attributes_contains_text_elements()
    {
      string html = "<span><span class=\"text\">d</span></span>";
      HTMLHelper.ContainsInnerText(html).MustBeTrue();
    }

    [TestMethod]
    public void HasText_returns_true_when_html_contains_text_with_self_closing_tags()
    {
      string html = "<span class=\"text\">d</span><br />";
      HTMLHelper.ContainsInnerText(html).MustBeTrue();
    }

    [TestMethod]
    public void HasText_returns_true_when_html_with_self_closing_tags_does_not_contain_text()
    {
      string html = "<span></span><br />";
      HTMLHelper.ContainsInnerText(html).MustBeFalse();
    }

    [TestMethod]
    public void ToText_returns_text_stripping_out_tags()
    {
      string html = "<strong>a test</strong>";
      HTMLHelper.ToText(html).MustBe("a test");
    }

    [TestMethod]
    public void ToText_returns_text_stripping_out_tags_when_nested()
    {
      string html = "<strong>a test <span>nested</span></strong>";
      HTMLHelper.ToText(html).MustBe("a test nested");
    }
  }
}
