using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;

    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> GetCoupon(string couponCode)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Url = $"{SD.CouponAPIBase}/api/coupon/GetByCode/{couponCode}"
        });
    }

    public async Task<ResponseDto?> GetAllCoupon()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Url = $"{SD.CouponAPIBase}/api/coupon"
        });
    }

    public async Task<ResponseDto?> GetCouponById(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Url = $"{SD.CouponAPIBase}/api/coupon/{id}"
        });
    }

    public async Task<ResponseDto?> CreateCoupon(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Url = $"{SD.CouponAPIBase}/api/coupon",
            Data = couponDto
        });
    }

    public async Task<ResponseDto?> UpdateCoupons(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Url = $"{SD.CouponAPIBase}/api/coupon",
            Data = couponDto
        });
    }

    public async Task<ResponseDto?> DeleteCouponsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{SD.CouponAPIBase}/api/coupon/{id}"
        });
    }
}