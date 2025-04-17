using System.Security.Claims;
using APICH.API.Models.Get;
using APICH.API.Security;
using APICH.CORE.interfaces;
using APICH.CORE.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PersianDate.Standard;
using SFM.Security;
namespace APICH.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private readonly IBannerService banerService;
        private readonly IUserService userService;
        private readonly IReviewService reviewService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        private readonly JwtService jwt;

        public GetDataController(IBannerService banerService, IUserService userService, IReviewService reviewService, IProductService productService, ICategoryService categoryService, IOrderService orderService, JwtService jwt)
        {
            this.banerService = banerService;
            this.userService = userService;
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
        [HttpGet("GetAllAdminProduct")]
        public async Task<IActionResult> GetAllAdminProduct(int PageNumber)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin() && rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");
            var model = await productService.GetAllAdmin(PageNumber);
            return Ok(model);
        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            return Ok(await productService.Search(search));
        }
        [HttpGet("AdvancedSearchProduct")]
        public async Task<IActionResult> AdvancedSearchProduct(string? search, int? maxPrice, int? minPrice, Guid? category)
        {
            return Ok(await productService.AdvancedSearch(search, maxPrice, minPrice, category));
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
            if (rol != Role.Admin() && rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");

            var Orders = await orderService.GetAllOrders(PageNumber);
            var OrdersModel = Orders.Select(o => new GetOrderModel()
            {
                Id = o.Id,
                CreateAt = o.CreateAt.ToFa(),
                Details = o.OrderDetails.Select(d => new GetOrderDetailModel()
                {
                    ProductName = d.Product.Name,
                    Stock = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList(),
                UserName = o.User.UserName,
                UserNumber = o.User.Number,
                Address = o.User.Address,
                totalPrice = o.TotalPrice,
            }).ToList();
            return Ok(OrdersModel);
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
            if (rol != Role.Admin() && rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");

            var Orders = await orderService.GetAllDeliveredOrders(PageNumber);
            var OrdersModel = Orders.Select(o => new GetOrderModel()
            {
                Id = o.Id,
                CreateAt = o.CreateAt.ToFa(),
                Details = o.OrderDetails.Select(d => new GetOrderDetailModel()
                {
                    ProductName = d.Product.Name,
                    Stock = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList(),
                UserName = o.User.UserName,
                UserNumber = o.User.Number,
                Address = o.User.Address,
                totalPrice = o.TotalPrice,
            }).ToList();

            return Ok(OrdersModel);
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
            if (rol != Role.Admin() && rol != Role.Owner())
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
            if (rol != Role.Admin() && rol != Role.Owner())
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
            if (rol != Role.Admin() && rol != Role.Owner())
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
            string UserName = null;
            var token = Request.Cookies["AuthToken"];
            var Number = "0";
            if (token != null)
            {
                var t = (await jwt.ValidateToken(token));
                Number = t.FindFirst(ClaimTypes.Name)?.Value ?? "";
                UserName = (await userService.GetByNumber(Number))?.UserName;
            }

            var AllData = await productService.GetAllDataOfProductById(Id);
            var AllComment = AllData.Reviews
                .Select(p => new GetAllDataOfProductComments
                {
                    Id = p.Id,
                    UserName = p.User.UserName,
                    Comment = p.Comment,
                    Rating = p.Rating,
                    IsOwner = p.UserNumber == Number,
                    date = p.CreateAt.ToFa(),
                })
                .ToList();
            var RC = await reviewService.ReviewCountByProductId(Id);
            var DataProduct = new GetAllDataOfProduct()
            {
                Id = Id,
                AverageRate = await reviewService.GetAverageRate(Id),
                Reviews = AllComment,
                CreateAt = AllData.CreateAt,
                Description = AllData.Description,
                ImageUrl = AllData.ImageUrl,
                Name = AllData.Name,
                Price = AllData.Price,
                Stock = AllData.Stock,
                ReviewCount = RC,
                UserName = UserName,
            };

            return Ok(DataProduct);
        }
        [HttpGet("GetBannerCountAdmin")]
        public async Task<IActionResult> GetBannerCountAdmin()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin() && rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await banerService.GetBanerCountAdmin());
        }
        [HttpGet("GetBannerCount")]
        public async Task<IActionResult> GetBannerCount()
        {
            return Ok(await banerService.GetBanerCount());
        }
        [HttpGet("GetBanerImageByPath")]
        public IActionResult GetBanerImageByPath(string filePath)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Baners");
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
        [HttpGet("GetAllBanners")]
        public async Task<IActionResult> GetAllBanners()
        {
            return Ok(await banerService.GetAllBaners());
        }
        [HttpGet("GetAllBannersAdmin")]
        public async Task<IActionResult> GetAllBannersAdmin()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Admin() && rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await banerService.GetAllBanersAdmin());
        }
        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;

            var Orders = await orderService.GetOrderByUserNumber(Number);

            var OrdersModel = Orders.Select(o => new GetOrderModel()
            {
                Id = o.Id,
                CreateAt = o.CreateAt.ToFa(),
                Details = o.OrderDetails.Select(d => new GetOrderDetailModel()
                {
                    ProductName = d.Product.Name,
                    Stock = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList(),
                UserName = o.User.UserName,
                UserNumber = o.User.Number,
                Address = o.User.Address,
                totalPrice = o.TotalPrice,
                IsDelivered = o.IsDelivered,
            }).ToList();
            return Ok(OrdersModel);
        }
        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(string Search, int PageNumber)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");

            var Users = await userService.SearchUser(Search, PageNumber);
            var UsersModel = new
            {
                Count = Users.Count,
                Users = Users.Select(User => new GetUserModel()
                {
                    Id = User.Id,
                    Number = User.Number,
                    Role = User.Role,
                    UserName = User.UserName,
                })
            };
            return Ok(UsersModel);
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser(int PageNumber)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");
            var Users = await userService.GetAllUser(PageNumber);
            var UsersModel = new
            {
                Count = Users.Count,
                Users = Users.Select(User => new GetUserModel()
                {
                    Id = User.Id,
                    Number = User.Number,
                    Role = User.Role,
                    UserName = User.UserName,
                })
            };
            return Ok(UsersModel);
        }
    }
}
