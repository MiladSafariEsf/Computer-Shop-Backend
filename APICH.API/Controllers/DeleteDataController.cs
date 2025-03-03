using APICH.API.Models;
using APICH.API.Security;
using APICH.BL.Services;
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
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;
        public DeleteDataController(IProductService productService, IOrderService orderService, JwtService jwt)
        {
            this.productService = productService;
            this.orderService = orderService;
            this.jwt = jwt;
        }
        [HttpDelete("RemoveProductById")]
        public async Task<IActionResult> RemoveProductById(DeleteModel deleteModel)
        {
            var product = await productService.GetById(deleteModel.Id);
            if (deleteModel.Id == Guid.Empty)
            {
                return BadRequest("Empty");
            }
            if (product == null)
            {
                return BadRequest("Null");
            }
            var user = await jwt.ValidateToken(deleteModel.Token);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.FindFirst(ClaimTypes.Role)?.Value == Role.Admin())
            {
                if (await productService.DeleteById(deleteModel.Id) == 1)
                {
                    if (System.IO.File.Exists(product.ImageUrl))
                    {
                        System.IO.File.Delete(product.ImageUrl);
                    }
                    return Ok("Remove was success full");
                }
                return BadRequest("moshkel rokh dad");
            }
            else
                return BadRequest("admin nistid");
        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid ProductId)
        {
            if (await productService.DeleteById(ProductId) == 1)
                return Ok("The desired product was successfully deleted.");
            return BadRequest("Delete encountered an error.");
        }
    }
}
