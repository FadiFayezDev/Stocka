using Application.Common.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bases
{
    public abstract class BaseHandler<T> : ResponseHandler where T : class
    {
        protected readonly T _services;
        protected T _repo => _services;  // Alias for compatibility
        protected readonly IMapper? _mapper;
        protected readonly IUnitOfWork? _work;

        public BaseHandler(IMapper? mapper, T services, IUnitOfWork? work = null)
        {
            _mapper = mapper;
            _services = services;
            _work = work;
        }

        public BaseHandler(IMapper? mapper, T services) : this(mapper, services, null)
        {
        }
    }
}