using Application.Bases;
using Application.Dtos;
using Domain.Entities.Products;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Warehouse.GetWarehouseTypes
{
    public class GetAllWarehouseTypesQuery : IRequest<Response<IEnumerable<WarehouseTypeDto>>>
    {
    }

    public class GetAllWarehouseTypesQueryHandler : IRequestHandler<GetAllWarehouseTypesQuery, Response<IEnumerable<WarehouseTypeDto>>>
    {
        public async Task<Response<IEnumerable<WarehouseTypeDto>>> Handle(GetAllWarehouseTypesQuery request, CancellationToken cancellationToken)
        {
            var types = Enum.GetValues(typeof(WarehouseType))
                .Cast<WarehouseType>()
                .Select(t => new WarehouseTypeDto
                {
                    Id = (int)t,
                    Name = t.ToString()
                })
                .AsEnumerable();
            return new ResponseHandler().Success(types);
        }
    }
}