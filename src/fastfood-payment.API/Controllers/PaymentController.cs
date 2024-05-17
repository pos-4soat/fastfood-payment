using fastfood_payment.API.Controllers.Base;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.CreatePayment;
using fastfood_payment.Application.UseCases.ReceivePayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace fastfood_payment.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{ver:apiVersion}/[controller]")]
    public class PaymentController(IMediator _mediator) : BaseController
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Create payment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<CreatePaymentResponse>>))]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest createOrderRequest, CancellationToken cancellationToken)
        {
            Result<CreatePaymentResponse> result = await _mediator.Send(createOrderRequest, cancellationToken);
            return await GetResponseFromResult(result);
        }


        /// <summary>
        /// Process payment webhook
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderPayedRequestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{orderId}")]
        [SwaggerOperation(Summary = "Create payment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<Result<ReceivePaymentResponse>>))]
        public async Task<IActionResult> ReceiveOrderPayment([FromRoute] int orderId, [FromBody] ReceivePaymentRequest orderPayedRequestDto, CancellationToken cancellationToken)
        {
            orderPayedRequestDto.OrderId = orderId;
            Result<ReceivePaymentResponse> result = await _mediator.Send(orderPayedRequestDto, cancellationToken);
            return await GetResponseFromResult(result);
        }
    }
}
