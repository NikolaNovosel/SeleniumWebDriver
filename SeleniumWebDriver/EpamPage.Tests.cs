using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebDriver
{
    public class EpamPageTests
    {
        //Arrange
        [TestCase("C#")]
        public void TestCase1(string searchText)
        {
            //Act
            using var driver = new EdgeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.epam.com/");
            var careersParent = driver.FindElement(By.CssSelector("ul.top-navigation__row > li:nth-child(5)"));
            var careers = careersParent.FindElement(By.PartialLinkText("Careers"));
            careers.Click();
            var keyword = driver.FindElement(By.Id("new_form_job_search-keyword"));
            keyword.SendKeys(searchText);
            var location = driver.FindElement(By.CssSelector("span.select2-selection__arrow"));
            location.Click();
            var allLocations = driver.FindElement(By.XPath("//li[normalize-space(text())='All Locations']"));
            allLocations.Click();
            var remoteParent = driver.FindElement(By.CssSelector("div.job-search__filter-list"));
            var remote = remoteParent.FindElement(By.XPath("//label[normalize-space(text())='Remote']"));
            remote.Click();
            var findForm = driver.FindElement(By.Id("jobSearchFilterForm"));
            var findCareers = findForm.FindElement(By.TagName("button"));
            findCareers.Click();
            var latestResult = wait.Until(driver => driver.FindElement(By.XPath("//ul[@class='search-result__list']/child::li[1]")));
            var viewAndApply = latestResult.FindElement(By.LinkText("VIEW AND APPLY"));
            viewAndApply.Click();

            //Assert
            var careersArticle = driver.FindElement(By.ClassName("vacancy-details-23__job-title"));
            Assert.That(careersArticle.Text, Does.Contain(searchText));
        }

        //Arange
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void TestCase2(string keyword)
        {
            //Act
            using var driver = new FirefoxDriver();
            var actions = new Actions(driver);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.epam.com/");
            var magnifier = driver.FindElement(By.CssSelector("span.header-search__search-icon"));
            magnifier.Click();
            var searchParent = driver.FindElement(By.ClassName("header-search__panel"));
            var search = searchParent.FindElement(By.Name("q"));
            search.SendKeys(keyword);
            var find = driver.FindElement(By.CssSelector("div.search-results__input-holder+button"));
            find.Click();
            var lastLink = driver.FindElement(By.XPath("//div[@class='search-results__items']/article[last()]"));
            actions.ScrollByAmount(0, lastLink.Location.Y).Perform();

            //Assert
            bool isLastLinkDisplayed = driver.FindElement(By.XPath("//div[@class='search-results__items']/article[20]")).Displayed;
            var links = driver.FindElements(By.XPath("//div[@class='search-results__items']//article/h3/a"));
            var keywordLinkCount = links.Count(link => link.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            Assert.That(keywordLinkCount != links.Count, Is.True);
            Assert.That(isLastLinkDisplayed, Is.True);
        }
    }
}
