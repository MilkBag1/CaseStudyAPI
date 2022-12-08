using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CaseStudyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        readonly AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Brand>>> Index()
        {
            BrandDAO dao = new(_db);
            List<Brand> allCategories = await dao.GetAll();
            return allCategories;
        }
    }
}
