using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Site.Models;

namespace YourApplication.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Read the site settings from the JSON file
            SiteSettings settings = null;
            try
            {
                string settingsJson = System.IO.File.ReadAllText("siteSettings.json");
                settings = JsonSerializer.Deserialize<SiteSettings>(settingsJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading site settings from JSON file");
            }

            // If the settings file doesn't exist or is empty, use the default settings
            if (settings == null)
            {
                settings = new SiteSettings
                {
                    CurrencySymbol = "€",
                    MonthStartDate = 1
                };
            }

            // Pass the settings to the view
            return View(settings);
        }

        [HttpPost]
        public IActionResult Index(SiteSettings settings)
        {
            // Validate the currency symbol
            if (!IsValidCurrencySymbol(settings.CurrencySymbol))
            {
                ModelState.AddModelError(nameof(settings.CurrencySymbol), "Invalid currency symbol");
            }

            // Validate the month start date
            if (settings.MonthStartDate < 1 || settings.MonthStartDate > 31)
            {
                ModelState.AddModelError(nameof(settings.MonthStartDate), "Invalid month start date");
            }

            // If there are validation errors, redisplay the form with error messages
            if (!ModelState.IsValid)
            {
                return View(settings);
            }

            // Save the settings to the JSON file
            try
            {
                string settingsJson = JsonSerializer.Serialize(settings);
                System.IO.File.WriteAllText("siteSettings.json", settingsJson);
                TempData["Message"] = "Settings saved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving site settings to JSON file");
                TempData["ErrorMessage"] = "Error saving settings";
            }

            // Redirect back to the index page
            return RedirectToAction(nameof(Index));
        }

        private bool IsValidCurrencySymbol(string currencySymbol)
        {
            // Allow any character
            if (currencySymbol.Length > 0 && currencySymbol.Length <= 3)
            {
                return true;
            }
            return false;
        }
    }
}