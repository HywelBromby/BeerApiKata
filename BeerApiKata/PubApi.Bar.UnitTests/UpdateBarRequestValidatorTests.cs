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
        private const string validAddress = "Hogwarts";
        private const string validId = "e42dfd75-d238-43d5-9002-1a199c46f6bd";

        [TestCase(validId, validName, validAddress, true)]
        [TestCase(validId, null, validAddress, false)]
        [TestCase(validId, "", validAddress, false)]
        [TestCase(validId, validName, null,false)]
        [TestCase(validId, validName, "", false)]
        public void Given_ValidationIsCalled_Then_ItShouldPassOrFailBasedOnValues(Guid id,string name,string address,bool PassesValidation)        {

            var request = new UpdateABarRequest()
            {
                Id = id,
                Name = name,
                Address = address
            };

            var validationResponse = _sut.Validate(request);

            Assert.AreEqual(PassesValidation, validationResponse.IsValid);
        }
    }
}