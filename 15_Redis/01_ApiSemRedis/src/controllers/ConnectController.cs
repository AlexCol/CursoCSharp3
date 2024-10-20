using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_ApiSemRedis.src.services;
using Microsoft.AspNetCore.Mvc;

namespace _01_ApiSemRedis.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectController : ControllerBase {
  private readonly IConnectService _connectService;

  public ConnectController(IConnectService connectService) {
    _connectService = connectService;
  }

  [HttpGet("test")]
  public IActionResult Test() {
    try {
      return Ok(_connectService.TestConnect());
    } catch (Exception e) {
      return BadRequest(e.Message);
    }
  }
}
