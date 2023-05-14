using NUnit.Framework;
using PubApi.Bar.Models;
using PubApi.Bar.Services;

namespace PubApi.Bar.UnitTests
{
    public class AddBarRequestValidatorTests
    {
        AddBarRequestValidator _sut = new AddBarRequestValidator();
            
        private const string validName = "test";
        private const string validAddress = "Hogwarts";

        [TestCase(validName, validAddress, true)]
        [TestCase(null, validAddress, false)]
        [TestCase("", validAddress, false)]
        [TestCase(validName, null, false)]
        [TestCase(validName, "", false)]
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(string name, string address, bool PassesValidation)        
        {

            var request = new AddABarRequest()
            {
                Name = name,
                Address = address
            };

            var validationResponse = _sut.Validate(request);


            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}