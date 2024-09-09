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

#### Account APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Account.png?alt=media&token=15552b0e-6f32-4cce-b02f-84e022c6b482)

#### Category APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Category.png?alt=media&token=fc974269-25ea-4a67-8be1-3b2df67a6626)

#### Country APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Country.png?alt=media&token=4c3d51ea-5ecd-4a5e-89c1-0f818bf16e37)

#### Owner APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Owner.png?alt=media&token=02fd0cbe-c041-4fd6-a5b6-df88136884fd)

#### Pokemon APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Pokemon.png?alt=media&token=4e6ae8e5-608c-47b7-ae43-9ca703ba230e)

#### Review APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Review.png?alt=media&token=ade9c67a-4131-4f37-a66c-e6704c4917d5)

#### Reviewer APIs

![image](https://firebasestorage.googleapis.com/v0/b/pokemon-net-core-api.appspot.com/o/Reviewer.png?alt=media&token=e77339cf-fab5-45fe-a7b5-5f256836779b)

### Reference Resources

[ASP.NET Web API Tutorial 2022](https://www.youtube.com/playlist?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0)

[pokemon-review-api Repository](https://github.com/teddysmithdev/pokemon-review-api)
