using AutoMapper;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Infrastructure.Order.Configuration;

public class OrderMapperConfiguration : Profile
{
    public OrderMapperConfiguration()
    {
        CreateMap<OrderCreateRequest, OrderEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<OrderEntity, OrderResponse>();

        CreateMap<OrderEntity, OrderPlacedMessage>();
    }
}