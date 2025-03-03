using APICH.API.Models;
using APICH.API.Security;
using APICH.BL.Services;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using SFM.Security;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APICH.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddDataController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IReviewService reviewService;
        private readonly ICategoryService categoryService;
        private readonly JwtService jwt;

        public AddDataController(IUserService userService,
            IProductService productService,
            IOrderService orderService,
            IReviewService reviewService,
            ICategoryService categoryService,
            JwtService jwt) 
        {
            this.userService = userService;
            this.productService = productService;
            this.orderService = orderService;
            this.reviewService = reviewService;
            this.categoryService = categoryService;
            this.jwt = jwt;
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequestModel model)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var i = Guid.NewGuid();
            var ImageName = i.ToString() + Path.GetExtension(model.Image.FileName);
            var product = new Product()
            {
                Id = i,
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = "wwwroot/Image/" + ImageName,
            };
            FileStream fileStream = new FileStream("wwwroot/Image/" + ImageName, FileMode.Create);
            await model.Image.CopyToAsync(fileStream);
            fileStream.Close();
            if (await productService.Add(product) == 0)
                return BadRequest("Oh No");
            return Ok("Add Product Success full");
        }

        [HttpPost("AddOrders")]
        public async Task<IActionResult> AddOrders(List<OrderModel> orderModels)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var number = t.FindFirst(ClaimTypes.Name)?.Value;
            var role = t.FindFirst(ClaimTypes.Role)?.Value;
            if (role != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            if (orderModels == null || orderModels.Count == 0)
                return BadRequest("The order list cannot be empty.");
            await userService.GetByNumber(number);
            var user = await userService.GetByNumber(number);
            //test
            //var number = "09136801391";
            //var user = await userService.GetByNumber(number);
            var OrderDetails = new List<OrderDetails>();
            foreach (var item in orderModels)
            {
                var p = await productService.GetById(item.ProductId);
                var Detail = new OrderDetails()
                {
                    Id = Guid.NewGuid(),
                    Product = p,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    UnitPrice = p.Price
                };
                OrderDetails.Add(Detail);
            }
            var Order = new Orders()
            {
                Id = Guid.NewGuid(),
                CreateAt = DateTime.Now,
                IsDelivered = false,
                OrderDetails = OrderDetails,
                TotalPrice = OrderDetails.Sum(p => p.UnitPrice),
                User = user,
                UserNumber = number
            };
            var hoohoo = (await orderService.AddOrder(Order));
            if (hoohoo == 0)
                return BadRequest("Oh No!");
            return Ok("Mission complited!!");
        }
        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(ReviewModel reviews)
        {
            //var token = Request.Cookies["AuthToken"];
            //var t = await jwt.ValidateToken(token);
            //if (t == null)
            //    return Unauthorized("Invalid or expired token.");
            //var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var User = await userService.GetByNumber("09136801391");
            //if (User == null)
            //{
            //    return BadRequest("Empty");
            //}
            var Product = await productService.GetById(reviews.ProductId);
            var Review = new Reviews()
            {
                Id = Guid.NewGuid(),
                Comment = reviews.Comment,
                CreateAt = DateTime.Now,
                Rating = reviews.Rating,
                //User = User,
                Product = Product
            };
            if (await reviewService.AddReview(Review) != 0)
            {
                return Ok();
            }
            return BadRequest("There are Problem in Adding");
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryModel categoryModel)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");
            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var User = await userService.GetByNumber(Number);
            if (User == null)
            {
                return BadRequest("Empty");
            }
            var Category = new Categories()
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryModel.CategoryName
            };
            if (await categoryService.AddCategory(Category) != 0)
            {
                return Ok();
            }
            return BadRequest("There are Problem in Adding");
        }
    }
}
