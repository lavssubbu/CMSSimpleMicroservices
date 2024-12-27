using CartAPI.Models.DTO;
using CartAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace CartAPI.Controllers
{
    [Route("api/Carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly CartContext _context;

        public CartsController(CartContext context)
        {
            _context = context;
        }

        // GET: api/Cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound();

            var cartDto = new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
            };

            return Ok(cartDto);
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(AddtoCartDto addToCartDto)
        {
            try
            {
                // Retrieve the cart for the user
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == addToCartDto.UserId);

                if (cart == null)
                {
                    // If the cart does not exist, create it and initialize the Items collection
                    cart = new Cart
                    {
                        UserId = addToCartDto.UserId,
                        Items = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                }

                // Ensure the Items collection is not null
                cart.Items ??= new List<CartItem>();

                // Check if the product already exists in the cart
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == addToCartDto.ProductId);

                if (existingItem != null)
                {
                    // Update the quantity of the existing product
                    existingItem.Quantity += addToCartDto.Quantity;
                }
                else
                {
                    // Add the new product to the cart
                    cart.Items.Add(new CartItem
                    {
                        ProductId = addToCartDto.ProductId,
                        Quantity = addToCartDto.Quantity
                    });
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok("Item added to cart successfully.");
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Cart/{userId}/items/{productId}
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound();

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound();

            cart.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cart/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound();

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
