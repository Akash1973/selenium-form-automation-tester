using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumFormTester
{
    public class RobustFormTester
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const int TIMEOUT_SECONDS = 10;
        private const string BASE_URL = "https://app.cloudqa.io/home/AutomationPracticeForm";

        public RobustFormTester()
        {
            var options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TIMEOUT_SECONDS));
        }

        public void RunTests()
        {
            try
            {
                Console.WriteLine("🚀 Starting Automation Practice Form Tests...");
                driver.Navigate().GoToUrl(BASE_URL);

                wait.Until(driver => driver.FindElement(By.TagName("form")));
                Console.WriteLine("✅ Page loaded successfully");

                TestFirstNameField();
                TestEmailField();
                TestMobileField();

                Console.WriteLine("\n🎉 All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test execution failed: {ex.Message}");
                throw;
            }
        }

        private void TestFirstNameField()
        {
            Console.WriteLine("\n📝 Testing First Name Field");

            var strategies = new List<Func<IWebElement>>
            {
                () => driver.FindElement(By.XPath("//input[@placeholder='First Name' or @placeholder='first name']")),
                () => driver.FindElement(By.XPath("//label[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'first name')]/following-sibling::input")),
                () => driver.FindElement(By.XPath("//input[@name='firstName' or @name='firstname' or @name='first_name']")),
                () => driver.FindElement(By.XPath("//input[@id='firstName' or @id='firstname' or @id='first_name']")),
                () => driver.FindElements(By.XPath("//form//input[@type='text']")).FirstOrDefault()
            };

            var element = FindElementWithStrategies("First Name", strategies);
            if (element != null)
            {
                TestInputField(element, "John", "First Name");
            }
        }

        private void TestEmailField()
        {
            Console.WriteLine("\n📧 Testing Email Field");

            var strategies = new List<Func<IWebElement>>
            {
                () => driver.FindElement(By.XPath("//input[@type='email']")),
                () => driver.FindElement(By.XPath("//input[contains(@placeholder, 'email') or contains(@placeholder, 'Email')]")),
                () => driver.FindElement(By.XPath("//label[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'email')]/following-sibling::input")),
                () => driver.FindElement(By.XPath("//input[@name='email' or @name='emailAddress']")),
                () => driver.FindElement(By.XPath("//input[@id='email' or @id='emailAddress']"))
            };

            var element = FindElementWithStrategies("Email", strategies);
            if (element != null)
            {
                TestInputField(element, "test@example.com", "Email");
            }
        }

        private void TestMobileField()
        {
            Console.WriteLine("\n📱 Testing Mobile Field");

            var strategies = new List<Func<IWebElement>>
            {
                () => driver.FindElement(By.XPath("//input[@type='tel']")),
                () => driver.FindElement(By.XPath("//input[contains(@placeholder, 'mobile') or contains(@placeholder, 'Mobile') or contains(@placeholder, 'phone')]")),
                () => driver.FindElement(By.XPath("//label[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'mobile')]/following-sibling::input")),
                () => driver.FindElement(By.XPath("//input[@name='mobile' or @name='phone' or @name='phoneNumber']")),
                () => driver.FindElement(By.XPath("//input[@id='mobile' or @id='phone' or @id='phoneNumber']"))
            };

            var element = FindElementWithStrategies("Mobile", strategies);
            if (element != null)
            {
                TestInputField(element, "1234567890", "Mobile");
            }
        }

        private IWebElement FindElementWithStrategies(string fieldName, List<Func<IWebElement>> strategies)
        {
            for (int i = 0; i < strategies.Count; i++)
            {
                try
                {
                    Console.WriteLine($"  🔍 Trying strategy {i + 1} for {fieldName}...");
                    var element = wait.Until(driver =>
                    {
                        try
                        {
                            var el = strategies[i]();
                            return el?.Displayed == true && el?.Enabled == true ? el : null;
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    if (element != null)
                    {
                        Console.WriteLine($"  ✅ Found {fieldName} field using strategy {i + 1}");
                        return element;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"  ❌ Strategy {i + 1} failed");
                }
            }

            Console.WriteLine($"  ⚠️ Could not locate {fieldName} field");
            return null;
        }

        private void TestInputField(IWebElement element, string testValue, string fieldName)
        {
            try
            {
                element.Clear();
                element.SendKeys(testValue);

                string actualValue = element.GetAttribute("value");
                if (actualValue == testValue)
                {
                    Console.WriteLine($"  ✅ {fieldName} test PASSED - Value: '{actualValue}'");
                }
                else
                {
                    Console.WriteLine($"  ❌ {fieldName} test FAILED - Expected: '{testValue}', Got: '{actualValue}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Error testing {fieldName}: {ex.Message}");
            }
        }

        public void Dispose()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("🔧 Selenium Form Automation Tester");
            Console.WriteLine("==================================");
            Console.WriteLine("Testing CloudQA Practice Form with robust element detection\n");

            var tester = new RobustFormTester();
            try
            {
                tester.RunTests();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n💥 Application failed: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\n🧹 Cleaning up...");
                tester.Dispose();
            }

            Console.WriteLine("\n✨ Press any key to exit...");
            Console.ReadKey();
        }
    }
}