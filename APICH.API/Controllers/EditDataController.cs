using APICH.API.Models.Edit;
using APICH.API.Security;
using APICH.BL.Services.interfaces;
using APICH.CORE.Entity;
using Microsoft.AspNetCore.Mvc;
using SFM.Security;
using System.Security.Claims;

namespace APICH.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EditDataController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;

        public EditDataController(ICategoryService categoryService , IProductService productService, IOrderService orderService, JwtService jwt)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.orderService = orderService;
            this.jwt = jwt;
        }
        [HttpPut("DeliverOrder")]
        public async Task<IActionResult> DeliverOrder(Guid OrderId)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            return Ok(await orderService.DeliverOrderById(OrderId));
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Guid ProductId, ProductEditModel product)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");
            //Get Old Product
            var OldProduct = await productService.GetById(ProductId);
            var NewProduct = new Product();
            //If Image was't Null
            if (product.image != null)
            {
                //delete proccess
                if (System.IO.File.Exists(OldProduct.ImageUrl))
                    System.IO.File.Delete(OldProduct.ImageUrl);
                //edit proccess
                var ImageName = ProductId.ToString() + Path.GetExtension(product.image.FileName);

                NewProduct.Id = ProductId;
                NewProduct.Name = product.name;
                NewProduct.Price = product.price;
                NewProduct.Description = product.description;
                NewProduct.Stock = product.Stock;
                NewProduct.ImageUrl = "wwwroot/Image/" + ImageName;
                //create new file
                FileStream fileStream = new FileStream("wwwroot/Image/" + ImageName, FileMode.Create);
                await product.image.CopyToAsync(fileStream);
                fileStream.Close();
            }

            //If Image was null
            else
            {
                NewProduct.Id = ProductId;
                NewProduct.Name = product.name;
                NewProduct.Price = product.price;
                NewProduct.Description = product.description;
                NewProduct.Stock = product.Stock;
                NewProduct.ImageUrl = OldProduct.ImageUrl;
            }
            //Complete Edit
            if (await productService.Update(NewProduct) != 0)
                return Ok("Edit was success full");
            return BadRequest("Edit Error");
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Guid CategoryId, CategoryEditModel Category)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var cat = new Categories()
            {
                Id = CategoryId,
                CategoryName = Category.CategoryName,
            };
            return Ok(await categoryService.EditCategory(cat));
        }
    }
}
