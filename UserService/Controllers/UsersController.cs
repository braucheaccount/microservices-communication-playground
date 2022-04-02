using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLogic;
using SharedLogic.Models;
using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Contracts.v1.Requests;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBus _bus;
        private readonly ISendEndpoint _sendEndpoint;
        private readonly IRequestClient<RequestRequest> _requestClient;
        private readonly IRequestClient<OrderCreatedEvent> _sagaRequestClient;

        public UsersController(
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint,
            IBus bus,
            ISendEndpointProvider sendEndpointProvider,
            IRequestClient<RequestRequest> requestClient,
            IRequestClient<OrderCreatedEvent> sagaRequestClient
            )
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _bus = bus;
            _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new("queue:order.saga")).Result;
            _requestClient = requestClient;
            _sagaRequestClient = sagaRequestClient;
        }

        [HttpPost]
        [Route("saga")]
        public async Task<IActionResult> Purchase()
        {
            var newOrderId = Guid.Parse("7ef12325-13e1-48c3-bb2c-e3f4979c7649");
            var existingUserId = Guid.Parse("7ef12325-13e1-48c3-bb2c-e3f4979c1337");

            await _sendEndpoint.Send<IOrderCreatedEvent>(new
            {
                OrderId = newOrderId,
                UserId = existingUserId
            });

        

            return Ok();
        }

        [HttpPost]
        [Route("exception")]
        public async Task<IActionResult> Exception()
        {
            var newRequest = new ExceptionRequest
            {
                ThrowException = true,
            };

            await _publishEndpoint.Publish(newRequest);

            return Ok();
        }

        [HttpPost]
        [Route("requestResponse")]
        public async Task<IActionResult> RequestResponse()
        {
            var requestData = new RequestRequest
            {
                RequestMessage = "I am waiting for a response and expect a value of 10",
                BaseValue = 5
            };

            var res = _requestClient.Create(requestData);
            var response = await res.GetResponse<RequestResponse>();

            return Ok(response);
        }

        [HttpPost]
        [Route("publish")]
        public async Task<IActionResult> Publish()
        {
            var newRequest = new PubishRequest
            {
                PublishMessage = "I am published to as many endpoints that are listening",
            };

            // use the message type (PubishRequest) to distribute itself to however many consumers are out there
            await _publishEndpoint.Publish<PubishRequest>(newRequest);

            return Ok();
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> Send()
        {
            var newRequest = new SendRequest
            {
                SendMessage = "I am send to a particular endpoint",
            };

            var endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/queue:send-example-queue"));

            // send goes directly to one specific endpoint
            await endpoint.Send(newRequest);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var data = await _userRepository.ListUsersAsync();

            return Ok(data);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var data = await _userRepository.FindUserByIdAsync(id);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {
            var newUser = new User
            {
                Username = createUserRequest.Username,
                Purchases = new()
            };

            var data = await _userRepository.CreateUserAsync(newUser);

            return Ok(data);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            await _userRepository.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
