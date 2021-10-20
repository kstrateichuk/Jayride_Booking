using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Jayride_Booking
{
    public class JayrideSearch
    {
        private readonly ChromeDriver chromeDriver;
        public JayrideSearch()
        {
             //Set up config for the driver
            var driver = new DriverManager().SetUpDriver(new ChromeConfig());
            chromeDriver = new ChromeDriver(Environment.CurrentDirectory);
        }

        [Theory]
        [InlineData("46 Church Street, Parramatta")]
        public void SearchResult(string address)
        {
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromMinutes(1));
            //Open the URL
            chromeDriver.Navigate().GoToUrl("https://www.jayride.com/airport-transfer/australia/sydney-airport-syd");
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div/div[1]/div[5]/div[2]/div[2]/a")).Displayed);
            
            // TODO: Select the second Quote Card
            IWebElement secondQuoteCard = chromeDriver.FindElement(By.XPath("/html/body/div[2]/div[1]/div/div[1]/div[5]/div[2]/div[2]/a"));
            secondQuoteCard.Click();
            
            // TODO: Wait for navigation to SPA
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[1]/div[2]/div/jr-location-autocomplete/div/div/div/input")));
            
            // TODO: Enter missing value
            IWebElement toField = chromeDriver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[1]/div[2]/div/jr-location-autocomplete/div/div/div/input"));
            toField.SendKeys(address);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[1]/div[2]/div/jr-location-autocomplete/div/div/div/typeahead-container")));
            toField.SendKeys(Keys.Enter);
           
            // TODO: Click on the find tranfer button
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[2]/div/div[3]/button")).Enabled);
            IWebElement updateButton = chromeDriver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[2]/div/div[3]/button"));
            updateButton.Click();
            
            // TODO: Verify that search results are shown
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[4]/div[1]/div/div/div/div/div[1]/div[1]")).Displayed);
           
            // TODO: Verify that minimum price is 118 AUD
            // Click on the expand button
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[4]/div[1]/div/div/div/div/div[2]/jr-quote-group[1]/div/div[2]/button")).Enabled);
            IWebElement expandButtonSedan = chromeDriver.FindElement(By.CssSelector("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[4]/div[1]/div/div/div/div/div[2]/jr-quote-group[1]/div/div[2]/button"));
            expandButtonSedan.Click();
            
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[4]/div[1]/div/div/div/div/div[2]/jr-quote-group[1]/div/div[2]/button")).Enabled);
            IWebElement expandButtonSUVs = chromeDriver.FindElement(By.CssSelector("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[4]/div[1]/div/div/div/div/div[2]/jr-quote-group[2]/div/div[2]/button"));
            expandButtonSUVs.Click();

            // Check the price 
            List<IWebElement> prices = new List<IWebElement>(chromeDriver.FindElements(By.CssSelector("price ng-star-inserted")));
            Assert.True(prices.Where(currency => currency.Text.IndexOf("AU$") >= 0 && Int16.Parse(currency.Text.Split(' ')[1]) == 118).ToArray().Length > 0);
            chromeDriver.Close();
        }
    }
}
