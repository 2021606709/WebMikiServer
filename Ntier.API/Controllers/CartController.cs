﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ntier.DAL.Context;
using Ntier.DAL.Entities;
using Ntier.DTO.DTO.Products;

namespace Ntier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ShopContext _shopContext;
        private readonly IConfiguration _configuration;
        public CartController(ShopContext shopContext, IConfiguration configuration)
        {
            _shopContext = shopContext;
            _configuration = configuration;
        }


        [HttpPost]

        public async Task<ActionResult> AddToCart(ProductDTO product, string userId)
        {
            try
            {
                var item = await _shopContext.CartDetails.FirstOrDefaultAsync(item => item.ProductId == product.Id);
                if (item != null)
                {
                    item.Quantity = item.Quantity + 1;
                    await _shopContext.SaveChangesAsync();
                }
                else
                {
                    //var cartDetail = new CartDetail
                    //{
                    //    CartId = cart.Id,
                    //    ProductId = product.Id,
                    //    Quantity = product.,
                    //    UserId = userId,
                    //    CreateAt = DateTime.Now.ToString(),
                    //};

                    //await _shopContext.CartDetails.AddAsync(cartDetail);
                }
                return Ok("Thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCartByUserID(string userID)
        {
            try
            {
                var cart = await _shopContext.CartDetails.Where(item => item.UserId == userID).GroupBy(item => item.ProductId).ToListAsync();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
