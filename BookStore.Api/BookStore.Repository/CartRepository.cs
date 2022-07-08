using BookStore.Models.DataModels;
using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Repository
{
    public class CartRepository : BaseRepository
    {
        public ListResponse<Cart> GetCartItems(int UserId, int pageIndex, int pageSize)
        {
            //var query = _context.Carts.Include(c => c.Book).Where(c => c.Userid = UserId).AsQueryable
            var query = _context.Carts.Where(c => c.Userid == UserId).AsQueryable();
            return new ListResponse<Cart>()
            {
                Records = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Include(c => c.Book).ToList(),
                TotalRecords = query.Count(),
            };
        }

        public Cart GetCarts(int id)
        {
            return _context.Carts.FirstOrDefault(c => c.Id == id);
        }

        public Cart AddCart(Cart cart)
        {
            try
            {
                var cartInDb = _context.Carts.FirstOrDefault(c => c.Userid == cart.Userid && c.Bookid == cart.Bookid);

                if (cartInDb == null)
                {
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    return cart;
                }
                else
                {
                    throw new Exception($"book {cart.Bookid} already exists in the cart for user {cart.Userid}. Update the quantity of existing item in the cart.");
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Cart UpdateCart(Cart category)
        {
            var entry = _context.Carts.Update(category);
            _context.SaveChanges();
            return entry.Entity;
        }

        public bool DeleteCart(int id)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.Id == id);
            if (cart == null)
                return false;

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return true;
        }
    }
}
