using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests {
    public class PageLinkTagHelperTests {
        [Fact]
        public void Can_Generate_Page_Links() {
            // Arrange
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(h => h.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");
            
            Mock<IUrlHelperFactory> urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(m => m.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);

            PageLinkTagHelper target = new PageLinkTagHelper(urlHelperFactory.Object) {
                PageModel = new PagingInfo {
                    TotalItems = 28,
                    ItemsPerPage = 10,
                    CurrentPage = 2
                },
                PageAction = "Test"
            };

            // Empty object. Not used in target.
            TagHelperContext ctx = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), String.Empty);

            Mock<TagHelperContent> content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));

            // Act
            target.Process(ctx, output);
            
            // Assert
            string result = output.Content.GetContent();
            Assert.Equal(@"<a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a>", result);
        }
    }
}