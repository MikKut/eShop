using System.Net;
using System;
using FluentAssertions;
using Infrastructure.Identity;

namespace PetShop.IntegrationTests;

[Scope("integrationtesting.test")]
public class PageTests
{
    [Fact]
    public async void CatalogFirstPageTest_ShouldRetunCatalogPage_WhenServiceIsRunning()
    {
        //Arrange
        var client = new HttpClient();

        //Act
        var defaultPage = await client.GetAsync("http://www.petshop.com:81/?page=0&itemsPage=6");
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        // Assert
        defaultPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async void BasketPageTest_ShouldRetunErrorPage_WhenServiceIsRunning()
    {
        //Arrange
        var client = new HttpClient();

        //Act
        var defaultPage = await client.GetAsync("http://www.petshop.com:81/Basket");
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        // Assert

        defaultPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async void CommitPurchasesPageTest_ShouldRetunErrorPage_WhenServiceIsRunning()
    {
        //Arrange
        var client = new HttpClient();

        //Act
        var commitPurchasesPage = await client.PostAsync("http://www.petshop.com:81/Basket/CommitPurchases",null);
        var content = await HtmlHelpers.GetDocumentAsync(commitPurchasesPage);

        // Assert

        commitPurchasesPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async void LoginPageTest_ShouldRetunLoginPage_WhenServiceIsRunning()
    {
        //Arrange
        var client = new HttpClient();

        //Act
        var commitPurchasesPage = await client.GetAsync("http://www.petshop.com:5002/Account/Login");
        var content = await HtmlHelpers.GetDocumentAsync(commitPurchasesPage);

        // Assert

        commitPurchasesPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async void AddToBasketPageTest_ShouldRetunErrorPage_WhenServiceIsRunning()
    {
        //Arrange
        var client = new HttpClient();

        //Act
        var addBasketErrorPage = await client.GetAsync("http://www.petshop.com:81/Basket/AddToBasket/9");
        var content = await HtmlHelpers.GetDocumentAsync(addBasketErrorPage);

        // Assert 
        addBasketErrorPage.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Children.Count().Should().BeGreaterThanOrEqualTo(1);
    }
}