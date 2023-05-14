using NUnit.Framework;
using PubApi.Brewery.Models;
using PubApi.Brewery.Services;

namespace PubApi.Brewery.UnitTests
{
    public class AddBreweryRequestValidatorTests
    {
        AddBreweryRequestValidator _sut = new AddBreweryRequestValidator();

        private const string validName = "test";       

        [TestCase(validName, true)]
        [TestCase(null, false)]
        [TestCase("", false)]       
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(string name, bool PassesValidation)
        {

            var request = new AddABreweryRequest()
            {
                Name = name
            };

            var validationResponse = _sut.Validate(request);


            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}