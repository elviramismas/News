# News Project Info

Technologies:
- .NET 8
- PostgreSQL

Prerequisites to run the project (in docker)
- docker

How to run the project
1. open terminal in root where docker-compose.yaml is located
2. run command "docker compose up -d"
3. wait for all containers to run
4. open browser on localhost:8080/swagger to access swagger
5. in docker compose is included pgadmin and can be launched by opening localhost:8888 in browser, username and password are in docker-compose file

When run, application creates database schema and seeds two initial users.
- user with "Reader" role, email: reader@email.com and password: Reader123!
- user with "Author" role, email: author@email.com and password: Author123!

Notes: 
- stuff in docker-compose like connection string, usernames, passwords should be stored in a secure keyvault like azure key vault
- for easier running of app, database is created on application startup and some initial data is seeded, that would not be desirable in production


Prerequisites to run the project (in IDE)
- .NET 8
- specify connection string in appsettings.Development.json

Possible feature improvements:
- create custom registration endpoint that will support role assignment to user on creation
- improve pagination to also provide count of all items
- add more unit tests
