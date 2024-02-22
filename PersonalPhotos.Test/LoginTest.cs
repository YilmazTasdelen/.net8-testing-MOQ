using PersonalPhotos.Controllers;
using Moq;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;
using Core.Models;
namespace PersonalPhotos.Test
{
    public class LoginTest
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins;
        private readonly Mock<IHttpContextAccessor> _accessor;
        public LoginTest()
        {
            _logins = new Mock<ILogins>();
            _accessor = new Mock<IHttpContextAccessor>();
            _controller = new LoginsController(_logins.Object,_accessor.Object);
        }

        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginView1()
        {
            var result = _controller.Index();

            Assert.IsAssignableFrom<IActionResult>(result);

 
        }

        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginViewAsNameLogin()
        {

            var result = (_controller.Index() as ViewResult);

            Assert.NotNull(result); 

            Assert.Equal("Login",result.ViewName,ignoreCase:true);

        }


        [Fact]
        public async Task Login_GivenModelStateInvalid_ReturnLoginView()
        {

            _controller.ModelState.AddModelError("test","test");

            //var model = new Mock<LoginViewModel>();
            //var result = _controller.Login(model.Object);


            var result =await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;

            Assert.Equal("Login", result.ViewName, ignoreCase: true);

        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToDisplayAction()
        {

            const string password = "123";
            var modelView = Mock.Of<LoginViewModel>(x=>x.Email == "asdasd@asd.com" && x.Password == password);
            var model = Mock.Of<User>(x=>x.Password == password);

            _logins.Setup(x=>x.GetUser(model.Email)).ReturnsAsync(model);

            var result = await _controller.Login(modelView);

            Assert.IsType<RedirectToActionResult>(result);

        }

    }
}