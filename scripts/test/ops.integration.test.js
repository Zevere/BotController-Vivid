const $ = require('shelljs');
const path = require('path');
require('../logging')

const root = path.resolve(`${__dirname}/../..`)
$.config.fatal = true


console.info(`running Ops integration tests`)

console.debug('starting a Docker container for MongoDB')
const container_id = $.exec(
    `docker run --rm --detach --publish 27017:27017 --name borzoo-mongo-test mongo`
).stdout.trim()

console.info('done')

// const commands = [
//         `dotnet build`,
//         `dotnet xunit -configuration Release -stoponfail -verbose --fx-version 2.1.4`
//     ]
//     .reduce((prev, curr) => `${prev} && ${curr}`, 'echo')

// `docker run --rm --tty webapi-borzoo:solution-debug`

// try {
//     $.exec(
//         `docker run --rm --tty ` +
//         `--volume "${root}:/project" ` +
//         `--workdir /project/test/Borzoo.Data.Tests.Mongo/ ` +
//         `--link borzoo-mongo-test ` +
//         `--env "MONGO_CONNECTION=mongodb://borzoo-mongo-test:27017/borzoo-test" ` +
//         `microsoft/dotnet:2.1.402-sdk ` +
//         `sh -c "${commands}"`
//     )
// } finally {
//     console.debug(`removing the Mongo container`)
//     $.exec(`docker rm --volumes --force "${container_id}"`)
// }
