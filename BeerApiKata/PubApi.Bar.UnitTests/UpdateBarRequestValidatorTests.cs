using NUnit.Framework;
using PubApi.Bar.Models;
using PubApi.Bar.Services;
using System;

namespace PubApi.Bar.UnitTests
{
    public class UpdateBarRequestValidatorTests
    {
        UpdateBarRequestValidator _sut = new UpdateBarRequestValidator();
            
        private const string validName = "test";
        private const string validPercentageAlcoholByVolume = "0.5";
        private const string validId = "e42dfd75-d238-43d5-9002-1a199c46f6bd";

        [TestCase(validId, validName, validPercentageAlcoholByVolume, true)]
        [TestCase(validId, null, validPercentageAlcoholByVolume, false)]
        [TestCase(validId, "", validPercentageAlcoholByVolume, false)]
        [TestCase(validId, validName, -1,false)]
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(Guid id,string name,decimal percentageAlcoholByVolume,bool PassesValidation)        {

            var request = new UpdateABarRequest()
            {
                Id = id,
                Name = name,
                PercentageAlcoholByVolume = percentageAlcoholByVolume
            };

            var validationResponse = _sut.Validate(request);


            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}