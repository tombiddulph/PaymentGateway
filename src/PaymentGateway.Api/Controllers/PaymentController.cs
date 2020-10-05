using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Models;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Api.Controllers
{
    [ApiController, Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        /// <summary>
        /// Creates a payment
        /// </summary>
        /// <remarks>
        ///  Sample Request:
        ///
        ///     POST /api/payment/create
        ///     {
        ///
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: 200, type: typeof(object))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            return Ok(new
            {
                hello = "from post"
            });
        }


        /// <summary>
        /// Gets a transaction by it's id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET /api/payment/transaction?id={id}
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>A transaction</returns>
        /// <response code="200">Returns the transaction</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If the transaction is not found</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(statusCode: 200, type: typeof(object))]
        public async Task<IActionResult> Transaction([FromQuery] TransactionRequest request)
        {
            return Ok(new
            {
                hello = "from get"
            });
        }
    }
}