## PokemonApi

#### 1. Clone project

```bash
$ git clone https://github.com/huynhducthanhtuan/PokemonApi.git
```

#### 2. Change directory to folder PokemonApi

```bash
$ cd PokemonApi
```

#### 3. Build project

```bash
$ dotnet build
```

#### 4. Install dotnet-ef if not already (Optional)

```bash
$ dotnet tool install --global dotnet-ef
```

#### 5. Create a migration (Optional)

```bash
$ dotnet-ef migrations add Init
```

#### 6. Update database definition from migration

```bash
$ dotnet-ef database update
```

#### 7. Seeding data (Optional)

```bash
$ dotnet run seeddata
```

#### 8. Run project

```bash
$ dotnet watch run
```
