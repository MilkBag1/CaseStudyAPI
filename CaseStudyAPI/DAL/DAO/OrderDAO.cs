using CaseStudyAPI.DAL.DomainClasses;
using CaseStudyAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyAPI.DAL.DAO
{
    public class OrderDAO
    {
        private readonly AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders!.Where(order => order.CustomerId == id).ToListAsync<Order>();
        }


        public async Task<int> AddOrder(int customerId, OrderSelectionHelper[] selections)
        {
            int orderId = -1;

            //transaction
            using(var _trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    Order order = new();
                    order.CustomerId = customerId;
                    order.OrderDate = DateTime.Now;
                    order.OrderAmount = 0;

                    //calculate the order total
                    foreach (OrderSelectionHelper selection in selections)
                    {
                       
                       order.OrderAmount += selection.Qty * selection.Item.MSRP;                         
                       
                    }
                    await _db.Orders!.AddAsync(order);
                    await _db.SaveChangesAsync();

                    //then OrderLineItems
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        OrderLineItem olItem = new();
                        olItem.ProductId = selection.Item!.Id;
                        olItem.OrderId = order.Id;
                        olItem.SellingPrice = selection.Item.MSRP;

                        //enough stock
                        if (selection.Qty <= selection.Item!.QtyOnHand)
                        {
                            
                            
                                var dbProduct = _db.Products!.Where(proudcut => proudcut.Id.Equals(selection.Item.Id)).FirstOrDefault();
                                dbProduct.QtyOnHand -= selection.Qty;
                                _db.SaveChanges();
                            
                            
                            olItem.QtySold = selection.Qty;
                            olItem.QtyOrdered = selection.Qty;
                            olItem.QtyBackOrdered = 0;
                            
                        }

                        //not enough stock
                        if (selection.Qty > selection.Item!.QtyOnHand)
                        {
                            
                            
                            var dbProduct = _db.Products!.Where(proudcut => proudcut.Id.Equals(selection.Item.Id)).FirstOrDefault();
                                dbProduct.QtyOnHand = 0;
                                dbProduct.QtyOnBackorder += selection.Qty - selection.Item.QtyOnHand;
                                _db.SaveChanges();
                            

                            olItem.QtySold = selection.Item.QtyOnHand;
                            olItem.QtyOrdered = selection.Qty;
                            olItem.QtyBackOrdered = selection.Qty - selection.Item.QtyOnHand;

                        }
                                                
                        await _db.OrderLineItems!.AddAsync(olItem);
                        await _db.SaveChangesAsync();
                    }
                    await _trans.CommitAsync();
                    orderId = order.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await _trans.RollbackAsync();
                }
            }
            return orderId;
        }

        public async Task<List<OrderDetailsHelper>> GetOrderDetails(int oid, string email)
        {
            Customer? customer = _db.Customers!.FirstOrDefault(user => user.Email == email);
            List<OrderDetailsHelper> allDetails = new();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oli in _db.OrderLineItems! on o.Id equals oli.OrderId
                          join p in _db.Products! on oli.ProductId equals p.Id
                          where (o.CustomerId == customer!.Id && o.Id == oid)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              CustomerId = o.CustomerId,
                              ProductId = oli.ProductId,
                              Description = p.Description,
                              Amount = p.MSRP,
                              QtyOrdered = oli.QtyOrdered,
                              QtySold = oli.QtySold,
                              QtyOnBackOrder = p.QtyOnBackorder,
                              ProductName = p.ProductName,
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = await results.ToListAsync();
            return allDetails;
        }

    }
}
