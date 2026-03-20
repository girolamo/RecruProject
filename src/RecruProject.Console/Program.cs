using Microsoft.Extensions.DependencyInjection;
using RecruProject.Console.Configuration;
using RecruProject.Core.Logger;
using RecruProject.Core.Models;
using RecruProject.Core.Services;

// Register
var services = new ServiceCollection();
services.Register();

// Build
var servicesProvider = services.BuildServiceProvider();
var logger = servicesProvider.GetRequiredService<ILogger>();
var orderService = servicesProvider.GetRequiredService<IOrderService>();

// Resolve
// Adding task using 'await' keyword - first approach
await orderService.AddOrderAsync(new Order { Id = 1, Description = "Laptop" });
await orderService.AddOrderAsync(new Order { Id = 2, Description = "Phone" });
await orderService.AddOrderAsync(new Order { Id = 2, Description = "Computer" }); // Force error

// Processing task using Task.WaitAll - second approach
var tasks = new Task[3];
tasks[0] = orderService.ProcessOrderAsync(1);
tasks[1] = orderService.ProcessOrderAsync(2);
tasks[2] = orderService.ProcessOrderAsync(-1);
Task.WaitAll(tasks);

logger.LogInfo("All orders processed.");

