### Database design

find the db design at file: ERD-CarWorkShop.png in route path

### How to Run Backend Application:

- Prepare database connection and database schema
- Update setup configuration at appsettings.json for database connection and smtpSetting (email) based on your own configuration.
- Build the solution and make sure no error founds
- Run application and the application will display endpoints lists.
- Execute endpoint /api/Systems/SeedData only one times to seeding initial Data

default data:

- Admin: admin@mailtrap.io | temp123
- Mechanic: mechan@mailtrap.io | temp123
- CarOwner: budi@mailtrap.io | temp123

Step to manually produce Data:

- create user admin at endpoint /api/Accounts/create
- create mechanic at endpoint api/Mechanics
- create service
- create car (include car owner & repair)
- create job (assign job to mehanic include service & car)
- update job (if completed)
- update repair (if all job completed)

### How to Migrate Database

```
dotnet ef migrations add Migration-Names --project CarWorkshopSystem.Infrastructure --startup-project CarWorkshopSystem.WebAPI
```

dotnet ef database update --project CarWorkshopSystem.Infrastructure --startup-project CarWorkshopSystem.WebAPI

```
dotnet ef migrations remove Migration-Names --project CarWorkshopSystem.Infrastructure --startup-project CarWorkshopSystem.WebAPI
```
