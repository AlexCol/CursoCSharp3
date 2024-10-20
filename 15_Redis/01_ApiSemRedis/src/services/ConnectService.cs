using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_ApiSemRedis.src.repository;

namespace _01_ApiSemRedis.src.services;

public interface IConnectService {
  string TestConnect();
}

public class ConnectService : IConnectService {
  private readonly IConnectRepository _connectRepository;

  public ConnectService(IConnectRepository connectRepository) {
    _connectRepository = connectRepository;
  }

  public string TestConnect() {
    return _connectRepository.TestConnect();
  }
}
