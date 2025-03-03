using APICH.API.Models;
using APICH.API.Security;
using APICH.BL.Services;
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
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;

        public EditDataController(IProductService productService, IOrderService orderService, JwtService jwt)
        {
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
                NewProduct.ImageUrl = OldProduct.ImageUrl;
            }
            //Complete Edit
            if (await productService.Update(NewProduct) == 1)
                return Ok("Edit was success full");
            return BadRequest("Edit Error");
        }
        //[HttpPut("EditProduct")]
        //public async Task<IActionResult> EditProduct(EditProductModel model)
        //{
        //    //validation admin
        //    var t = await jwt.ValidateToken(model.Token);
        //    if (t == null)
        //        return BadRequest("Null");

        //    var username = t.FindFirst(ClaimTypes.Name)?.Value;
        //    var role = t.FindFirst(ClaimTypes.Role)?.Value;
        //    if (role != Role.Admin())
        //        return BadRequest("Role Erorr");

        //    //Get Old Product
        //    var OldProduct = await productService.GetById(model.Id);
        //    var NewProduct = new Product();
        //    //If Image was't Null
        //    if (model.Image != null)
        //    {
        //        //delete proccess
        //        if (System.IO.File.Exists(OldProduct.ImageUrl))
        //            System.IO.File.Delete(OldProduct.ImageUrl);
        //        //edit proccess
        //        var ImageName = model.Id.ToString() + Path.GetExtension(model.Image.FileName);

        //        NewProduct.Id = model.Id;
        //        NewProduct.Name = model.Name;
        //        NewProduct.Price = model.Price;
        //        NewProduct.Description = model.Description;
        //        NewProduct.ImageUrl = "wwwroot/Image/" + ImageName;
        //        //create new file
        //        FileStream fileStream = new FileStream("wwwroot/Image/" + ImageName, FileMode.Create);
        //        await model.Image.CopyToAsync(fileStream);
        //        fileStream.Close();
        //    }

        //    //If Image was null
        //    else
        //    {
        //        NewProduct.Id = model.Id;
        //        NewProduct.Name = model.Name;
        //        NewProduct.Price = model.Price;
        //        NewProduct.Description = model.Description;
        //        NewProduct.ImageUrl = OldProduct.ImageUrl;
        //    }
        //    //Complete Edit
        //    if (await productService.Update(NewProduct) == 1)
        //        return Ok("Edit was success full");
        //    return BadRequest("Edit Error");
        //}
    }
}
