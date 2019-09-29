﻿using System;
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

        private ChromeDriver driver;

        public VacanciesTest()
        {
            this.driver = new ChromeDriver();
            driver.Url = "https://careers.veeam.com/";
        }

        [TestMethod]
        public void Nubmer_Of_Vacancies_Should_Be_Correct()
        {
            var classname = "vacancies-blocks-col";

            driver.Manage().Window.Maximize();
            var countryDropdown = driver.FindElement(By.Id("country-element"));
            countryDropdown.Click();
            var countryList = driver.FindElement(By.XPath("//*[@id=\"country-element\"]/div/div/div[2]"));
            var value = countryList.FindElement(By.XPath($"span[text()='{Country}']"));
            value.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Timeout);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".animate.vacancies-blocks-item")));
            var vacanciesContainer = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("vacancies-blocks-container")));
            var vacancies = vacanciesContainer.FindElements(By.ClassName(classname));
            var total = vacancies.Count;

            var allJobsButton = driver.FindElement(By.XPath("//*[@id=\"index-vacancies-buttons\"]/div/a"));
            if (allJobsButton.Displayed)
            {
                allJobsButton.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".animate.vacancies-blocks-item")));
                //vacanciesContainer = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"hp\"]/section[3]/div/div[1]/div[3]")));
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                vacanciesContainer = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"hp\"]/section[3]/div/div[1]/div[3]")));
                var additionalVacancies = vacanciesContainer.FindElements(By.ClassName(classname));
                total += additionalVacancies.Count;
            }

            Assert.AreEqual(ExpectedVacancyNumber, total);

            TearDown();
        }
        private void TearDown()
        {
            this.driver.Close();
        }
    }
}