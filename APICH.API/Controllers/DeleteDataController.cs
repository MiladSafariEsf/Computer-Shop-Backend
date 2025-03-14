using APICH.API.Models;
using APICH.API.Security;
using APICH.BL.Services.Classes;
using APICH.BL.Services.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SFM.Security;
using System.Security.Claims;
namespace APICH.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeleteDataController : ControllerBase
    {
        private readonly IBannerService bannerService;
        private readonly IReviewService reviewService;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;
        public DeleteDataController(IBannerService bannerService, IReviewService reviewService, ICategoryService categoryService, IProductService productService, IOrderService orderService, JwtService jwt)
        {
            this.bannerService = bannerService;
            this.reviewService = reviewService;
            this.categoryService = categoryService;
            this.productService = productService;
            this.orderService = orderService;
            this.jwt = jwt;
        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid ProductId)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var product = await productService.GetById(ProductId);
            if (await productService.DeleteById(ProductId) == 1)
            {
                if (System.IO.File.Exists(product.ImageUrl))
                {
                    System.IO.File.Delete(product.ImageUrl);
                }
                return Ok("Remove was success full");
            }
            return BadRequest("moshkel rokh dad");
            //if (await productService.DeleteById(ProductId) == 1)
            //    return Ok("The desired product was successfully deleted.");
            //return BadRequest("Delete encountered an error.");
        }
        [HttpDelete("DeleteCategoryById")]
        public async Task<IActionResult> DeleteCategoryById(Guid CategoryId)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");
            if (await categoryService.DeleteCategoryById(CategoryId) == 1)
                return Ok("Remove was success full");
            return BadRequest();
        }
        [HttpDelete("DeleteReviewById")]
        public async Task<IActionResult> DeleteReviewById(Guid ReviewId)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;

            var Review = await reviewService.GetReviewById(ReviewId);
            if (Review.UserNumber == Number)
            {
                return Ok(await reviewService.DeleteReview(ReviewId));
            }
            return BadRequest();
        }
        [HttpDelete("DeleteBannerById")]
        public async Task<IActionResult> DeleteBannerById(Guid BannerId)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var Banner = await bannerService.GetBannerById(BannerId);
            if (await bannerService.DeleteBanerById(BannerId) == 1)
            {
                if (System.IO.File.Exists(Banner.BanerImageUrl))
                {
                    System.IO.File.Delete(Banner.BanerImageUrl);
                }
                return Ok("Remove was success full");
            }
            return BadRequest("moshkel rokh dad");
        }
    }
}
