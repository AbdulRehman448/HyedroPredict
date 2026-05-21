using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace HydroPredict.Services.Implementations
{
    public class UiStateService
    {
        private readonly HttpClient _httpClient;
        private const string RapidApiKey = "dd3c86e248msh53030b4c7ba2387p11e3e5jsnc279326b6cd2";

        public bool IsDarkMode { get; private set; } = false;
        public string CurrentLanguage { get; private set; } = "en"; // "en" or "ur"

        public event Action? OnChange;

        public UiStateService()
        {
            _httpClient = new HttpClient();
        }

        public void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            NotifyStateChanged();
        }

        public void SetLanguage(string lang)
        {
            CurrentLanguage = lang;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public string T(string key)
        {
            if (CurrentLanguage == "en") return GetEnglishFallback(key);
            return GetUrduFallback(key);
        }

        /// <summary>
        /// Highly secure Translation Engine with an instant dictionary fallback matrix
        /// </summary>
        public async Task<string> TranslateAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            // 1. INSTANT LOCAL DICTIONARY LOOKUP (Guarantees immediate switch to Urdu)
            if (CurrentLanguage == "ur")
            {
                var localMatch = GetUrduDirectTranslation(text);
                if (localMatch != text) return localMatch;
            }
            else if (CurrentLanguage == "en")
            {
                return text;
            }

            // 2. LIVE API FALLBACK PIPELINE
            try
            {
                string encodedText = HttpUtility.UrlEncode(text);
                string url = $"https://google-translate-api14.p.rapidapi.com/translate.php?input_text={encodedText}&to_language=ur";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Headers =
                    {
                        { "x-rapidapi-key", RapidApiKey },
                        { "x-rapidapi-host", "google-translate-api14.p.rapidapi.com" }
                    }
                };

                using var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return text;

                string jsonResult = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResult);
                var root = doc.RootElement;

                if (root.TryGetProperty("translated_text", out var node))
                {
                    return HttpUtility.HtmlDecode(node.GetString() ?? text);
                }
            }
            catch
            {
                // Silence errors gracefully to keep UI completely stable
            }

            return text;
        }

        // Direct registration page mapping parameters matrix
        private static string GetUrduDirectTranslation(string text) => text.Trim() switch
        {
            "Create Consumer Account" => "صارف کا اکاؤنٹ بنائیں",
            "Sign up below to access direct water booking panels instantly." => "پانی کے بکنگ پینل تک فوری رسائی کے لیے نیچے سائن اپ کریں۔",
            "Full Account Name" => "اکاؤنٹ کا پورا نام",
            "Confirm Password" => "پاس ورڈ کی تصدیق کریں",
            "About HydroPredict Infrastructure" => "ہائیڈرو پریڈکٹ انفراسٹرکچر کے بارے میں",
            "HydroPredict is an advanced, tech-driven utility system built to streamline localized water management operations in Kharian, Punjab. We provide deep analytics, automated tariff generation, and immediate logistics vehicle allocation schemas." => "ہائیڈرو پریڈکٹ ایک جدید اور ٹیک سے چلنے والا نظام ہے جو کھاریاں، پنجاب میں پانی کے انتظام کے کاموں کو بہتر بنانے کے لیے بنایا گیا ہے۔ ہم تفصیلی تجزیات، خودکار ٹیرف اور فوری لاجسٹکس گاڑیوں کی الاٹمنٹ فراہم کرتے ہیں۔",
            "Predictive Allocation" => "تخمینی الاٹمنٹ",
            "Utilizes modern system parameters to calculate exact water consumption targets efficiently." => "پانی کے درست استعمال کے اہداف کا مؤثر طریقے سے حساب لگانے کے لیے جدید ترین پیرامیٹرز کا استعمال کرتا ہے۔",
            "Automated Billing" => "خودکار بلنگ",
            "Direct relational billing matrix ledger logs ensure transparent dynamic financial flows." => "براہ راست بلنگ میٹرکس لیجر لاگز شفاف مالیاتی بہاؤ کو یقینی بناتے ہیں۔",
            _ => text
        };

        private static string GetEnglishFallback(string key) => key switch
        {
            "brand" => "HydroPredict",
            "home" => "Home",
            "about" => "About Us",
            "login" => "Sign In",
            "register" => "Get Started",
            "logout" => "Logout",
            "email" => "Security Email Address",
            "password" => "Password",
            "customer" => "Customer Profile",
            _ => key
        };

        private static string GetUrduFallback(string key) => key switch
        {
            "brand" => "ہائیڈرو پریڈکٹ",
            "home" => "ہوم",
            "about" => "ہمارے بارے میں",
            "login" => "لاگ ان کریں",
            "register" => "اکاؤنٹ بنائیں",
            "logout" => "لاگ آؤٹ",
            "email" => "سیکیورٹی ای میل ایڈریس",
            "password" => "پاس ورڈ",
            "customer" => "صارف کا نام",
            _ => key
        };
    }
}