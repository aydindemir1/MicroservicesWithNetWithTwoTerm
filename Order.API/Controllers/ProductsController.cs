using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Categories;
using Order.Application.Products.Commands.Create;
using Order.Application.Products.Queries.GetAll;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        //create category
        [HttpPost("createCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand request)
        {
            
            return Ok(await mediator.Send(request));
        }

        //create product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand request)
        {
            await mediator.Send(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            await mediator.Send(new GetAllProductsQuery());
            return Ok();
        }
    }
}
