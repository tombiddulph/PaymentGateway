using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Services;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Models.Enums;

#pragma warning disable 8509

namespace PaymentGateway.Api.Controllers
{
    [ApiController, Route("api/[controller]/"), Authorize]
    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault")]
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
        ///     POST /api/payment/
        ///     {
        ///         "name": "Tom"
        ///         "cardNumber": "1234567890123456",
        ///         "expiryMonth" : "20",
        ///         "expiryYear": "22",
        ///         "Amount" : "12.21",
        ///         "cvv": "4135"
        ///     }    
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="201">Returns the created payment</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">If An unknown error occurs</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 422)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            var user = HttpContext.User.GetLoggedInUserId();
            var transaction = await _transactionService.CreateTransactionAsync(request, user);


            return transaction.Status switch
            {
                PaymentStatus.Success => new CreatedResult(
                    $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/api/payment/{transaction.Id}",
                    new CreatePaymentResponse
                    {
                        // ReSharper disable once PossibleInvalidOperationException
                        Id = transaction.Id.Value,
                        Status = transaction.Status
                    }),
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
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If the transaction is not found</response>
        /// <response code="422">Bad request</response>
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(statusCode: 200, type: typeof(GetTransactionRequest))]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 422)]
        public async Task<IActionResult> Transaction([FromRoute] GetTransactionRequest request)
        {
            var user = HttpContext.User.GetLoggedInUserId();
            var transaction = await _transactionService.GetById(request.Id.GetValueOrDefault(), user);

            return transaction.Status switch
            {
                PaymentStatus.Success => new OkObjectResult(transaction),
                PaymentStatus.Failure => new BadRequestResult(),
                PaymentStatus.NotFound => new NotFoundObjectResult($"Transaction {request.Id} not found "),
            };
        }
    }
}