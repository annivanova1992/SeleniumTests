using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumSampleExample
{
    [TestClass]
    public class VacanciesTest
    {
        private const string Country = "Romania";
        private const int ExpectedVacancyNumber = 33;

        private const int Timeout = 10;

        private static ChromeDriver driver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            driver = new ChromeDriver();
            driver.Url = "https://careers.veeam.com/";
        }

        [TestMethod]
        public void Number_Of_Vacancies_Should_Be_Correct()
        {
            var classname = "vacancies-blocks-col";

            driver.Manage().Window.Maximize();
            var countryDropdown = driver.FindElement(By.Id("country-element"));
            countryDropdown.Click();
            var countryList = driver.FindElement(By.XPath("//*[@id=\"country-element\"]/div/div/div[2]"));
            var value = countryList.FindElement(By.XPath($"span[text()='{Country}']"));
            value.Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".animate.vacancies-blocks-item")));
            var vacanciesContainer = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("vacancies-blocks-container")));
            var vacancies = vacanciesContainer.FindElements(By.ClassName(classname));
            var total = vacancies.Count;

            var allJobsButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"index-vacancies-buttons\"]/div/a")));
            if (allJobsButton.Displayed)
            {
                allJobsButton.Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".animate.vacancies-blocks-item")));
                vacanciesContainer = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"hp\"]/section[3]/div/div[1]/div[3]")));
                var additionalVacancies = vacanciesContainer.FindElements(By.ClassName(classname));
                total += additionalVacancies.Count;
            }

            Assert.AreEqual(ExpectedVacancyNumber, total);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
