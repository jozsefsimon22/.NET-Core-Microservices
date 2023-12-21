using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface ICouponService
{
    Task<ResponseDto?> GetCoupon(string couponCode);
    Task<ResponseDto?> GetAllCoupon();
    Task<ResponseDto?> GetCouponById(int couponCode);
    Task<ResponseDto?> CreateCoupon(CouponDto couponDto);
    Task<ResponseDto?> UpdateCoupons(CouponDto couponDto);
    Task<ResponseDto?> DeleteCouponsAsync(int id);
}