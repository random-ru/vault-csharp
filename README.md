<!-- Logo -->
<p align="center">
  <a href="#">
    <img height="128" width="128" src="https://raw.githubusercontent.com/random-ru/vault-csharp/master/src/icon.png">
  </a>
</p>

<!-- Name -->
<h1 align="center">
  ✨ Vault library for C# ✨
</h1>
<p align="center">
  <a href="#">
    <img alr="MIT License" src="https://img.shields.io/:license-MIT-blue.svg">
  </a>
  <a href="https://www.nuget.org/packages/ivy.vault/">
    <img alt="Nuget" src="https://img.shields.io/nuget/v/ivy.vault.svg?color=%23884499">
  </a>
  <a href="https://t.me/ivysola">
    <img alt="Telegram" src="https://img.shields.io/badge/Ask%20Me-Anything-1f425f.svg">
  </a>
</p>
<p align="center">
  <a href="#">
    <img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg">
    <img src="https://forthebadge.com/images/badges/designed-in-ms-paint.svg">
    <img src="https://forthebadge.com/images/badges/ages-18.svg">
    <img src="https://ForTheBadge.com/images/badges/winter-is-coming.svg">
    <img src="https://forthebadge.com/images/badges/gluten-free.svg">
  </a>
</p>


Remark:       
  `This project is part of a project Vault`    


### Install   
`dotnet add package ivy.vault --version 1.0.0`


# API


### Get Field

```CSharp
// app: market
// space: coop
// fullpath: /@/coop/market/prices
// auth: NO

using var vault = Vault
    .Create("vault.api")
    .Space("coop")
    .App("market");

var prices = await vault.FieldAsync<Price[]>("prices");
```

### Update Field


```CSharp
// app: market
// space: coop
// fullpath: /@/coop/market/features
// auth: NO

using var vault = Vault
    .Create("vault.api")
    .Space("coop")
    .App("market");

var features = await vault.FieldAsync<List<FeatureFlag>>("features");

features.RemoveWhen(x => x.name == "task");


await vault.UpdateAsync("features", features);

```

### With autorization

```CSharp
// app: market
// space: coop
// fullpath: /@/coop/market/prices
// auth: YES

using var vault = Vault
    .Create("vault.api")
    .Space("coop")
    .WithToken(SPACE_TOKEN)
    .App("market")
    .WithToken(APP_TOKEN);

var prices = await vault.FieldAsync<Price[]>("prices");
```


### Errors

```js
class VaultBadRequestException   // Application or Field path not found.
class VaultAccessDeniedException // Access Code is not correct.
```