# microservices-communication-playground

> RabbitMQ is installed locally.

Inside UserService/Controllers/UsersController we have different endpoints for

---

### Publish

> Send a message to all listeners

### Send

> Send a message to a particular endpoint

### Request/Response

> Send a message, process it and return the altered message

### Exception

> Handles Exception with Faults

---

### Saga Orchestration

> The flow for **OrderStateMachine** consits of
>
> - creating an order _OrderCreatedEvent_
> - check if product is available _ProductReservedEvent_ or _ProductNotReservedEvent_
> - proceed to _ShipmentEvent_
