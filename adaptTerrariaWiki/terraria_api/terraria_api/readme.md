Welcome to the Terraria API documentation!
This API provides a set of endpoints for interacting with the Terraria game. Below is a brief overview of things you should know to make changes in the database.
## Getting Started
1. **Database Setup**: Set up the database by running the provided migration scripts. This will create the necessary tables and seed the database with initial data.
open the package manager console and run the following command:
```bash
Update-Database
```

When you made changes in the models do the following to update the database:
```bash
Add-Migration <MigrationName>
Update-Database
```