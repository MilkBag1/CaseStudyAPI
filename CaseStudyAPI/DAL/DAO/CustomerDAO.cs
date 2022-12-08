using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyAPI.DAL.DAO
{
    public class CustomerDAO
    {
        private readonly AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<Customer> Register(Customer customer)
        {
            await _db.Customers!.AddAsync(customer);
            await _db.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer?> GetByEmail(string? email)
        {
            Customer? customer = await _db.Customers!.FirstOrDefaultAsync(u => u.Email == email);
            return customer;
        }
    }
}
