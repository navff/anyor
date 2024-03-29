﻿using System.Web;
using AmoToSheetFunc;
using AmoToSheetFunc.Dtos;
using Api.Common;
using Api.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PreachyBudgetController: ControllerBase
{
    private readonly AmoService _amoService;
    private readonly EnvironmentConfig _envConfig;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public PreachyBudgetController(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
        _envConfig = new EnvironmentConfig();
        _amoService = new AmoService(_envConfig.AmoTokenConfig.AccessToken);
    }
    
    [HttpPost("AmoHookHandle")]
    public async Task<IActionResult> AmoHookHandle()
    {
        if (!Request.Body.CanRead) return BadRequest();
        
        var streamReader = new StreamReader(Request.Body);
        string request = await streamReader.ReadToEndAsync();

        _backgroundJobClient.Enqueue(() => AmoHookHandle(request));
        return Ok();
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> AmoHookHandle(string request)
    {
        var hook = ParseHook(request);

        Console.WriteLine("LEAD_ID: " + hook.LeadId);

        var lead = await _amoService.GetAmoLead(hook.LeadId);
        var contact = _amoService.GetAmoContact(lead.Embedded.Contacts.First().Id);
        Console.WriteLine("CONTACT_NAME: " + contact.Name);

        var amount = lead.CustomFieldsValues
            .FirstOrDefault(f => f.FieldName == "tinkoff_amount")?.Values
            .FirstOrDefault()?.Value;
        
        var sheetDonationService = new SheetDonationsAddService(_envConfig.GoogleCredsJson);
        sheetDonationService.AddNewRow(new Donation
        {
            AmoLeadId = lead.Id,
            Amount = amount,
            Date = DateTime.Now.AddHours(3),
            ContactName = contact.Name
        });  

        return Ok();
    }
    
    
    private AmoLeadStatusHook ParseHook(string valueToConvert)
    {
        var unescaped = Uri.UnescapeDataString(valueToConvert);
        var valueCollection = HttpUtility.ParseQueryString(unescaped);

        return new AmoLeadStatusHook
        {
            LeadId = long.Parse(valueCollection["leads[status][0][id]"]),
            NewPipelineId = long.Parse(valueCollection["leads[status][0][pipeline_id]"]),
            NewStatusId = long.Parse(valueCollection["leads[status][0][status_id]"]),
            OldPipelineId = long.Parse(valueCollection["leads[status][0][old_pipeline_id]"]),
            OldStatusId = long.Parse(valueCollection["leads[status][0][old_status_id]"])
        };
    }
}