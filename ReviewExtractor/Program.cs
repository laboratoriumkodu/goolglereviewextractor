using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;

class GooglePlayScraper
{
    static void Main()
    {
        Console.WriteLine("Enter the application id, e.g. for the Revoult application it will be com.revolut.revolut and for the IKO application it will be pl.pkobp.iko and press <enter>");
        var appId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(appId))
            return;

        appId = appId.Trim();

        var url = $"https://play.google.com/store/apps/details?id={appId}&hl=en&gl=us";

        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        using (var driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl(url);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement reviewsTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@aria-label, 'See more information on Ratings and reviews')]")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", reviewsTab);
            Thread.Sleep(2000);

            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[@class='odk6He']")));



            IWebElement menu = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@aria-label, 'Most relevant') and contains(@role, 'button')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", menu);
            Thread.Sleep(2000);

            IWebElement newestOption = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[contains(@aria-label, 'Newest') and contains(@role, 'menuitemradio')]")));
            newestOption.Click();
            Thread.Sleep(2000);

            IWebElement newest2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@aria-label, 'Newest') and contains(@role, 'button')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", newest2);
            Thread.Sleep(2000);


            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            for (int i = 0; i < 20; i++) // Scroll multiple times
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "document.querySelector('div.fysCi.Vk3ZVd').scrollBy(0, 2500);");
                Thread.Sleep(2000);
            }

            var reviewElements = driver.FindElements(By.CssSelector("div.RHo1pe"));

            int countr = 0;
            using (StreamWriter writer = new StreamWriter($"{appId}.txt"))
            {

                foreach (var review in reviewElements)
                {
                    try
                    {
                        string rating = review.FindElement(By.CssSelector("div.iXRFPc")).GetAttribute("aria-label");
                        string date = review.FindElement(By.CssSelector("span.bp9Aid")).Text;
                        string text = review.FindElement(By.CssSelector("div[class='h3YV2d']")).Text;

                        string helpful = "0";
                        try
                        {
                            helpful = review.FindElement(By.CssSelector("div.AJTPZc")).Text;
                        }
                        catch { }

                        if (date.EndsWith("2025"))
                        {
                            Console.WriteLine($"Rating: {rating}");
                            Console.WriteLine($"Date: {date}");
                            Console.WriteLine($"Review: {text}");
                            Console.WriteLine($"Helpful Count: {helpful}");
                            Console.WriteLine(new string('-', 40));

                            string output = $"Rating: {rating}\nDate: {date}\nReview: {text}\nHelpful: {helpful}\n{"-".PadLeft(60, '-')}\n";
                            writer.WriteLine(output);
                            countr++;
                        }
                    }
                    catch { continue; }
                }
            }

            driver.Quit();
            Console.WriteLine(countr);
            Console.ReadLine();
        }
    }
}
