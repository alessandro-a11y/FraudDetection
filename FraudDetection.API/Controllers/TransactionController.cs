using FraudDetection.Application.DTOs;
using FraudDetection.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FraudDetection.API.Controllers;

[ApiController]
[Route("api/v1/transaction")]  // versionado
[Authorize]                    // JWT obrigatório
public class TransactionController : ControllerBase
{
    private readonly CreateTransactionUseCase _useCase;

    public TransactionController(CreateTransactionUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var result = await _useCase.ExecuteAsync(dto);
        return Ok(result);
    }
}