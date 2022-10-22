using AutoMapper;
using Domain.Tickets_Aggregate;
using Domain.Tickets_Aggregate.Inputs;
using Infrastructure.UOW;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Controllers.Requests;

namespace TicketSystem.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : BaseController
    {
        public UnitOfWork UOW { get; }
        public IMediator Mediator { get; }
        private readonly IMapper _mapper;

        public TicketController(UnitOfWork UOW, IMediator _mediatR, IMapper mapper)
        {
            this.UOW = UOW;
            Mediator = _mediatR;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateTicket")]
        public ActionResult CreateTicket([FromBody] CreateTicketRequest request)
        {
            return TryCatchLog(() =>
            {
                Ticket ticket = new();
                UOW.TicketsRepo.Add(ticket);
                CreateTicketInput input = _mapper.Map<CreateTicketInput>(request);
                ticket.CreateTicket(input);
                UOW.SaveChanges();
                return Ok(ticket);
            });
        }

        [HttpPost]
        [Route("FilterList")]
        public ActionResult GetTicketsList([FromBody] TicketsFilter filter)
        {
            return TryCatchLog(() =>
            {
                var tickets = UOW.TicketsRepo.getList(filter);
                return Ok(tickets);
            });
        }

        [HttpPost]
        [Route("HandleTicket")]
        public ActionResult HandleTicket([FromBody] long TicketId)
        {
            Ticket ticket = UOW.TicketsRepo.GetTicketById(TicketId);
            ticket.HandleTicket();
            UOW.SaveChanges();
            return Ok();
        }

    }

}
