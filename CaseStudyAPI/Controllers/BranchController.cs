using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        readonly AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }
        [AllowAnonymous]
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<List<Branch>?>> Index(float lat, float lon)
        {
            BranchDAO dao = new(_db);
            return await dao.GetThreeClosestBranches(lat, lon);
        }
    }
}