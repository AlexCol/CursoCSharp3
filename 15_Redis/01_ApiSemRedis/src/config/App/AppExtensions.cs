using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_ApiSemRedis.src.config.App;

public static class AppExtensions {
  public static void AddDependencies(this WebApplication app) {
    if (app.Environment.IsDevelopment()) {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();
    app.MapControllers();
  }
}
