using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebDriver
{
    public class EpamPageTests
    {
        //Arrange
        [TestCase(".NET")]
        public void TestCase1(string searchText)
        {
            // Setup EdgeDriver
            using var driver = new EdgeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.epam.com/");

            // Click Careers Button
            var careersParent = driver.FindElement(By.CssSelector("ul.top-navigation__row > li:nth-child(5)"));
            var careers = careersParent.FindElement(By.PartialLinkText("Careers"));
            careers.Click();

            // Search Keyword
            var keyword = driver.FindElement(By.Id("new_form_job_search-keyword"));
            keyword.SendKeys(searchText);

            // Select All Locations
            var locations = driver.FindElement(By.CssSelector("span.select2-selection__arrow"));
            locations.Click();
            var allLocations = driver.FindElement(By.XPath("//li[normalize-space(text())='All Locations']"));
            allLocations.Click();

            // Check Remote Option
            var remoteParent = driver.FindElement(By.CssSelector("div.job-search__filter-list"));
            var remote = remoteParent.FindElement(By.XPath("//label[normalize-space(text())='Remote']"));
            remote.Click();

            // Click Find Button
            var findForm = driver.FindElement(By.Id("jobSearchFilterForm"));
            var findCareers = findForm.FindElement(By.TagName("button"));
            findCareers.Click();

            // Click View And Apply Button
            var latestResult = wait.Until(driver => driver.FindElement(By.XPath("//ul[@class='search-result__list']/child::li[1]")));
            var viewAndApply = latestResult.FindElement(By.LinkText("VIEW AND APPLY"));
            viewAndApply.Click();

            //Assert Head Article Contains Test Parameter
            var careersArticle = driver.FindElement(By.ClassName("vacancy-details-23__job-title")).Text;
            Assert.That(careersArticle, Does.Contain(searchText));
        }

        //Arange
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void TestCase2(string keyword)
        {
            // Setup EdgeDriver
            using var driver = new FirefoxDriver();
            var actions = new Actions(driver);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.epam.com/");

            // Click Magnifier Icon
            var magnifier = driver.FindElement(By.CssSelector("span.header-search__search-icon"));
            magnifier.Click();

            // Search Keywords
            var searchParent = driver.FindElement(By.ClassName("header-search__panel"));
            var search = searchParent.FindElement(By.Name("q"));
            search.SendKeys(keyword);

            // Click Find Button
            var find = driver.FindElement(By.CssSelector("div.search-results__input-holder+button"));
            find.Click();

            // Scroll To The Last Link
            var lastLink = driver.FindElement(By.XPath("//div[@class='search-results__items']/article[last()]"));
            actions.ScrollByAmount(0, lastLink.Location.Y).Perform();

            // Check Is Last Element List Is Displayed
            bool isLastLinkDisplayed = driver.FindElement(By.XPath("//div[@class='search-results__items']/article[20]")).Displayed;

            // Query All The Links
            var links = driver.FindElements(By.XPath("//div[@class='search-results__items']//article/h3/a"));

            // Query All The Links Contain Seach Keyword 
            var allLinksContainKeyword = links.All(link => link.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (keyword == "Cloud")
            {
                // Assert That All The Links Contain Seach Keyword "Cloud"
                Assert.That(allLinksContainKeyword, Is.True);

                // Assert That All 20 links Are Displayed
                Assert.That(isLastLinkDisplayed, Is.True);
            }
            else
            {
                // Assert That Not All The Links Contain Seach Keyword "BLOCKCHAIN" and "Automation"
                Assert.That(allLinksContainKeyword, Is.Not.True);

                // Assert That All 20 links Are Displayed
                Assert.That(isLastLinkDisplayed, Is.True);
            }
        }
    }
}
