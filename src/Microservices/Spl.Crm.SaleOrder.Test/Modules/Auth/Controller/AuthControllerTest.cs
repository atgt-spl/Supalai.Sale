using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.Localization;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using Spl.Crm.SaleOrder.Cache.Redis.Service;
using Spl.Crm.SaleOrder.Modules.Auth.Controller;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;

namespace Spl.Crm.SaleOrder.Test.Modules.Auth.Controller;

public class AuthControllerTest
{
    private AuthController _controller;
    private Mock<IAuthService> _iAuthServiceMock;
    private Mock<IAppLogger> _iAppLoggerMock;
    private Mock<IStringLocalizer<LocalizeResource>> _iLocalizerMock;
    private Mock<IUserCacheService> _userCacheServiceMock;
    
    public AuthControllerTest() {
        //setup
        _iAuthServiceMock = new Mock<IAuthService>();
        _iAppLoggerMock = new Mock<IAppLogger>();
        _iLocalizerMock = new Mock<IStringLocalizer<LocalizeResource>>();
        _userCacheServiceMock = new Mock<IUserCacheService>();
        _controller = new AuthController(_iAppLoggerMock.Object,_iLocalizerMock.Object,_iAuthServiceMock.Object,_userCacheServiceMock.Object);
    }


    [Fact]
    public void Test_Case_Authentication_Success()
    {
        //arrange
        UserInfo userInfo =  new UserInfo()
        {
            firstname = "supachai",
            lastname = "supachaisri",
            email = "spl@gmail.com",
            user_id = "user01",
            username = "supachai",
            role_name = new string[]{"admin"}
        };
        LoginResponse loginResponse = new LoginResponse();
        loginResponse.token = "token1234567890";
        loginResponse.refresh_token = "refresh_token1234567890";
        loginResponse.user_info = userInfo;
        var  response = new BaseResponse(new StatusResponse("0", "success"), loginResponse);
        _iAuthServiceMock.Setup(p => p.Login(It.IsAny<LoginRequest>())).Returns(response);

        //act
        var result = _controller.Login(new LoginRequest()
        {
            username = "supachai",
            password = "password"
        });

        //assert
        Assert.NotNull(result);
        var okObjectResult = result as OkObjectResult;
        Assert.IsType<BaseResponse>(okObjectResult?.Value);
        
        var responseModel = okObjectResult?.Value as BaseResponse;
        Assert.Equal(200, okObjectResult?.StatusCode);
        Assert.NotNull(responseModel);
       
        var data = responseModel?.data as LoginResponse;
        Assert.NotNull(data);
        Assert.Equal("token1234567890", data?.token);
        Assert.Equal("refresh_token1234567890", data?.refresh_token);
        Assert.NotNull(data?.user_info);
    }
    
    [Fact]
    public void Test_Case_RefreshToken_Success()
    {
        //arrange
        
        var httpContext = new DefaultHttpContext();
        httpContext.Items["TokenInfo"] = new TokenInfo()
        {
            username = "supachai"
        };
        _controller.ControllerContext.HttpContext = httpContext;
        RefreshTokenResponse refreshTokenResponse = new RefreshTokenResponse()
        {
            token = "token1234567890",
            refresh_token = "refreshToken1234567890"
        };
        var  response = new BaseResponse(new StatusResponse("0", "success"), refreshTokenResponse);
        _iAuthServiceMock.Setup(p => p.RefreshToken(It.IsAny<TokenInfo>())).Returns(response);
        
        //act
        var result = _controller.refreshToken();

        //assert
        Assert.NotNull(result);
        var okObjectResult = result as OkObjectResult;
        Assert.IsType<BaseResponse>(okObjectResult?.Value);
        
        var responseModel = okObjectResult?.Value as BaseResponse;
        Assert.Equal(200, okObjectResult?.StatusCode);
        Assert.NotNull(responseModel);
       
        var data = responseModel?.data as RefreshTokenResponse;
        Assert.NotNull(data);
        Assert.Equal("token1234567890", data?.token);
        Assert.Equal("refreshToken1234567890", data?.refresh_token);
    }
}