﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    /// <summary>
    /// Proxy design pattern
    /// </summary>
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            // we only want to learn whether it is true or false we do not want to get data from cache.
            //_ prevents memory allocation of data because we are just checking
            // we cannot use await in constructor but we are using asyncron method so we added .Result
            if(!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                   _memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            await CacheAllProductsAsync();
            return await _repository.AnyAsync(expression);
            
        }

        public  Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            return Task.FromResult(products);
            
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey)
               .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            }

            return Task.FromResult(product);
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync());
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto));
        }

        public Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto)
        {
            throw new NotImplementedException();
        }

        Task<CustomResponseDto<ProductDto>> IService<Product, ProductDto>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<CustomResponseDto<IEnumerable<ProductDto>>> IService<Product, ProductDto>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<CustomResponseDto<IEnumerable<ProductDto>>> IService<Product, ProductDto>.Where(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        Task<CustomResponseDto<bool>> IService<Product, ProductDto>.AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<ProductDto>> AddAsync(ProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<IEnumerable<ProductDto>>> AddRangeAsync(IEnumerable<ProductDto> dtos)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<NoContentDto>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        Task<CustomResponseDto<IEnumerable<ProductWithCategoryDto>>> IProductService.GetProductsWithCategory()
        {
            throw new NotImplementedException();
        }
    }
}
