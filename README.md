# Vivid

Web API for chat bot operations(BotOps) in Zevere

## Getting Started

This project requires .NET Core SDK 2.1 and a Mongo database instance.

1. First, clone this repository
    ```sh
    # Clone the repository
    git clone https://github.com/Zevere/BotOps-Vivid.git

    # Switch to the project directory
    cd BotOps-Vivid

    # Clone dependency projects
    git submodule update --init --recursive
    ```
1. Run a Mongo database instance. For example, using Docker:
    ```sh
    docker run --detach --publish 27017:27017 --name vivid-mongo mongo
    ```
1. Run the web app
    ```sh
    dotnet run src/Vivid.Web/Vivid.Web.csproj
    ```
1. [http://localhost:5000/api/docs/swagger](http://localhost:5000/api/docs/swagger)
