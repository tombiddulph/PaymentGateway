using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Services;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Models.Domain;
using PaymentGateway.Models.Enums;

namespace PaymentGateway.Api.Controllers
{
    [ApiController, Route("api/[controller]/")]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public PaymentController(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        /// <summary>
        /// Creates a payment
        /// </summary>
        /// <remarks>
        ///  Sample Request:
        ///
        ///     POST /api/payment/create
        ///     {
        ///         "cardNumber": "1234567890123456",
        ///         "expiryMonth" : "20",
        ///         "expiryYear": "22",
        ///         "Amount" : "12.21",
        ///         "cvv": "4135"
        ///     }    
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: 200, type: typeof(Transaction))]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            var transaction = _transactionService.CreateTransaction(request);

            return transaction.Status switch
            {
                PaymentStatus.Success => new CreatedResult(
                    $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/api/payment/{transaction.Id}",
                    transaction),
                PaymentStatus.Failure => new UnprocessableEntityResult(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        /// <summary>
        /// Gets a transaction by it's id
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET /api/payment/{id}
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>A transaction</returns>
        /// <response code="200">Returns the transaction</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">If the transaction is not found</response>
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(statusCode: 200, type: typeof(object))]
        public async Task<IActionResult> Transaction([FromRoute] TransactionRequest request)
        {
            var transaction = _transactionService.GetById(request.Id.GetValueOrDefault());


            return transaction.Status switch
            {
                PaymentStatus.Success => new OkObjectResult(transaction),
                PaymentStatus.Failure => new BadRequestResult(),
                _ => new NotFoundResult()
            };
        }
    }
}