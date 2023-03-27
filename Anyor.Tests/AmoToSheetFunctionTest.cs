using AmoToSheetFunc;
using Api.Common;
using Api.Controllers;
using Api.Services;

namespace Anyor.Tests;

public class AmoToSheetFunctionTest
{
    private readonly AmoService _amoService;

    public AmoToSheetFunctionTest()
    {
        var envConfig = new EnvironmentConfig();
        _amoService = new AmoService(envConfig.AmoTokenConfig.AccessToken);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AllFunctionTest()
    {
        var controller = new PreachyBudgetController();
        var s =
            "{\"httpMethod\":\"POST\",\"headers\":{\"Accept-Encoding\":\"gzip\",\"Content-Length\":\"301\",\"Content-Type\":\"application/x-www-form-urlencoded\",\"Host\":\"functions.yandexcloud.net\",\"Uber-Trace-Id\":\"103e4354b4218eca:08fef0f886999d00:103e4354b4218eca:1\",\"User-Agent\":\"amoCRM-Webhooks/3.0\",\"X-Amocrm-Requestid\":\"4deb1687-f69f-4f06-930e-95c2947134ca\",\"X-Forwarded-For\":\"88.212.240.28\",\"X-Real-Remote-Address\":\"[88.212.240.28]:48088\",\"X-Request-Id\":\"5640fce3-02e1-4281-b770-8b1a61bf4bbc\",\"X-Trace-Id\":\"be5f5e10-dcbb-469c-b8d7-d62b006bf0b0\"},\"url\":\"\",\"params\":{},\"multiValueParams\":{},\"pathParams\":{},\"multiValueHeaders\":{\"Accept-Encoding\":[\"gzip\"],\"Content-Length\":[\"301\"],\"Content-Type\":[\"application/x-www-form-urlencoded\"],\"Host\":[\"functions.yandexcloud.net\"],\"Uber-Trace-Id\":[\"103e4354b4218eca:08fef0f886999d00:103e4354b4218eca:1\"],\"User-Agent\":[\"amoCRM-Webhooks/3.0\"],\"X-Amocrm-Requestid\":[\"4deb1687-f69f-4f06-930e-95c2947134ca\"],\"X-Forwarded-For\":[\"88.212.240.28\"],\"X-Real-Remote-Address\":[\"[88.212.240.28]:48088\"],\"X-Request-Id\":[\"5640fce3-02e1-4281-b770-8b1a61bf4bbc\"],\"X-Trace-Id\":[\"be5f5e10-dcbb-469c-b8d7-d62b006bf0b0\"]},\"queryStringParameters\":{},\"multiValueQueryStringParameters\":{},\"requestContext\":{\"identity\":{\"sourceIp\":\"88.212.240.28\",\"userAgent\":\"amoCRM-Webhooks/3.0\"},\"httpMethod\":\"POST\",\"requestId\":\"5640fce3-02e1-4281-b770-8b1a61bf4bbc\",\"requestTime\":\"27/Mar/2023:14:33:56 +0000\",\"requestTimeEpoch\":1679927636},\"body\":\"bGVhZHMlNUJzdGF0dXMlNUQlNUIwJTVEJTVCaWQlNUQ9MTA3NTQxNDcmbGVhZHMlNUJzdGF0dXMlNUQlNUIwJTVEJTVCc3RhdHVzX2lkJTVEPTU0NDIzOTIyJmxlYWRzJTVCc3RhdHVzJTVEJTVCMCU1RCU1QnBpcGVsaW5lX2lkJTVEPTYzNDQyOTAmbGVhZHMlNUJzdGF0dXMlNUQlNUIwJTVEJTVCb2xkX3N0YXR1c19pZCU1RD01NDQzNTAxNCZsZWFkcyU1QnN0YXR1cyU1RCU1QjAlNUQlNUJvbGRfcGlwZWxpbmVfaWQlNUQ9NjM0NDI5MCZhY2NvdW50JTVCaWQlNUQ9MzAzMDc1NTgmYWNjb3VudCU1QnN1YmRvbWFpbiU1RD1hbnlvcg==\",\"isBase64Encoded\":true}";
        var response = controller.AmoHookHandle(s).Result;
        if (response is YaCloudFunctionResponse)
        {
            Assert.That(((YaCloudFunctionResponse)response).StatusCode, Is.EqualTo(200));
        }
    }

    [Test]
    public void GetLeadTest()
    {
        var lead = _amoService.GetAmoLead(9997167);
        Assert.That(lead.Embedded.Contacts.First().Id, Is.EqualTo(13180131));
    }
    
    [Test]
    public void GetContactTest()
    {
        var contact = _amoService.GetAmoContact(13183325);
        Assert.That(contact.Id, Is.EqualTo(13183325));
    }
}