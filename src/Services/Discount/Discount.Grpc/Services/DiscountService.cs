using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null) throw new RpcException(new Status(StatusCode.InvalidArgument,"Invalid request object passed."));
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount successfully created for product: {productName}", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null) throw new RpcException(new Status(StatusCode.InvalidArgument,"Invalid request object passed."));
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount successfully updated for product: {productName}", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = dbContext.Coupons.FirstOrDefault(x => x.ProductName == request.ProductName);
        if(coupon == null) throw new RpcException(new Status(StatusCode.NotFound, "Product does not exist."));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount successfully deleted for product: {productName}", coupon.ProductName);
        return new() { Success = true };
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if(coupon == null) throw new RpcException(new Status(StatusCode.NotFound, "Product does not exist."));
        logger.LogInformation("Discount successfully retrieved for product: {productName}", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }
}