using AutoMapper;
using RecruProject.API.DTOs;
using RecruProject.Core.Models;

namespace RecruProject.API.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderRequestDto, Order>();
        CreateMap<Order, OrderResponseDto>();
    }
}