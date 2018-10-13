const $ = require('shelljs');
const path = require('path');
require('../logging')

$.config.fatal = true
const root = path.resolve(`${__dirname}/../..`)


console.info(`running Vivid Operations systems integration tests`)
try {
    console.debug('starting test dependencies. docker-compose project: "ops"')
    $.cd(`${root}/test/Ops.IntegrationTests`)
    $.exec(`docker-compose --project-name ops up -d --force-recreate --remove-orphans`)

    try {
        console.debug('running tests')

        const settings = JSON.stringify(JSON.stringify({
            MongoConnection: "mongodb://mongo/vivid-tests-ops",
            ZevereApiEndpoint: "http://borzoo/zv/GraphQL"
        }))

        $.exec(
            `docker run --rm --tty ` +
            `--workdir /project/test/Ops.IntegrationTests/ ` +
            `--env "VIVID_TEST_SETTINGS=${settings}" ` +
            `--network ops_borzoo-network ` +
            `vivid:debug ` +
            `dotnet test --no-build --verbosity normal`
        )
    } finally {
        console.debug('removing test dependency containers via docker-compose')
        $.exec(`docker-compose --project-name ops rm -fv`)
    }
} catch (e) {
    console.error(e)
    process.exit(1)
}
