using AutoMapper;
using Domain.Tickets_Aggregate.Inputs;
using TicketSystem.Controllers.Requests;

namespace TicketSystem.Controllers.Mappers
{
    public class TicketMapper : Profile
    {
        public TicketMapper()
        {
            _ = CreateMap<CreateTicketRequest, CreateTicketInput>();

        }

    }
}
