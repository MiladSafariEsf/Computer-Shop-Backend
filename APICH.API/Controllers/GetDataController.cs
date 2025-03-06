using APICH.API.Models;
using APICH.API.Security;
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
    public class GetDataController : ControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;

        public GetDataController(IReviewService reviewService,IProductService productService,ICategoryService categoryService, IOrderService orderService, JwtService jwt)
        {
            this.reviewService = reviewService;
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.jwt = jwt;
        }
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await productService.GetById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        [HttpGet("GetImageByPath")]
        public IActionResult GetImageByPath(string filePath)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image");
            var fullPath = Path.GetFullPath(Path.Combine(rootPath, filePath));

            // جلوگیری از حملات Path Traversal
            if (!fullPath.StartsWith(rootPath))
            {
                return BadRequest("Invalid file path.");
            }

            // بررسی وجود فایل
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            // بررسی فرمت‌های مجاز
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(fullPath).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type.");
            }

            // تعیین نوع محتوا
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // باز کردن فایل با مدیریت بهینه حافظه
            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return File(fileStream, contentType);
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct(int PageNumber)
        {
            var model = await productService.GetAll(PageNumber);
            return Ok(model);
        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            return Ok(await productService.Search(search));
        }   
        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder(int PageNumber)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var OrderL = await orderService.GetAllOrders(PageNumber);
            //var orderListModel = new List<GetOrderModel>();
            //foreach (var Item in OrderL)
            //{
            //    var Order = new GetOrderModel()
            //    {
            //        Id = Item.Id,
            //        UserName = Item.User.UserName,
            //        //ProductId = Item.ProductId,
            //        UserNumber = Item.UserNumber,
            //        //ProductName = Item.Product.Name,
            //        //ProductNumber = Item.Number,
            //    };
            //    orderListModel.Add(Order);
            //}
            return Ok(OrderL);
        }
        [HttpGet("GetAllDeliveredOrder")]
        public async Task<IActionResult> GetAllDeliveredOrder(int PageNumber)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");

            var OrderL = await orderService.GetAllDeliveredOrders(PageNumber);
            //var orderListModel = new List<GetOrderModel>();
            //foreach (var Item in OrderL)
            //{
            //    var Order = new GetOrderModel()
            //    {
            //        Id = Item.Id,
            //        UserName = Item.User.UserName,
            //        //ProductId = Item.ProductId,
            //        UserNumber = Item.UserNumber,
            //        //ProductName = Item.Product.Name,
            //        //ProductNumber = Item.Number,
            //    };
            //    orderListModel.Add(Order);
            //}
            return Ok(OrderL);
        }
        [HttpGet("GetOrderCount")]
        public async Task<IActionResult> GetOrderCount()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await orderService.GetOrderCount());
        }
        [HttpGet("GetDeliveredOrderCount")]
        public async Task<IActionResult> GetDeliveredOrderCount()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await orderService.GetDeliveredOrderCount());
        }
        [HttpGet("GetProductCount")]
        public async Task<IActionResult> GetProductCount()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await productService.GetProductCount());
        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            return Ok(await categoryService.GetCategories());
        }
        [HttpGet("GetCategoryByPageNumber")]
        public async Task<IActionResult> GetCategoryByPageNumber(int PageNumber) 
        {
            return Ok(await categoryService.GetCategoryCountByPageNumber(PageNumber));
        }
        [HttpGet("GetCategoryCount")]
        public async Task<IActionResult> GetCategoryCount()
        {
            return Ok(await categoryService.GetCategoryCount());
        }
        [HttpGet("GetRewiewByProductId")]
        public async Task<IActionResult> GetRewiewByProductId(Guid ProductId)
        {
            return Ok(await reviewService.GetReviewByProductId(ProductId));
        }
        [HttpGet("GetAllDataOfProductById")]
        public async Task<IActionResult> GetAllDataOfProductById(Guid Id) 
        {
            return Ok(await productService.GetAllDataOfProductById(Id));
        }
    }
}
