const $ = require('shelljs');
const path = require('path');
require('../logging')

$.config.fatal = true
const root = path.resolve(`${__dirname}/../..`)

console.info(`running Vivid's Postman tests`)

try {
    console.debug('starting test dependencies. docker-compose project: "postman"')
    $.cd(`${root}/test/Postman/`)
    $.exec(`docker-compose up -d --force-recreate --remove-orphans`)

    try {
        console.debug('running tests')

        $.exec('sleep 5')

        $.exec(
            `node ${root}/scripts/node_modules/newman/bin/newman.js run ` +
            `--iteration-count 3 ` +
            `--bail ` +
            `--environment BotOps.Development.postman_environment.json ` +
            `BotOps.postman_collection.json`
        )
    } finally {
        console.debug('removing test dependency containers via docker-compose')
        $.exec(`docker-compose rm --stop -fv`)
    }
} catch (e) {
    console.error(e)
    process.exit(1)
}
