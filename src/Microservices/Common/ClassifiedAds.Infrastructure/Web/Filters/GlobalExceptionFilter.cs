﻿using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ClassifiedAds.Infrastructure.Logging;

namespace ClassifiedAds.Infrastructure.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IAppLogger _logger;
        public GlobalExceptionFilter(IAppLogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                string code = string.Empty;
                string msg = string.Empty;
                int httpStatus;
                switch (context.Exception)
                {
                    case AuthenicationErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.userName)
                            ? e.message : string.Format("[{0}]{1}", e.userName, e.message);
                        httpStatus = e.httpStatus;
                        break;
                    case ClientErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;
                        break;
                    case ValidationErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.massageDetailInValid)
                            ? e.message : string.Format("{0} The problem is {1}", e.message, e.massageDetailInValid);
                        httpStatus = e.httpStatus;
                        break;
                    case TokenErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;
                        break;
                    case ExternalErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.partnerName)
                            ? e.message : string.Format("[{0}]{1}", e.partnerName, e.message);
                        httpStatus = e.httpStatus;
                        break;
                    case SystemErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;

                        if (e.exception != null)
                        {
                            _logger.Error(e.exception.Message);
                        }

                        break;
                    default:
                        code = ResponseData.SYSTEM_ERROR.Code;
                        msg = ResponseData.SYSTEM_ERROR.Message;
                        httpStatus = ResponseData.SYSTEM_ERROR.HttpStatus;
                        _logger.Error(context.Exception.Message);
                        break;
                }

                context.Result = new ObjectResult(new BaseResponse(new StatusResponse(code, msg)))
                {
                    StatusCode = httpStatus
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
