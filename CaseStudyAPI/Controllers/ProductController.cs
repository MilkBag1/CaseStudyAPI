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
    public class ProductController : ControllerBase
    {
        readonly AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        [Route("{brandid}")]
        public async Task<ActionResult<List<Product>>> Index(int brandid)
        {
            ProductDAO dao = new(_db);
            List<Product> itemsForBrand = await dao.GetAllByBrand(brandid);
            return itemsForBrand;
        }

    }
}
