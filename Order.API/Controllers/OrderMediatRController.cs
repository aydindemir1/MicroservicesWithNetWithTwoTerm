using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Order.Commands.Create;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMediatRController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateCommand orderCreateCommand)
        {
            var result = await mediator.Send(orderCreateCommand);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new Application.Order.Queries.GetOrderByIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
