using Microsoft.AspNetCore.Mvc;
using PubApi.Bar.Models;

namespace PubApi.Bar.Interfaces;

public interface IBarService
{
    Task<ObjectResult> AddABar(AddABarRequest request);   
    Task<ObjectResult> GetAllBars();
    Task<ObjectResult> UpdateABar(UpdateABarRequest request);
    Task<ObjectResult> GetBar(Guid id);
}
