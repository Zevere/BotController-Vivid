# Vivid Operations - Systems Integration Tests

## Getting Started

1. Start the dependency services
    ```sh
    docker-compose --project-name ops up -d --remove-orphans
    ```
1. Run the tests. You can see the log of services using this command:
    ```sh
    docker-compose --project-name ops logs --follow
    ```
1. Remove the containers
    ```sh
    docker-compose --project-name ops stop
    docker-compose --project-name ops rm -fv
    ```
