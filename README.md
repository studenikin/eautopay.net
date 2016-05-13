# E-Autopay API Client Library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/a60j9gjal78heahf/branch/master?svg=true)](https://ci.appveyor.com/project/studenikin/eautopay-net/branch/master)

![logo](eautopay-logo.png)

EAutopay.NET is a client library targeting .NET that provides an easy
way to interact with the [E-Autopay](http://e-autopay.com/) service.

## Usage examples

Get product from E-Autopay.

```c#
var repository = new EAutopayProductRepository();
var product = repository.Get(1234);
Console.WriteLine("Name: " + product.Name + " Price: " + product.Price);
```
Create new product in E-Autopay.

```c#
var product = new Product();
product.Name = "Some nice DVD";
product.Price = 99.99;

var repository = new EAutopayProductRepository();
repository.Save(product);
```
Bind an upsell to a product in E-Autopay.

```c#
int originId = 1234; // id of the product the upsell is based on
int parentId = 4321; // id of the product the upsell is bound to

var upsell = new Upsell();
upsell.OriginID = originId;
upsell.Price = 500.99;
upsell.SuccessUri = "http://myecommerce.com/upsells/success";

var repository = new EAutopayUpsellRepository();
repository.Save(upsell, parentId);
```
