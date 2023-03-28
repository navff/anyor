using AmoToSheetFunc;
using Api.Common;
using Api.Controllers;
using Api.Services;
using Microsoft.AspNetCore.Http;

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
            "leads%5Bstatus%5D%5B0%5D%5Bid%5D=10754147&leads%5Bstatus%5D%5B0%5D%5Bstatus_id%5D=54423922&leads%5Bstatus%5D%5B0%5D%5Bpipeline_id%5D=6344290&leads%5Bstatus%5D%5B0%5D%5Bold_status_id%5D=54435014&leads%5Bstatus%5D%5B0%5D%5Bold_pipeline_id%5D=6344290&account%5Bid%5D=30307558&account%5Bsubdomain%5D=anyor";
        
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