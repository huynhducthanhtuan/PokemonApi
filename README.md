## PokemonApi

Review and rate your favorite pokemons

### Installation Guide

#### 1. Clone project

```bash
git clone https://github.com/huynhducthanhtuan/PokemonApi.git
```

#### 2. Import SQL Server database from `pokemon_db.bak` file

#### 3. Update SQL Server database connection string

`appsettings.json`

```bash
"ConnectionStrings": {
  "DefaultConnection": "Data Source=THANHTUAN;Initial Catalog=pokemon_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
}
```

##### Change `THANHTUAN` with your computer name

#### 4. Build project

```bash
dotnet build
```

#### 5. Run project

```bash
dotnet watch run
```

<details><summary><b>Exception when step 2 fails</b></summary>

#### 1. Install dotnet-ef if not already

```bash
dotnet tool install --global dotnet-ef
```

#### 2. Create a migration

```bash
dotnet-ef migrations add Init
```

#### 3. Update database definition from migration

```bash
dotnet-ef database update
```

#### 4. Seeding data

```bash
dotnet run seeddata
```

</details>

### Demonstration

#### Category APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Category.png)

#### Country APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Country.png)

#### Owner APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Owner.png)

#### Pokemon APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Pokemon.png)

#### Review APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Review.png)

#### Reviewer APIs

![image](https://shopee-hdttuan.web.app/pokemon-api-images/Reviewer.png)

### Reference Resources

[ASP.NET Web API Tutorial 2022](https://www.youtube.com/playlist?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0)

[pokemon-review-api Repository](https://github.com/teddysmithdev/pokemon-review-api)
