using NUnit.Framework;
using PubApi.Brewery.Models;
using PubApi.Brewery.Services;
using System;

namespace PubApi.Brewery.UnitTests
{
    public class UpdateBreweryRequestValidatorTests
    {
        UpdateBreweryRequestValidator _sut = new UpdateBreweryRequestValidator();

        private const string validName = "test";   
        private const string validId = "e42dfd75-d238-43d5-9002-1a199c46f6bd";

        [TestCase(validId, validName, true)]
        [TestCase(validId, null, false)]
        [TestCase(validId, "", false)]     
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(Guid id, string name, bool PassesValidation)
        {

            var request = new UpdateABreweryRequest()
            {
                Id = id,
                Name = name
            };

            var validationResponse = _sut.Validate(request);


            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}