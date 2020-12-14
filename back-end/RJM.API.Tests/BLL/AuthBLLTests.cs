using RJM.API.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RJM.API.Tests.BLL
{
    public class AuthBLLTests
    {
        [Fact]
        public void Login_ShouldReturnAUserAndToken()
        {
            // Arrange
            LoginVM loginVM = new LoginVM() {
                EmailOrUsername = "steff",
                Password = "SECRET"
            };

            // Act

            // Assert
        }

        [Theory]
        [InlineData("steff", "Steff12345!")]
        [InlineData("steff", "WrongPassword")]
        public void Login_WithParameters(
            string emailOrUsername,
            string password
        )
        {
            // Arrange
            LoginVM loginVM = new LoginVM()
            {
                EmailOrUsername = emailOrUsername,
                Password = password
            };

            // Act

            // Assert
        }
    }
}
