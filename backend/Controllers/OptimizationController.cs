using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using backend.Data;
using backend.Models;
using System;
using System.Collections.Generic;

namespace GamsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptimizationController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly string gamsPath = "/usr/local/bin/gams"; // or exact installed path

        private readonly string modelPath = "/app/GamsModels/model.gms";
        private readonly string inputPath = "/app/GamsModels/input.inc";
        private readonly string resultPath = "/app/GamsModels/result.csv";


        public OptimizationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("run")]
        public IActionResult RunOptimization([FromBody] OptimizationParameters input)
        {
            try
            {
                // Write input values to GAMS input file
                System.IO.File.WriteAllText(inputPath,
    $@"budget = {input.Budget};
priceCar = {input.PriceCar};
priceTruck = {input.PriceTruck};
revenueCar = {input.RevenueCar};
revenueTruck = {input.RevenueTruck};
barnSpaces = {input.BarnSpaces};
truckSpaces = {input.TruckSpaces};
");

                // Run GAMS
              var psi = new ProcessStartInfo
{
    FileName = gamsPath,
    Arguments = $"\"{modelPath}\"",
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
};

using (var process = Process.Start(psi))
{
    if (process == null)
    {
        return StatusCode(500, "Failed to start GAMS process.");
    }

    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    Console.WriteLine("GAMS Output: " + output);
    Console.WriteLine("GAMS Error: " + error);
}



                if (!System.IO.File.Exists(resultPath))
                    return StatusCode(500, "GAMS result file not found.");

                // Parse result
                var resultLines = System.IO.File.ReadAllLines(resultPath);
                var result = resultLines.Select(line => line.Split(','))
                                        .ToDictionary(parts => parts[0].ToLower(), parts => double.Parse(parts[1]));

                var optimizationResult = new OptimizationResult
                {
                    Cars = (int)result["car"],
                    Trucks = (int)result["truck"],
                    Revenue = result["revenue"],
                    CreatedAt = DateTime.UtcNow
                };

                // Save to database
                _context.OptimizationResults.Add(optimizationResult);
                _context.SaveChanges();

                return Ok(new
                {
                    Cars = optimizationResult.Cars,
                    Trucks = optimizationResult.Trucks,
                    Revenue = optimizationResult.Revenue
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
