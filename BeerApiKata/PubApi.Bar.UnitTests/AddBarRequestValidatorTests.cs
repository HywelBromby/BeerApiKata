using NUnit.Framework;
using PubApi.Bar.Models;
using PubApi.Bar.Services;

namespace PubApi.Bar.UnitTests
{
    public class AddBarRequestValidatorTests
    {
        AddBarRequestValidator _sut = new AddBarRequestValidator();
            
        private const string validName = "test";
        private const string validPercentageAlcoholByVolume = "0.5";

        [TestCase(validName, validPercentageAlcoholByVolume, true)]
        [TestCase(null, validPercentageAlcoholByVolume, false)]
        [TestCase("", validPercentageAlcoholByVolume, false)]
        [TestCase(validName, "-1", false)]
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(string name, decimal percentageAlcoholByVolume, bool PassesValidation)        
        {

            var request = new AddABarRequest()
            {
                Name = name,
                PercentageAlcoholByVolume =percentageAlcoholByVolume
            };

            var validationResponse = _sut.Validate(request);


            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}