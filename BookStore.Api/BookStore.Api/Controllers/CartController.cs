using BookStore.Models.DataModels;
using BookStore.Models.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BookStore.Models.Models;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly CartRepository _cartRepository = new();

        [HttpGet]
        [Route("list")]
        public IActionResult GetCartItems( int UserId, int pageIndex = 1, int pageSize = 10)
        {
            ListResponse<Cart> carts = _cartRepository.GetCartItems(UserId, pageIndex, pageSize);
            ListResponse<GetCartModel> cartModels = new ListResponse<GetCartModel>()
            {
                Records = carts.Records.Select(c => new GetCartModel(c.Id, c.Userid, new BookModel(c.Book), c.Quantity)),
                TotalRecords = carts.TotalRecords
            };
            return Ok(cartModels);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<CartModel> AddCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new Cart()
            {
                Id = model.Id,
                Quantity = 1,
                Bookid = model.BookId,
                Userid = model.UserId
            };
            cart = _cartRepository.AddCart(cart);
            if(cart == null)
            {
                return StatusCode(500);
            }
            return new CartModel(cart);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new Cart()
            {
                Id = model.Id,
                Quantity = model.Quantity,
                Bookid = model.BookId,
                Userid = model.UserId
            };
            cart = _cartRepository.UpdateCart(cart);

            return Ok(new CartModel(cart));
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteCart(int id)
        {
            if (id == 0)
                return BadRequest();

            bool response = _cartRepository.DeleteCart(id);
            return Ok(response);
        }
    }
}
